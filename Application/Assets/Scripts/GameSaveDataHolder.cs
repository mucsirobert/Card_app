using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//using System.Security.Policy;
using System.Collections.Specialized;


[Serializable]
public class GameSaveDataHolder : MonoBehaviour
{

    public HandZone handzone;
    public Table table;
    [SerializeField]
    public Player[] players;// = GameObject.FindGameObjectsWithTag("Player").GetComponent<Player>();
    [SerializeField]
    public TableEditorDataHolder.Tables tables;// = GameObject.FindGameObjectWithTag("TableEditorDataHolder").GetComponent<TableEditorDataHolder>();

    public static GameSaveDataHolder Instance { get; set; }

    [Serializable]
    public class CardSaveData {
        public string cardName;
        public Vector3 zonePosition;
        public string zoneID;
        public bool isHandZoneCard;

        public CardSaveData(string card, Vector3 position, string id, bool hand) {
            cardName = card;
            zonePosition = position;
            zoneID = id;
            isHandZoneCard = hand;
        }
    }

    [Serializable]
    public class DataSave
    {
        public TableEditorDataHolder.Tables tables;
        public List<CardSaveData> cardDataList;
        public List<String> playersNameList;
       // public List<CardView> horizontalZoneCardList;
       
        public void loadCards()
        {
            GameObject[] allCards = GameObject.FindGameObjectsWithTag("Card");
            foreach (GameObject item in allCards)
            {
                if (item.GetComponent<CardView>().currentZoneObject.name == "HandZone(Clone)")
                {
                    string handZoneOwner = getPlayerNameFromCard(item.GetComponent<CardView>());
                    cardDataList.Add(new CardSaveData(item.name, item.GetComponent<CardView>().currentZoneObject.transform.position, handZoneOwner, true));

                }
                else {
                    cardDataList.Add(new CardSaveData(item.name, item.GetComponent<CardView>().currentZoneObject.transform.position, item.GetComponent<CardView>().currentZoneObject.GetComponent<Zone>().zoneID, false));
                }
            }
           
        }

        public string getPlayerNameFromCard(CardView card)
        {
            foreach (Player player in Player.Players)
            {
               // if (player.getHandZone().transform.position == card.currentZoneObject.transform.position)
                if(GameObject.ReferenceEquals(player.getHandZone(), card.currentZoneObject))
                {
                    Debug.Log(player.playerName);
                    return player.playerName;
                }
            }

            return null;
        }

        //próba: egy listat csinálni egy card neve-zónájának helye alapján.

    }


    public DataSave data;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           // tabledata = GameObject.FindGameObjectWithTag("TableEditorDataHolder").GetComponent<TableEditorDataHolder>();
           // players = GameObject.FindGameObjectsWithTag("Player").GetComponent<Player>();
            TableEditorDataHolder tedh = GameObject.FindGameObjectWithTag("TableEditorDataHolder").GetComponent<TableEditorDataHolder>();
            tables = tedh.getTables();


            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    

    public void SaveToJSON() {
        string fileName = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + ".txt";
        string path = Application.dataPath;

        Debug.Log(path);

        DirectoryInfo di = new DirectoryInfo(Path.Combine(path, "Games"));
        if (!di.Exists)
            di.Create();

        TableEditorDataHolder tedh = GameObject.FindGameObjectWithTag("TableEditorDataHolder").GetComponent<TableEditorDataHolder>();
        tables = tedh.getTables();

        data.tables = tedh.getTables();
        data.loadCards();
    

        Debug.Log(Path.Combine(di.FullName, fileName));
        Debug.Log("Mentve");
        var sr = File.CreateText(Path.Combine(di.FullName, fileName));
        //sr.WriteLine(JsonUtility.ToJson(tables));
        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        // SerializableObjects so = new SerializableObjects();
        //so.loadForSave();
        // sr.WriteLine(JsonConvert.SerializeObject(CommandProcessor.Instance.getStackToSave(), settings));
       // GameSaveDataHolder gsdh = GameObject.FindGameObjectWithTag("GameSaveDataHolder").GetComponent<GameSaveDataHolder>();
        sr.WriteLine(JsonConvert.SerializeObject(data, settings));
        sr.Close();
    }

    public void LoadFromJSON(string dataAsJson) {
        //string dataAsJson = File.ReadAllText(Path.Combine(Application.dataPath, Path.Combine("Games", listOfGames[index].Name)));
        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        //TableEditorDataHolder.Tables tables = JsonConvert.DeserializeObject<TableEditorDataHolder.Tables>(dataAsJson, settings);
        DataSave dataa = JsonConvert.DeserializeObject<DataSave>(dataAsJson, settings);
        data = dataa;
        TableEditorDataHolder tableEditorManager = GameObject.FindGameObjectWithTag("TableEditorDataHolder").GetComponent<TableEditorDataHolder>();
        tableEditorManager.setTables(dataa.tables);
      

        Debug.Log("delegétmukodik");
    }
}
