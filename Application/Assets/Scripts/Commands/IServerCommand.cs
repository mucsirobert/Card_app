public interface IServerCommand {
    bool Multi { get; set; }


    void ExecuteOnServer(bool executeOnAllClients);
    void UnExecuteServerCommand(bool executeOnAllClients);
}
