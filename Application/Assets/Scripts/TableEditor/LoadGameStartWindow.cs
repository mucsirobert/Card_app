using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadGameStartWindow : MonoBehaviour
{

    [SerializeField]
    private Dropdown dropDown;

    public delegate void StringAction(string param);
    private UnityAction onNewLayoutSelected;
    private StringAction onEditLayoutSelected;
    private StringAction onLoadLayoutSelected;

    private StringAction onLoadGameSelected;
    List<string> gameFileNames;

    public void Show(UnityAction onNewLayoutSelected, StringAction onEditLayoutSelected, StringAction onLoadLayoutSelected)//StringAction onLoadGameSelected)
    {
        gameObject.SetActive(true);

        this.onNewLayoutSelected = onNewLayoutSelected;
        this.onEditLayoutSelected = onEditLayoutSelected;
        this.onLoadLayoutSelected = onLoadLayoutSelected;

        // this.onLoadGameSelected = onLoadGameSelected;

        DirectoryInfo dataDir = new DirectoryInfo(Path.Combine(Application.dataPath, "Games"));
        gameFileNames = new List<string>();
        List<string> gameFileNamesWithoutExtension = new List<string>();
        if (dataDir.Exists)
        {
            Debug.Log("hm");
            FileInfo[] fileinfos = dataDir.GetFiles();
            for (int i = 0; i < fileinfos.Length; i++)
            {
                gameFileNames.Add(fileinfos[i].Name);
                string name = Path.GetFileNameWithoutExtension(fileinfos[i].Name);
                gameFileNamesWithoutExtension.Add(name);
            }
        }
        else { Debug.Log("Nincs"); }
        if (gameFileNames.Count == 0)
        {
            dropDown.interactable = false;
        }
        else
        {
            dropDown.interactable = true;
            dropDown.ClearOptions();
            dropDown.AddOptions(gameFileNamesWithoutExtension);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


    public void OnLoadGamePressed()
    {
        if (onLoadGameSelected != null)
        {
            if (gameFileNames.Count != 0 && dropDown.value >= 0 && dropDown.value < gameFileNames.Count)
            {
                onLoadGameSelected(gameFileNames[dropDown.value]);
            }
        }
    }

    
}
