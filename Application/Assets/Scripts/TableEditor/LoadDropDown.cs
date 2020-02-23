using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadDropDown : MonoBehaviour {

    private Dropdown dropDown;

	// Use this for initialization
	void Start () {
        dropDown = GetComponent<Dropdown>();
    }

    public void Init(string directoryName)
    {
        dropDown = GetComponent<Dropdown>();
        LoadFileNames(directoryName);
    }

    private void LoadFileNames(string path)
    {
        DirectoryInfo dataDir = new DirectoryInfo(path);
        if (dataDir.Exists)
        {

            FileInfo[] fileinfo = dataDir.GetFiles();
            for (int i = 0; i < fileinfo.Length; i++)
            {
                string name = fileinfo[i].Name;
                dropDown.options.Add(new Dropdown.OptionData(name));

                dropDown.RefreshShownValue();
                //Debug.Log("name  " + name);
            }
        }
        
    }

    public string GetSelectedFileName()
    {
        return dropDown.options[dropDown.value].text;
    }
}
