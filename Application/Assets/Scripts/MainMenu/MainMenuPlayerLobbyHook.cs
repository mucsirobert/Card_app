using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MainMenuPlayerLobbyHook : LobbyHook {


    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        base.OnLobbyServerSceneLoadedForPlayer(manager, lobbyPlayer, gamePlayer);

        gamePlayer.GetComponent<Player>().playerName = lobbyPlayer.GetComponent<MainMenuPlayer>().playerName;
        gamePlayer.GetComponent<Player>().playerColor = lobbyPlayer.GetComponent<MainMenuPlayer>().playerColor;
    }
}
