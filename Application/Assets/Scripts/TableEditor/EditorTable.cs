using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTable : MonoBehaviour {

    public string tableName;

	// Use this for initialization
	void Start () {
		
	}
	

    public void Save(TableData tableData)
    {
        //tableData.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<EditorEntity>().Save(tableData);
        }
    }

    public void DestroyChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
