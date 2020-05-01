using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CenterTable : NetworkBehaviour {

    // Use this for initialization
    public  void Start() {

        if (isServer)
        {
            TableEditorDataHolder tableEditorManager = GameObject.FindGameObjectWithTag("TableEditorDataHolder").GetComponent<TableEditorDataHolder>();


            tableEditorManager.CenterTable.SpawnEntities(this.gameObject, null);
        }
        /*
        GameObject[] zones = GameObject.FindGameObjectsWithTag("Zone");
        List<GameSaveDataHolder.CardSaveData> tempList = GameSaveDataHolder.Instance.data.cardDataList;
        if (tempList == null) UnityEngine.Debug.Log("templist ures");
        if (GameSaveDataHolder.Instance == null) UnityEngine.Debug.Log("instance ures");
        int i = 0;
        foreach (GameObject item in zones)
        {
            foreach (GameSaveDataHolder.CardSaveData carddata in tempList)
            {
                i++;
                UnityEngine.Debug.Log(i);
                UnityEngine.Debug.Log("jéj");
                CardView card = GameObject.Find(carddata.cardName).GetComponent<CardView>();
                UnityEngine.Debug.Log("jéjután");
                if (item.transform.position == carddata.zonePosition)
                    item.GetComponent<Zone>().tempMethod(card);
            }
        }
        */
    }

}
