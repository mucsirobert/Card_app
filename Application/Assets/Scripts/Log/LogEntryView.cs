using System;
using UnityEngine;
using UnityEngine.UI;


public class LogEntryView : ItemBase { 

    [SerializeField]
    private Text textView;

    [SerializeField]
    private Text timeTextView;

    /*void ScrollCellIndex(int idx)
    {
        textView.text = LogManager.Instance.Entries[idx].Text;
    }*/


    public override void SetData(object data)
    {
        LogEntryData logEntryData = (LogEntryData)data;
        textView.text = logEntryData.Text;
        timeTextView.text = logEntryData.Time.ToString("HH:mm:ss");
    }

}


