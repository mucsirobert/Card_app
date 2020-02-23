using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EditModeStartWindow : MonoBehaviour {

    [SerializeField]
    private Dropdown dropDown;

    public delegate void StringAction(string param);

    private UnityAction onNewLayoutSelected;
    private StringAction onEditLayoutSelected;
    private StringAction onLoadLayoutSelected;
    List<string> layoutFileNames;

	public void Show(UnityAction onNewLayoutSelected, StringAction onEditLayoutSelected, StringAction onLoadLayoutSelected)
    {
        gameObject.SetActive(true);
        this.onNewLayoutSelected = onNewLayoutSelected;
        this.onEditLayoutSelected = onEditLayoutSelected;
        this.onLoadLayoutSelected = onLoadLayoutSelected;

        DirectoryInfo dataDir = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "Layouts"));
        layoutFileNames = new List<string>();
        List<string> layoutFileNamesWithoutExtension = new List<string>();
        if (dataDir.Exists)
        {
            FileInfo[] fileinfos = dataDir.GetFiles();
            for (int i = 0; i < fileinfos.Length; i++)
            {
                layoutFileNames.Add(fileinfos[i].Name);
                string name = Path.GetFileNameWithoutExtension(fileinfos[i].Name);
                layoutFileNamesWithoutExtension.Add(name);
            }
        }
        if (layoutFileNames.Count == 0)
        {
            dropDown.interactable = false;
        } else
        {
            dropDown.interactable = true;
            dropDown.ClearOptions();
            dropDown.AddOptions(layoutFileNamesWithoutExtension);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnNewLayoutPressed()
    {
        if (onNewLayoutSelected != null)
        {
            onNewLayoutSelected();
        }
    }

    public void OnLoadLayoutPressed()
    {
        if (onLoadLayoutSelected != null)
        {
            if (layoutFileNames.Count != 0 && dropDown.value >= 0 && dropDown.value < layoutFileNames.Count)
            {
                onLoadLayoutSelected(layoutFileNames[dropDown.value]);
            }
        }
    }

    public void OnEditLayoutPressed()
    {
        if (onEditLayoutSelected != null)
        {
            if (layoutFileNames.Count != 0 && dropDown.value >= 0 && dropDown.value < layoutFileNames.Count)
            {
                onEditLayoutSelected(layoutFileNames[dropDown.value]);
            }
        }
    }
}
