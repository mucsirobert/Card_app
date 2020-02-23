using MogoEngine.UISystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogPanel : Dialog<LogPanel> {

    [SerializeField]
    private DataGrid dataGrid;
    //private LoopVerticalScrollRect scrollRect;

    [SerializeField]
    private Scrollbar scrollbar;

    [SerializeField]
    private LogEntryView entryPrefab;

    public static void Show()
    {
        var dialog = Create(MenuManager.Instance.logPanelPrefab);

        //Instance.InitEntries();
        /*Instance.scrollRect.totalCount = LogManager.Instance.Entries.Count;
        Instance.scrollRect.RefillCells();*/
        dialog.dataGrid.SetItemsData(dialog.entryPrefab.gameObject, LogManager.Instance.Entries.Count, dialog.UpdateItem);
        dialog.dataGrid.ShowViewBottom();

    }

    private void UpdateItem(ItemBase t, int index)
    {
        t.SetData(LogManager.Instance.Entries[index]);
    }


    /*private void InitEntries()
    {

        foreach (var entry in LogManager.Instance.Entries)
        {
            LogEntryView newEntry = Instantiate(entryPrefab, entryList.transform);
            newEntry.InitView(entry);
        }

    }*/

    public void Start()
    {
        //scrollbar.value = 0;

    }

    public void OnCancelPressed()
    {
        //This is needed to fix the bug when log is not scrolled to the bottom after opening the log again
        dataGrid.ShowViewTop();
        Close();
    }


}
