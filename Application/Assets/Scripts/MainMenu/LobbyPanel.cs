using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour {

    [SerializeField]
    private LobbyPlayerList playerList;

    [SerializeField]
    private Button editModeButton;

    [SerializeField]
    private Button loadGameButton;

    private EditModeManager editModeManager;


    private UnityAction onBackPressed;

    public void Show(UnityAction onBackPressed)
    {
        editModeButton.gameObject.SetActive(NetworkServer.active);
        loadGameButton.gameObject.SetActive(NetworkServer.active);
        editModeManager = EditModeManager.Instance;
        this.onBackPressed = onBackPressed;
        gameObject.SetActive(true);
    }

    public LobbyPlayerList getPlayerList() {
        return playerList;
    }

    public void OnEditModeButtonPressed()
    {
        Hide();
        editModeManager.ShowEditMode(() =>
        {
            Show(onBackPressed);
        });
    }

  //  private FileInfo[] listOfGames;// ezek kellenek 
 //   private MainMenuManager.SerializableObjects objectsForLoadGame;// a visszatolteshez

    public void OnLoadGameButtonPressed()
    {
      //  Hide();
      /*  editModeManager.ShowLoadGameMode(() =>
        {
            Show(onBackPressed);
        });*/
        


        MenuManager.Instance.loadGame();
    
    }

    public void OnBackPressed()
    {
        Hide();
        if (onBackPressed != null)
        {
            onBackPressed();
        }
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
