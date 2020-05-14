using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;


//List of players in the lobby
public class LobbyPlayerList : MonoBehaviour
{
    public static LobbyPlayerList Instance { get; private set; }

    public RectTransform playerListContentTransform;
    //public GameObject warningDirectPlayServer;
    //public Transform addButtonRow;

    protected VerticalLayoutGroup _layout;
    protected List<MainMenuPlayer> players = new List<MainMenuPlayer>();

    private void Awake()
    {
        Instance = this;
    }

    public void OnEnable()
    {
        //_instance = this;
        _layout = playerListContentTransform.GetComponent<VerticalLayoutGroup>();
    }

    void Update()
    {
        //this dirty the layout to force it to recompute evryframe (a sync problem between client/server
        //sometime to child being assigned before layout was enabled/init, leading to broken layouting)

        if (_layout)
            _layout.childAlignment = Time.frameCount % 2 == 0 ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
    }

    public List<MainMenuPlayer> getPlayers() {
        return players;
    }

    public void AddPlayer(MainMenuPlayer player)
    {
        if (players.Contains(player))
            return;

        players.Add(player);

        player.transform.SetParent(playerListContentTransform, false);
        //addButtonRow.transform.SetAsLastSibling();

    }

    public void RemovePlayer(MainMenuPlayer player)
    {
        players.Remove(player);
    }

    public List<MainMenuPlayer> getMainMenuPlayerList() {
        return players;
    }
}

