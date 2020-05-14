using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

    public HandZone handPrefab;
    public Button playerNameButtonPrefab;
    public Table tablePrefab;

    //String used for loading back data from JSON to the right player
    public string whereToLoad;

    [SyncVar]
    private GameObject handObject;

    [SyncVar]
    private GameObject tableObject;

    private HandZone hand;
    public Table Table { get; private set; }

    [SyncVar]
    public string playerName;

    [SyncVar]
    public Color playerColor;

    public static Player LocalPlayer { get; private set; }

    public static List<Player> Players { get; private set; }

    private bool onStartClientCalled;

    private void Start()
    {
        //This is to prevent bug, when Start is called before OnStartClient
        StartCoroutine(DelayedInit());

    }

    private void Init()
    {
        //Tis method is called after OnStartLocalPlayer
        if (!isLocalPlayer)
        {
            //The zone's on the other player's table are mirrored
            MirrorOtherPlayerTable();
        }

        var uiManagerObject = GameObject.FindGameObjectWithTag("UIManager");

        if (uiManagerObject == null)
        {
            Debug.LogError("There is no UIManager in the scene!");
        }
        else
        {
            var uiManager = uiManagerObject.GetComponent<UndoRedoUI>();
            uiManager.CreatePlayerButton(this);
            if (isLocalPlayer)
            {
                uiManager.OwnTable = Table.gameObject;
            }
        }


        if (Players == null)
        {
            Players = new List<Player>();
        }
        Players.Add(this);

    }

    public override void OnStartServer()
    {
        SpawnHandAndTable();
    }

    public override void OnStartClient()
    {
        //Need to call SetHandAndTable here, because OnStartClient is called before every Start method
        SetHandAndTable();
        Debug.Log("Player OnStartClient");
        onStartClientCalled = true;
    }

    private IEnumerator DelayedInit()
    {
        while (!onStartClientCalled)
        {
            yield return null;
        }
        Init();

    }   

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        //Override the table and hand position for this client
        SetOwnHandAndTable();

        LocalPlayer = this;
        
    }

    public void OnDestroy()
    {
        Player.Players.Remove(this);
    }

    public GameObject getHandZone() {
        return handObject;
    }

    void SpawnHandAndTable()
    {
        //hand.transform.localPosition.Set(0, -2.7f, 0);
        hand = Instantiate(handPrefab, this.transform);
        //hand.parent = this.gameObject;
        hand.ownerNetId = this.netId;
        hand.zoneName = playerName + "'s hand";
        hand.zoneColor = this.playerColor;


        NetworkServer.Spawn(hand.gameObject);
        handObject = hand.gameObject;

        Table = Instantiate(tablePrefab, this.transform);
        NetworkServer.Spawn(Table.gameObject);
        tableObject = Table.gameObject;

        TableEditorDataHolder tableEditorManager = GameObject.FindGameObjectWithTag("TableEditorDataHolder").GetComponent<TableEditorDataHolder>();

        tableEditorManager.PlayerTable.SpawnEntities(Table.gameObject, this);
    }

    //[ClientRpc]
    private void SetHandAndTable()
    {
        //Set the hand and table on the client just connected
        //This applies to all clients, including the local one. Need to call SetOwnHandAndTable() to set the local client's table and hand
        if (tableObject != null)
        {
            Table = tableObject.GetComponent<Table>();
            Table.transform.SetParent(this.transform);

            Table.transform.position = new Vector3(0, 10, 0);
        }

        if (handObject != null)
        {
            hand = handObject.GetComponent<HandZone>();
          
            hand.transform.SetParent(Table.transform);

            //This will be mirrored later
            hand.transform.localPosition = new Vector3(0, -3.5f, -0.5f);
            hand.HiddenPosition = new Vector3(0, 5.8f, -0.5f);
            hand.ShowPosition = new Vector3(0, 3.5f, -0.5f); ;
            hand.IsOwnHand = false;
            
        }
    }

    private void SetOwnHandAndTable()
    {
        if (Table != null)
        {
            Table.transform.SetParent(this.transform);

            Table.transform.position = new Vector3(0, -10, 0);
           
        }

        if (hand != null)
        {
            hand.transform.SetParent(Camera.main.transform);
            hand.transform.localPosition = new Vector3(0, -3.5f, 8);
            hand.IsOwnHand = true;
            hand.HiddenPosition = new Vector3(0, -5.8f, 8);
            hand.ShowPosition = hand.transform.localPosition;

        }

        

    }

    private void MirrorOtherPlayerTable()
    {
        //We mirror the zones' position on the other player's table
        foreach (Transform t in Table.transform)
        {
            Vector3 newPosition = new Vector3(-t.localPosition.x, -t.localPosition.y, t.localPosition.z);
            t.localPosition = newPosition;
        }

       
    }

    public void DealCardToPlayer(CardView card)
    {
        hand.DropCardOnClientsOnTop(card);
    }















}
