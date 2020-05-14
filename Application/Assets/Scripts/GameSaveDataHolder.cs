using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections.Specialized;


[Serializable]
public class GameSaveDataHolder : MonoBehaviour
{

    public HandZone handzone;
    public Table table;
    [SerializeField]
    public Player[] players;
    [SerializeField]
    public TableEditorDataHolder.Tables tables;

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
               
                if(GameObject.ReferenceEquals(player.getHandZone(), card.currentZoneObject))
                {
                    return player.playerName;
                }
            }

            return null;
        }

        

    }


    public DataSave data;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           
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
        string path = Application.persistentDataPath;

   

        DirectoryInfo di = new DirectoryInfo(Path.Combine(path, "Games"));
        if (!di.Exists)
            di.Create();

        TableEditorDataHolder tedh = GameObject.FindGameObjectWithTag("TableEditorDataHolder").GetComponent<TableEditorDataHolder>();
        tables = tedh.getTables();

        data.tables = tedh.getTables();
        data.loadCards();
        data.playersNameList = new List<string>();
        foreach (MainMenuPlayer player in LobbyPlayerList.Instance.getPlayers())
        {
            data.playersNameList.Add(player.playerName);
        }


        Debug.Log(Path.Combine(di.FullName, fileName));
        Debug.Log("Mentve");
        var sr = File.CreateText(Path.Combine(di.FullName, fileName));

        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
      
        sr.WriteLine(JsonConvert.SerializeObject(data, settings));
        
        sr.Close();
    }

    public void LoadFromJSON(string dataAsJson) {
        
        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        
        DataSave dataFromFile = JsonConvert.DeserializeObject<DataSave>(dataAsJson, settings);
        data = dataFromFile;
        TableEditorDataHolder tableEditorManager = GameObject.FindGameObjectWithTag("TableEditorDataHolder").GetComponent<TableEditorDataHolder>();
        tableEditorManager.setTables(dataFromFile.tables);
      
    }
}
