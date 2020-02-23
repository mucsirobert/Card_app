using UnityEngine;

public interface IClientCommand {
    void SendToServer();

    void ExecuteOnClient(bool executeOnAllClients);
	
}
