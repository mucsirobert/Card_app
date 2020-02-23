using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerListDiscovery : NetworkDiscovery {

    public delegate void OnReceivedBroadcastDelegate(LocalServerInfo server);
    public OnReceivedBroadcastDelegate onReceivedBroadcast;

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);
        string ipAddress = fromAddress.Split(new char[] { ':' }, 4)[3];
        var server = JsonUtility.FromJson<LocalServerInfo>(data);
        server.ip = ipAddress;

        if (onReceivedBroadcast != null)
            onReceivedBroadcast(server);
    }
}
