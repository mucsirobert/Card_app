using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using DG.Tweening;
using System.IO;
using UnityEngine.Events;

public class EditModeManager : MonoBehaviour {
    public GameObject editModeManagerCanvas;
    public TablesHolder tablesHolder;

    public CanvasGroup bubbleMenu;

    public static EditModeManager Instance { get; set; }

    [SerializeField]
    private EditModeStartWindow startWindow;

    [SerializeField]
    private Text tableNameText;

    private string currenteditedFileName;

    private UnityAction backButtonDelegate;

    List<string> layoutFileNames;

    void Awake () {
        Instance = this;
	}

    private void Start()
    {
        bubbleMenu.transform.localScale = new Vector3(1, 0, 1);
    }

    private void OnDestroy()
    {
        Instance = null;
    }


    public void OnBackButtonPressed()
    {

        HideEditMode();
        if (backButtonDelegate != null)
            backButtonDelegate();
    }
   
    public void ShowEditMode(UnityAction backButtonDelegate)
    {
        this.backButtonDelegate = backButtonDelegate;
        editModeManagerCanvas.gameObject.SetActive(true);
        tableNameText.text = tablesHolder.ActiveTable.tableName;
        

        startWindow.Show(() =>
        {
            TextInputDialog.Show("New layout", (string text) =>
            {
                if (!String.IsNullOrEmpty(text))
                {
                    string fileName = text + ".json";
                    currenteditedFileName = fileName;
                    startWindow.Hide();
                    tablesHolder.Clear();
                }
            });
        },
        fileName => {
            currenteditedFileName = fileName;
            tablesHolder.LoadFromJson(fileName);
            startWindow.Hide();
        },
        fileName => {
            tablesHolder.LoadFromJson(fileName);
            tablesHolder.Apply();
            OnBackButtonPressed();
        });


    }

    public void HideEditMode()
    {
        tablesHolder.DestroyEntities();
        editModeManagerCanvas.gameObject.SetActive(false);
    }

    public void SpawnEditorEntity(EditorEntity entity)
    {
       

        var spawnedEntity = Instantiate(entity, tablesHolder.ActiveTable.transform);
        //spawnedEntity.gameObject.SetActive(true);
        
        spawnedEntity.transform.position = new Vector3(0, 0, 0f);
    }
    
    public void OnApplyPressed()
    {
        tablesHolder.Apply();
    }

    public void OnLoadPressed()
    {

    }


    public void OnSwithTablePressed()
    {
        tablesHolder.SwitchTable();
        tableNameText.text = tablesHolder.ActiveTable.tableName;
    }

    public void OnSpawnButtonPressed(EditorEntity entity)
    {
        SpawnEditorEntity(entity);
    }

    public void OnClearTablePressed()
    {
        tablesHolder.Clear();
    }

    public void ToggleBubbleMenu(bool state)
    {
        bubbleMenu.DOKill();
        if (state)
        {
            bubbleMenu.transform.DOScaleY(1, 0.5f).SetEase(Ease.OutBack);
            DOTween.To(() => bubbleMenu.alpha, x => bubbleMenu.alpha = x, 1f, 0.3f).SetEase(Ease.InOutQuart);
        } else
        {
            bubbleMenu.transform.DOScaleY(0, 0.5f).SetEase(Ease.InOutQuart);
            DOTween.To(() => bubbleMenu.alpha, x => bubbleMenu.alpha = x, 0f, 0.3f).SetEase(Ease.InOutQuart);
        }
    }

    public void OnSaveButtonPressed()
    {
        
        tablesHolder.SaveToJson(currenteditedFileName);
        
    }

    public void OnSaveAsButtonPressed()
    {
        TextInputDialog.Show("Save layout as", (string text) =>
        {
            if (!String.IsNullOrEmpty(text))
            {
                string fileName = text + ".json";
                tablesHolder.SaveToJson(fileName);
                currenteditedFileName = fileName;
            }
        });
    }






}
