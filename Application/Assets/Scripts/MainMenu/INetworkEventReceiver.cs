using UnityEngine.Networking;

public interface INetworkEventReceiver {

    void OnPlaySceneLoaded();


    void OnStartHost(string ip);

    void OnStopHost();

    void OnConnectToServer(string ip);

    void OnDisconnectFromServer(NetworkError error);

    void OnClientError(int errorCode);

    void OnKickedFromServer();
    void OnLobbySceneLoaded();
}
