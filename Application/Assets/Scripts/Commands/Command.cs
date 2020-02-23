using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command : IServerCommand, IClientCommand, ILoggableCommand
{
    private static int id = 0;
    public int Id { get; set; }
    public bool Multi { get; set; }
    public bool Done { get; private set; }

    public abstract void SendToServer();
    public abstract void ExecuteOnClient(bool executeOnAllClients);
    public virtual void ExecuteOnServer(bool executeOnAllClients)
    {
        Done = true;
    }
    public abstract LogEntry GetLogEntry();
    public virtual void UnExecuteServerCommand(bool executeOnAllClients)
    {
        Done = false;
    }

    public virtual void UnExecute()
    {

    }
}

