using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LobbyNetworkManager : NetworkLobbyManager
{
    static short MsgKicked = MsgType.Highest + 1;

    static public LobbyNetworkManager s_Singleton;


    [Header("Unity UI Lobby")]
    [Tooltip("Time in second between all players ready & match start")]
    public float prematchCountdown = 5.0f;

    [Space]
    [Header("UI Reference")]

    public LocalServerList discovery;


    //Client numPlayers from NetworkManager is always 0, so we count (throught connect/destroy in LobbyPlayer) the number
    //of players, so that even client know how many player there is.
    [HideInInspector]
    public int _playerNumber = 0;

    protected LobbyHook _lobbyHooks;

    [SerializeField]
    private LobbyPlayerList playerList;

    [SerializeField]
    private GameObject networkEventReceiverObject;

    private INetworkEventReceiver networkEventReceiver;


    void Start()
    {
        //This will run when the game starts, so put this here to prevent devices to go to sleep mode
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        connectionConfig.NetworkDropThreshold = 90;

        networkEventReceiver = networkEventReceiverObject.GetComponent<INetworkEventReceiver>();

        s_Singleton = this;
        _lobbyHooks = GetComponent<LobbyHook>();

        GetComponent<Canvas>().enabled = true;

        DontDestroyOnLoad(gameObject);
    }


    public void JoinServer(string ip)
    {
        //ChangeTo(lobbyPanel);

        networkAddress = ip;
        StartClient();

        //lobbyManager.backDelegate = lobbyManager.StopClientClbk;
        //lobbyManager.DisplayIsConnecting();

        //lobbyManager.SetServerInfo("Connecting...", lobbyManager.networkAddress);
    }

    public override void OnLobbyClientSceneChanged(NetworkConnection conn)
    {
        if (SceneManager.GetSceneAt(0).name == lobbyScene)
        {
            networkEventReceiver.OnLobbySceneLoaded();
        } else
        {
            networkEventReceiver.OnPlaySceneLoaded();
        }
    }


    // ----------------- Server management

    class KickMsg : MessageBase { }
    public void KickPlayer(NetworkConnection conn)
    {
        conn.Send(MsgKicked, new KickMsg());
    }


    public void KickedMessageHandler(NetworkMessage netMsg)
    {
        //infoPanel.Display("Kicked by Server", "Close", null);
        networkEventReceiver.OnKickedFromServer();
        netMsg.conn.Disconnect();
    }

    //===================

    public override NetworkClient StartHost()
    {
        //Need to override this and not OnStartHost(), because NetworkManager calls OnStartHost() first, and then starts the server
        var ret = base.StartHost();

        /*ChangeTo(lobbyPanel);
        backDelegate = StopHostClbk;
        SetServerInfo("Hosting", networkAddress);
        editButton.SetActive(true);*/

        networkEventReceiver.OnStartHost(networkAddress);
        discovery.StartBroadcast();

        return ret;
    }

    public override void OnStopHost()
    {
        base.OnStopHost();

        networkEventReceiver.OnStopHost();
        discovery.StopBroadcast();
    }

    public void ChangeToLobbyScene()
    {
        ServerChangeScene(lobbyScene);
    }


    // ----------------- Server callbacks ------------------

    //we want to disable the button JOIN if we don't have enough player
    //But OnLobbyClientConnect isn't called on hosting player. So we override the lobbyPlayer creation
    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;

        MainMenuPlayer newPlayer = obj.GetComponent<MainMenuPlayer>();
        newPlayer.OnRemoveButtonPressed.AddListener(KickPlayer);
        /*newPlayer.ToggleJoinButton(numPlayers + 1 >= minPlayers);


        for (int i = 0; i < lobbySlots.Length; ++i)
        {
            MainMenuPlayer p = lobbySlots[i] as MainMenuPlayer;

            if (p != null)
            {
                p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
            }
        }*/

        return obj;
    }

    public override void OnLobbyServerConnect(NetworkConnection conn)
    {
        CheckReadyToBegin();
    }

    public override void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId)
    {
        /*for (int i = 0; i < lobbySlots.Length; ++i)
        {
            MainMenuPlayer p = lobbySlots[i] as MainMenuPlayer;

            if (p != null)
            {
                p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
            }
        }*/
    }

    public override void OnLobbyServerDisconnect(NetworkConnection conn)
    {
        CheckReadyToBegin();
        /* for (int i = 0; i < lobbySlots.Length; ++i)
         {
             MainMenuPlayer p = lobbySlots[i] as MainMenuPlayer;

             if (p != null)
             {
                 p.ToggleJoinButton(numPlayers >= minPlayers);
             }
         }*/

    }

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        //This hook allows you to apply state data from the lobby-player to the game-player
        //just subclass "LobbyHook" and add it to the lobby object.

        if (_lobbyHooks)
            _lobbyHooks.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);

        return true;
    }

    // --- Countdown management

    public override void OnLobbyServerPlayersReady()
    {
       /* bool allready = true;
        for (int i = 0; i < lobbySlots.Length; ++i)
        {
            if (lobbySlots[i] != null)
                allready &= lobbySlots[i].readyToBegin;
        }

        if (allready)*/
            StartCoroutine(ServerCountdownCoroutine());
    }

    public IEnumerator ServerCountdownCoroutine()
    {
        float remainingTime = prematchCountdown;
        int floorTime = Mathf.FloorToInt(remainingTime);

        while (remainingTime > 0)
        {
            yield return null;

            remainingTime -= Time.deltaTime;
            int newFloorTime = Mathf.FloorToInt(remainingTime);

            if (newFloorTime != floorTime)
            {//to avoid flooding the network of message, we only send a notice to client when the number of plain seconds change.
                floorTime = newFloorTime;

                for (int i = 0; i < lobbySlots.Length; ++i)
                {
                    if (lobbySlots[i] != null)
                    {//there is maxPlayer slots, so some could be == null, need to test it before accessing!
                        (lobbySlots[i] as MainMenuPlayer).RpcUpdateCountdown(floorTime);
                    }
                }
            }
        }

        for (int i = 0; i < lobbySlots.Length; ++i)
        {
            if (lobbySlots[i] != null)
            {
                (lobbySlots[i] as MainMenuPlayer).RpcUpdateCountdown(0);
            }
        }

        ServerChangeScene(playScene);
    }

    // ----------------- Client callbacks ------------------

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        //infoPanel.gameObject.SetActive(false);

        conn.RegisterHandler(MsgKicked, KickedMessageHandler);

        if (!NetworkServer.active)
        {//only to do on pure client (not self hosting client)
            /*ChangeTo(lobbyPanel);
            backDelegate = StopClientClbk;
            SetServerInfo("Client", networkAddress);*/

            networkEventReceiver.OnConnectToServer(networkAddress);
        }
    }


    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        //ChangeTo(mainMenuPanel);
        networkEventReceiver.OnDisconnectFromServer(conn.lastError);
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        /*ChangeTo(mainMenuPanel);
        infoPanel.Display("Cient error : " + (errorCode == 6 ? "timeout" : errorCode.ToString()), "Close", null);*/
        networkEventReceiver.OnClientError(errorCode);
    }

}
	


