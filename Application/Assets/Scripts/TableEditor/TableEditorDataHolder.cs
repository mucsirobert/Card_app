using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TableEditorDataHolder : MonoBehaviour  {

    public DeckEditor deckEditorPrefab;
    public DeckView deckPrefab;

    public SingleZoneEditor singleZoneEditorPrefab;
    public SingleZone singleZonePrefab;

    public HorizontalZoneEditor horizontalZoneEditorPrefab;
    public HorizontalZone horizontalZonePrefab;

    [Serializable]
    public class Tables
    {
        public TableData centerTable;
        public TableData playerTable;
    }

    [SerializeField]
    private Tables tables;

    public TableData CenterTable { get { return tables.centerTable; } private set { tables.centerTable = value; } }
    public TableData PlayerTable { get { return tables.playerTable; } private set { tables.playerTable = value; } }

    public static TableEditorDataHolder Instance { get; set; }


    /*private void OnDestroy()
    {
        Instance = null;
    }*/

    public Tables getTables() {
        return tables;
    }

    public void setTables(Tables tbls) {
        tables = tbls;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        

        CenterTable = new TableData();
        PlayerTable = new TableData();

    }

    public void SaveToJson(string fileName)
    {
        DirectoryInfo di = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "Layouts"));
        if (!di.Exists)
            di.Create();

        Debug.Log(Path.Combine(Application.persistentDataPath, Path.Combine("Layouts", fileName)));
        var sr = File.CreateText(Path.Combine(Application.persistentDataPath, Path.Combine("Layouts", fileName)));
        //sr.WriteLine(JsonUtility.ToJson(tables));
        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        sr.WriteLine(JsonConvert.SerializeObject(tables, settings));

        sr.Close();
    }

    public void LoadFromJson(string filenName)
    {

        string dataAsJson = File.ReadAllText(Path.Combine(Application.persistentDataPath, Path.Combine("Layouts", filenName)));

        //tables = JsonUtility.FromJson<Tables>(dataAsJson);
        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        tables = JsonConvert.DeserializeObject<Tables>(dataAsJson, settings);

        Debug.Log("Loaded");
    }
	

    public void Clear()
    {
        CenterTable.Clear();
        PlayerTable.Clear();
    }
}
