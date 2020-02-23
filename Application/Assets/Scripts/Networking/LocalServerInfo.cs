using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LocalServerInfo
{
    public string name;
    public int currentSize;
    public int maxSize;
    public string ip;

    public LocalServerInfo(string name, int currentSize, int maxSize, string ip)
    {
        this.name = name;
        this.currentSize = currentSize;
        this.maxSize = maxSize;
        this.ip = ip;
    }
}
