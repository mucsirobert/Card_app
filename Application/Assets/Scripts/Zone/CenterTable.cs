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

    }

}
