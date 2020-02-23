using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DeckSettings : Dialog<DeckSettings>
{

    [SerializeField]
    private Dropdown deckInfoDp;

    [SerializeField]
    private Button cancelBtn;

    [SerializeField]
    private Button okBtn;

    [SerializeField]
    private Button saveBtn;

    private DeckMeta deckMeta;

    [SerializeField]
    private DecksFile decksFile;

    private DeckLayout currentDeckLayout;

    public CardList cardList;

    private DeckEditor currentDeck;


    void Start()
    {
        deckInfoDp.AddOptions(decksFile.GetDeckNames());
        deckInfoDp.value = currentDeckLayout.deckIndex;
        deckInfoDp.RefreshShownValue();

        deckInfoDp.onValueChanged.AddListener((int index) =>
        {
            deckMeta = Instantiate(decksFile.decks[index].deckMeta);
            currentDeckLayout = deckMeta.defaultDeckLayout.Copy();
            cardList.Load(deckMeta, currentDeckLayout.cardNums);
        });

    }

    public static void Show(DeckEditor deck)
    {
        var dialog = Create(MenuManager.Instance.deckSettingsPrefab);
        dialog.currentDeck = deck;
        //Need to copy these, or the Scriptable object asset will also be changed
        dialog.deckMeta = Instantiate(deck.DeckMeta);
        dialog.currentDeckLayout = deck.DeckLayout.Copy(); ;

        dialog.cardList.Load(dialog.deckMeta, dialog.currentDeckLayout.cardNums);
    }


    /*public static DeckMeta LoadDeckInfoFromFile(string fileName)
    {
        string dataAsJson = null;
        string filePath = Path.Combine(Application.streamingAssetsPath, Path.Combine("DeckInfos", fileName));
        #if UNITY_EDITOR || UNITY_IOS || UNITY_STANDALONE_WIN
        dataAsJson = File.ReadAllText(filePath);

        #elif UNITY_ANDROID
        WWW reader = new WWW (filePath);
        while (!reader.isDone) {
        }
        dataAsJson = reader.text;
        #endif

        return JsonUtility.FromJson<DeckMeta>(dataAsJson);
    }*/


    public void OnLoadDeckLayoutButtonPressed()
    {
        DirectoryInfo dataDir = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "DeckLayouts"));
        List<string> fileNames = new List<string>();
        if (dataDir.Exists)
        {
            FileInfo[] fileinfos = dataDir.GetFiles();
            for (int i = 0; i < fileinfos.Length; i++)
            {
                string name = Path.GetFileNameWithoutExtension(fileinfos[i].Name);
                fileNames.Add(name);

            }
            DropdownDialog.Show(fileNames, "Select a file", (int index) =>
            {
                currentDeckLayout = LoadDeckLayoutFromFile(fileinfos[index].Name);

                deckMeta = decksFile.decks[currentDeckLayout.deckIndex].deckMeta;

                cardList.Load(deckMeta, currentDeckLayout.cardNums);
            });

        }


    }

    public static DeckLayout LoadDeckLayoutFromFile(string fileName)
    {
        string dataAsJson = File.ReadAllText(Path.Combine(Application.persistentDataPath, Path.Combine("DeckLayouts", fileName)));
        return JsonUtility.FromJson<DeckLayout>(dataAsJson);
    }

    /*public static DeckData LoadDefaultDeckDataFromFile(string fileName)
    {
        string dataAsJson = null;

        string filePath = Path.Combine(Application.streamingAssetsPath, Path.Combine("DeckLayouts", fileName));
        #if UNITY_EDITOR || UNITY_IOS || UNITY_STANDALONE_WIN
        dataAsJson = File.ReadAllText(filePath);

        #elif UNITY_ANDROID
        WWW reader = new WWW (filePath);
        while (!reader.isDone) {
        }
        dataAsJson = reader.text;
        #endif
        return JsonUtility.FromJson<DeckData>(dataAsJson);
    }*/



    public void OnCancelPressed()
    {
        Close();
    }


    public void OnOkButtonPressed()
    {
        currentDeckLayout.cardNums = cardList.getNumberOfEachCard();
        currentDeck.DeckLayout = currentDeckLayout;
        currentDeck.DeckMeta = deckMeta;
        Close();
    }

    public void OnSaveButtonPressed()
    {
        TextInputDialog.Show("Save deck layout", (string text) =>
        {
            if (!String.IsNullOrEmpty(text))
            {
                string fileName = text + ".json";
                DeckLayout deckLayoutToSave = currentDeckLayout;
                deckLayoutToSave.cardNums = cardList.getNumberOfEachCard();

                SaveDeckLayoutToFile(fileName, deckLayoutToSave);
            }
        });

    }

    public static void SaveDeckLayoutToFile(string fileName, DeckLayout deckData)
    {
        DirectoryInfo di = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "DeckLayouts"));
        if (!di.Exists)
            di.Create();

        var sr = File.CreateText(Path.Combine(Application.persistentDataPath, Path.Combine("DeckLayouts", fileName)));
        sr.WriteLine(JsonUtility.ToJson(deckData));

        sr.Close();
    }


}
