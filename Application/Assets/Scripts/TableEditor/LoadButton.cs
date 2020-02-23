using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour {
    public LoadDropDown dropDown;
    public TablesHolder tablesHolder;

    void Start()
    {
        string path = Path.Combine(Application.persistentDataPath, "Layouts");

        dropDown.Init(path);
    }

    public void LoadFileFromDropdown()
    {
        string fileName = dropDown.GetSelectedFileName();
        tablesHolder.LoadFromJson(fileName);
    }
	
}
