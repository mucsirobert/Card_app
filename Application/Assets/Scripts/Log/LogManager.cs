using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LogManager : NetworkBehaviour {
    private List<LogEntry> entries;

    [SerializeField]
    private LogPopUp logPopUp;

    /*public List<LogEntry> Entries
    {
        get
        {
            if (isServer)
                return entries;
            else
            {
                var list = new List<LogEntry>();
                foreach (var entry in entryDatas)
                {
                    list.Add(new LogEntry(entry));
                }
                return list;
            }
        }
    }*/

    public IList<LogEntryData> Entries
    {
        get
        {
            return entryDatas;
        }
    }
    private SyncListLogEntry entryDatas = new SyncListLogEntry();




    public static LogManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        entries = new List<LogEntry>();
       
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

    }

    private void OnDestroy()
    {
        Instance = null;
    }



    [Server]
    public void MakeEntryActive(LogEntry logEntry, bool v)
    {
        logEntry.Active = v;
        int index = entries.IndexOf(logEntry);
        var data = logEntry.Data;
        data.active = v;
        entryDatas[index] = data;
        entryDatas.Dirty(index);
    }

    [Server]
    public void AddEntry(LogEntry entry)
    {
        entries.Add(entry);
        entryDatas.Add(entry.Data);
        //RpcAddEntry(entry.Text);
    }

    [ClientRpc]
    public void RpcShowLogPopup(string message)
    {
        logPopUp.ShowLastLog(message);
    }

    [ClientRpc]
    private void RpcAddEntry(string entry)
    {
        if (!isServer)
        {
            entries.Add(new LogEntry(entry));
        }

    }

    public class SyncListLogEntry : SyncListStruct<LogEntryData>
    {
    }

    
}
