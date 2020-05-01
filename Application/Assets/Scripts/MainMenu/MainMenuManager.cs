using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class MainMenuManager : MonoBehaviour, INetworkEventReceiver {
    [SerializeField]
    private LobbyNetworkManager lobbyNetworkManager;

    [SerializeField]
    private GameObject mainMenuPanel;

    [SerializeField]
    private JoinGameMenu joinGameMenu;

    [SerializeField]
    private LobbyPanel lobbyPanel;

    [SerializeField]
    private LobbyInfoPanel infoPanel;

    private bool isHosting;

    private string ipAddress;

    public void OnPlayAndHostPressed()
    {
       
        lobbyNetworkManager.StartHost();
        
    }

    public void OnJoinGamePressed()
    {
        HideMainMenu();
        joinGameMenu.Show(JoinServer, ShowMainMenu);
    }

    public void JoinServer(string ip)
    {
        ipAddress = ip;
        infoPanel.Display("Connecting to "+  ip, "Cancel", () => { lobbyNetworkManager.StopClient(); });
        lobbyNetworkManager.JoinServer(ip);

        //Change to lobbypanel

        //DisplayIsConnecting();
    }

    public void OnPlaySceneLoaded()
    {
       lobbyPanel.Hide();
    }

    public void OnLobbySceneLoaded()
    {
        if (isHosting)
        {
            lobbyPanel.Show(() =>
            {
                ShowMainMenu();
                lobbyNetworkManager.StopHost();
            });
        }
        else
        {
            lobbyPanel.Show(() =>
            {
                ShowMainMenu();
                lobbyNetworkManager.StopClient();
            });
        }
    }

    public void OnStartHost(string ip)
    {
        ipAddress = ip;
        isHosting = true;
        HideMainMenu();
        lobbyPanel.Show(() =>
        {
            ShowMainMenu();
            lobbyNetworkManager.StopHost();
        });
    }

    public void OnStopHost()
    {
        isHosting = false;
        lobbyPanel.Hide();
        ShowMainMenu();
    }

    public void OnConnectToServer(string ip)
    {
        infoPanel.Hide();
        joinGameMenu.Hide();
        lobbyPanel.Show(() =>
        {
            ShowMainMenu();
            lobbyNetworkManager.StopClient();
        });
    }

    public void OnDisconnectFromServer(NetworkError error)
    {
        lobbyPanel.Hide();
        ShowMainMenu();
        if (error != NetworkError.Ok)
        {
            infoPanel.Display("Error : " + error.ToString(), "Close", null);
        }
    }

    public void OnClientError(int errorCode)
    {
        infoPanel.Display("Cient error : " + (errorCode == 6 ? "timeout" : errorCode.ToString()), "Close", null);
    }

    public class SerializableObjects {

        //public List<MainMenuPlayer> playerList = LobbyPlayerList.Instance.getMainMenuPlayerList();



        //  public static TableEditorDataHolder tedh ;
        public Stack<CommandStackEntry> redoStack = CommandProcessor.Instance.getStackToSave();
       /*
        public void loadForSave() {
            foreach (Player p in Player.Players)
            {
                Debug.Log(p.playerName);
                playerList.Add(p);
            }

          //  tedh = TableEditorDataHolder.Instance;
         //   undoStack = CommandProcessor.Instance.getUndoStack();
        }

        public List<Player> getPlayerList() {
           /* List<Player> getter = new List<Player>();
            foreach (Player p in playerList)
            {
                getter.Add(p);
            }
            return playerList;
        }*/
    }

    public void OnIngameExitPressed()
    {
        if (isHosting)
        {
            QuestionPanel.Show("Are you sure you want to exit to the lobby?", () => {
                QuestionPanel.Show("Do you want to save the game?", () => { // ez a QuestionPanel YesAction-je
                    /*
                    string fileName = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss")+".txt";
                    string path = Application.dataPath;
                    
                    Debug.Log(path);

                    DirectoryInfo di = new DirectoryInfo(Path.Combine(path, "Games"));
                    if (!di.Exists)
                        di.Create();

                    
                    Debug.Log(Path.Combine(di.FullName, fileName));
                    Debug.Log("Mentve");
                    var sr = File.CreateText(Path.Combine(di.FullName, fileName));
                    //sr.WriteLine(JsonUtility.ToJson(tables));
                    JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    // SerializableObjects so = new SerializableObjects();
                    //so.loadForSave();
                    // sr.WriteLine(JsonConvert.SerializeObject(CommandProcessor.Instance.getStackToSave(), settings));
                   
                    sr.WriteLine(JsonConvert.SerializeObject(gsdh, settings));
                    sr.Close();
                    */

                    GameSaveDataHolder gsdh = GameObject.FindGameObjectWithTag("GameSaveDataHolder").GetComponent<GameSaveDataHolder>();
                    gsdh.SaveToJSON();

                    lobbyNetworkManager.ChangeToLobbyScene();
                    HideMainMenu();
                    lobbyPanel.Show(() =>
                    {
                        ShowMainMenu();
                        lobbyNetworkManager.StopHost();
                    });
                    

                }, () => { // ez a questionPanel NoAction-je

                    lobbyNetworkManager.ChangeToLobbyScene();
                    HideMainMenu();
                       lobbyPanel.Show(() =>
                         {
                             ShowMainMenu();
                             lobbyNetworkManager.StopHost();
                          });

                });
                
            }, null);
        } else
        {
            QuestionPanel.Show("Are you sure you want to leave the match?", () =>
            {
                lobbyNetworkManager.StopClient();
                lobbyPanel.Hide();
                ShowMainMenu();
            }, null);
        }
    }

    public void OnKickedFromServer()
    {
        infoPanel.Display("Kicked by Server", "Close", null);
    }

    private void ShowMainMenu()
    {
        mainMenuPanel.gameObject.SetActive(true);
    }
    private void HideMainMenu()
    {
        mainMenuPanel.gameObject.SetActive(false);
    }


}
