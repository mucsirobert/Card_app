using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalServerList : MonoBehaviour {
    public RectTransform serverListRect;
    public LocalServerEntry serverEntryPrefab;
    public GameObject noServerFound;

    public ServerListDiscovery discovery;

    public Color OddServerColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    public Color EvenServerColor = new Color(.94f, .94f, .94f, 1.0f);

    private List<LocalServerInfo> serverInfos = new List<LocalServerInfo>();

    private JoinServerDelegate onJoinServer;

    void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        discovery.onReceivedBroadcast -= OnReceivedBroadcast;
        StopBroadcast();
    }

    public void Init(JoinServerDelegate onJoinServer)
    {
        this.onJoinServer = onJoinServer;

        foreach (Transform t in serverListRect)
            Destroy(t.gameObject);

        noServerFound.SetActive(false);

        discovery.onReceivedBroadcast += OnReceivedBroadcast;
        StartListening();

        ShowServers();
    }

    public void StartBroadcast()
    {
        if (discovery.running)
            discovery.StopBroadcast();

        //TODO
        var data = new LocalServerInfo("Test", 0, 0, "");

        discovery.broadcastData = JsonUtility.ToJson(data);
        discovery.Initialize();
        discovery.StartAsServer();
    }

    private void StartListening()
    {
        if (discovery.running)
            discovery.StopBroadcast();

        discovery.Initialize();
        discovery.StartAsClient();
    }

    public void StopBroadcast()
    {
        discovery.StopBroadcast();

    }

    private void OnReceivedBroadcast(LocalServerInfo serverInfo)
    {
        if (!serverInfos.Contains(serverInfo))
        {
            serverInfos.Add(serverInfo);
            ShowServers();
        }
    }


    private void ShowServers()
    {
        if (serverInfos.Count == 0)
        {

            noServerFound.SetActive(true);

            return;
        }

        noServerFound.SetActive(false);
        foreach (Transform t in serverListRect)
            Destroy(t.gameObject);

        for (int i = 0; i < serverInfos.Count; ++i)
        {
            LocalServerEntry serverEntry = Instantiate(serverEntryPrefab);

            serverEntry.Populate(serverInfos[i], (i % 2 == 0) ? OddServerColor : EvenServerColor, onJoinServer);

            serverEntry.transform.SetParent(serverListRect, false);
        }
    }
}
