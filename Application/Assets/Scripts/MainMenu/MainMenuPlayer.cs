using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class NetworkConnectionUnityEvent : UnityEvent<NetworkConnection> { }

//Player entry in the lobby. Handle selecting color/setting name & getting ready for the game
//Any LobbyHook can then grab it and pass those value to the game player prefab (see the Pong Example in the Samples Scenes)
public class MainMenuPlayer : NetworkLobbyPlayer
{
    static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
    //used on server to avoid assigning the same color to two player
    static List<int> _colorInUse = new List<int>();

    public Button colorButton;
    public InputField nameInput;
    public Button readyButton;
    public Button waitingPlayerButton;
    public Button removePlayerButton;

    //OnMyName function will be invoked on clients when server change the value of playerName
    [SyncVar(hook = "OnMyName")]
    public string playerName = "";
    [SyncVar(hook = "OnMyColor")]
    public Color playerColor = Color.white;

    [SerializeField]
    private GameObject waitingIcon;

    [SerializeField]
    private GameObject readyIcon;

    [SerializeField]
    private Text playerNameText;

    [SerializeField]
    private Image background;

    private LobbyPlayerList playerList;

    //I need this, because NetworkLobbyPlayer's readyToBegin is not synced when a player joins after one is already readyToBegin
    [SyncVar]
    private bool isPlayerReady;

    public NetworkConnectionUnityEvent OnRemoveButtonPressed { get; set; }

    private void Awake()
    {
        OnRemoveButtonPressed = new NetworkConnectionUnityEvent();
    }

    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();

        playerList = LobbyPlayerList.Instance;

        //if (LobbyNetworkManager.s_Singleton != null) LobbyNetworkManager.s_Singleton.OnPlayersNumberModified(1);

        playerList.AddPlayer(this);
        //playerList.DisplayDirectServerWarning(isServer && LobbyManager.s_Singleton.matchMaker == null);

        if (isLocalPlayer)
        {
            SetupLocalPlayer();
        }
        else
        {
            SetupOtherPlayer();
        }

        //setup the player data on UI. The value are SyncVar so the player
        //will be created with the right value currently on server
        OnMyName(playerName);
        OnMyColor(playerColor);

       /* if (isServer)
        {*/
        //}
    }

    public override void OnClientExitLobby()
    {
        base.OnClientExitLobby();

        //Set isPlayerReady to false on every client, and reset the plaeyr objects
        OnClientReady(false);
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        SetupLocalPlayer();
    }
    void SetupOtherPlayer()
    {
        removePlayerButton.gameObject.SetActive(NetworkServer.active);

        readyIcon.SetActive(false);
        waitingIcon.SetActive(true);
        playerNameText.gameObject.SetActive(true);
        nameInput.gameObject.SetActive(false);
        colorButton.gameObject.SetActive(false);

        readyButton.gameObject.SetActive(false);


        OnClientReady(isPlayerReady);
    }

    void SetupLocalPlayer()
    {
        nameInput.gameObject.SetActive(true);
        playerNameText.gameObject.SetActive(false);
        removePlayerButton.gameObject.SetActive(false);
        colorButton.gameObject.SetActive(true);
        readyButton.gameObject.SetActive(true);
        waitingPlayerButton.gameObject.SetActive(false);


        if (playerColor == Color.white)
            CmdColorChange();

        readyIcon.SetActive(false);
        waitingIcon.SetActive(true);

        //have to use child count of player prefab already setup as "this.slot" is not set yet
        if (playerName == "")
            CmdNameChanged("Player" + (playerList.playerListContentTransform.childCount));

        colorButton.interactable = true;
        nameInput.interactable = true;
        readyButton.interactable = true;

        nameInput.onEndEdit.RemoveAllListeners();
        nameInput.onEndEdit.AddListener(OnNameChanged);

        colorButton.onClick.RemoveAllListeners();
        colorButton.onClick.AddListener(OnColorClicked);

        readyButton.onClick.RemoveAllListeners();
        readyButton.onClick.AddListener(OnReadyClicked);

        //when OnClientEnterLobby is called, the loval PlayerController is not yet created, so we need to redo that here to disable
        //the add button if we reach maxLocalPlayer. We pass 0, as it was already counted on OnClientEnterLobby
        //if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(0);
    }


    public override void OnClientReady(bool readyState)
    {
        if (isServer)
        {
            isPlayerReady = readyState;
        }
        if (readyState)
        {
            readyIcon.SetActive(true);
            waitingIcon.SetActive(false);

            readyButton.gameObject.SetActive(false);
            waitingPlayerButton.gameObject.SetActive(isLocalPlayer);
            colorButton.interactable = false;
            nameInput.interactable = false;
        }
        else
        {
            readyIcon.SetActive(false);
            waitingIcon.SetActive(true);

            readyButton.gameObject.SetActive(isLocalPlayer);
            waitingPlayerButton.gameObject.SetActive(false);
            colorButton.interactable = isLocalPlayer;
            nameInput.interactable = isLocalPlayer;
        }


    }

  
    ///===== callback from sync var

    public void OnMyName(string newName)
    {
        playerName = newName;
        nameInput.text = playerName;
        playerNameText.text = playerName;
    }

    public void OnMyColor(Color newColor)
    {
        playerColor = newColor;
        colorButton.GetComponent<Image>().color = newColor;

        //We paint the background with the player's color with a little transparency
        var backgroundColor = playerColor;
        backgroundColor.a = 0.5f;
        background.color = backgroundColor;
    }

    //===== UI Handler

    //Note that those handler use Command function, as we need to change the value on the server not locally
    //so that all client get the new value throught syncvar
    public void OnColorClicked()
    {
        CmdColorChange();
    }

    public void OnReadyClicked()
    {
        SendReadyToBeginMessage();
    }

    public void OnNameChanged(string str)
    {
        CmdNameChanged(str);
    }

    public void OnRemovePlayerClick()
    {
        if (!isLocalPlayer && isServer)
        {
            OnRemoveButtonPressed.Invoke(connectionToClient);
        }
        /*else if (isServer)
            LobbyManager.s_Singleton.KickPlayer(connectionToClient);*/

    }

    [ClientRpc]
    public void RpcUpdateCountdown(int countdown)
    {
        /*LobbyManager.s_Singleton.countdownPanel.UIText.text = "Match Starting in " + countdown;
        LobbyManager.s_Singleton.countdownPanel.gameObject.SetActive(countdown != 0);*/
    }

    //====== Server Command

    [Command]
    public void CmdColorChange()
    {
        int idx = System.Array.IndexOf(Colors, playerColor);

        int inUseIdx = _colorInUse.IndexOf(idx);

        if (idx < 0) idx = 0;

        idx = (idx + 1) % Colors.Length;

        bool alreadyInUse = false;

        do
        {
            alreadyInUse = false;
            for (int i = 0; i < _colorInUse.Count; ++i)
            {
                if (_colorInUse[i] == idx)
                {//that color is already in use
                    alreadyInUse = true;
                    idx = (idx + 1) % Colors.Length;
                }
            }
        }
        while (alreadyInUse);

        if (inUseIdx >= 0)
        {//if we already add an entry in the colorTabs, we change it
            _colorInUse[inUseIdx] = idx;
        }
        else
        {//else we add it
            _colorInUse.Add(idx);
        }

        playerColor = Colors[idx];
    }

    [Command]
    public void CmdNameChanged(string name)
    {
        playerName = name;
    }

    //Cleanup thing when get destroy (which happen when client kick or disconnect)
    public void OnDestroy()
    {
        playerList.RemovePlayer(this);
        //if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(-1);

        int idx = System.Array.IndexOf(Colors, playerColor);

        if (idx < 0)
            return;

        for (int i = 0; i < _colorInUse.Count; ++i)
        {
            if (_colorInUse[i] == idx)
            {//that color is already in use
                _colorInUse.RemoveAt(i);
                break;
            }
        }
    }
}

