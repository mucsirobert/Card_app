using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiCommand : Command
{
    private Command command1;
    private Command command2;


    public MultiCommand(Command command1, Command command2)
    {
        this.command1 = command1;
        this.command2 = command2;

        this.command2.Multi = true;
    }

    public override void SendToServer()
    {
        command1.SendToServer();
        command2.SendToServer();
    }


    public override void ExecuteOnServer(bool executeOnAllClients)
    {
        base.ExecuteOnServer(executeOnAllClients);
        command1.ExecuteOnServer(executeOnAllClients);
        command2.ExecuteOnServer(executeOnAllClients);
    }


    public override void UnExecuteServerCommand(bool executeOnAllClients)
    {
        base.UnExecuteServerCommand(executeOnAllClients);
        command2.UnExecuteServerCommand(executeOnAllClients);
        command1.UnExecuteServerCommand(executeOnAllClients);
    }

    public override void ExecuteOnClient(bool executeOnAllClients)
    {
        command1.ExecuteOnClient(executeOnAllClients);
        command2.ExecuteOnClient(executeOnAllClients);
    }

    public override LogEntry GetLogEntry()
    {
        /*LogEntry[] command1LogEntry = command1.GetLogEntry();
        LogEntry[] command2LogEntry = command2.GetLogEntry();
        var ret = new LogEntry[command1LogEntry.Length + command2LogEntry.Length];
        command1LogEntry.CopyTo(ret, 0);
        command2LogEntry.CopyTo(ret, command1LogEntry.Length);
        return ret;*/
        return null;
    }
}

