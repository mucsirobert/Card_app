using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TablesHolder : MonoBehaviour {

    private TableEditorDataHolder tableEditorDataHolder;

    public EditorTable centerTable;
    public EditorTable playerTable;

    private EditorTable activeTable;

    public EditorTable ActiveTable {
        get
        {
            return activeTable;
        }
        private set
        {
            activeTable = value;
            centerTable.gameObject.SetActive(false);
            playerTable.gameObject.SetActive(false);
            activeTable.gameObject.SetActive(true);
        }
    }


    // Use this for initialization
    void Start () {
        tableEditorDataHolder = GameObject.FindGameObjectWithTag("TableEditorDataHolder").GetComponent<TableEditorDataHolder>();

        
        ActiveTable = centerTable;
    }

    public void Apply()
    {
        tableEditorDataHolder.Clear();

        centerTable.Save(tableEditorDataHolder.CenterTable);
        playerTable.Save(tableEditorDataHolder.PlayerTable);
    }

    public void SaveToJson(string fileName)
    {

        Apply();

        tableEditorDataHolder.SaveToJson(fileName);


        Debug.Log("Saved");
    }
    public void LoadFromJson(string filenName)
    {
        Clear();

        tableEditorDataHolder.LoadFromJson(filenName);

        tableEditorDataHolder.CenterTable.SpawnEditorEntities(centerTable.gameObject);
        tableEditorDataHolder.PlayerTable.SpawnEditorEntities(playerTable.gameObject);
        tableEditorDataHolder.Clear();
        /*centerTable.Save(tableEditorDataHolder.CenterTable);
        playerTable.Save(tableEditorDataHolder.PlayerTable);*/
    }

    public void Clear()
    {
        tableEditorDataHolder.Clear();

        DestroyEntities();
    }

    public void DestroyEntities()
    {
        playerTable.DestroyChildren();
        centerTable.DestroyChildren();
    }

    public void SwitchTable()
    {
        if (ActiveTable == centerTable) ActiveTable = playerTable;
        else ActiveTable = centerTable;
    }

}
