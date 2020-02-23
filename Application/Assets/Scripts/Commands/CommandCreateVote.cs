using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCreateVote : Command  {
    private Player caller;
    private string text;

    public CommandCreateVote(string text)
    {
        this.text = text;
        this.caller = Player.LocalPlayer;
    }

    public CommandCreateVote(Player caller, string text)
    {
        this.text = text;
        this.caller = caller;
    }

    public override void SendToServer()
    {
        CommandProcessor.Instance.CmdCreateVote(caller.gameObject, text);
    }

    public override void ExecuteOnClient(bool executeOnAllClients)
    {
    }

    public override void ExecuteOnServer(bool executeOnAllClients)
    {
        base.ExecuteOnServer(executeOnAllClients);
        VoteManager.Instance.CreateVoteOnServer(caller, text);
    }

    public override LogEntry GetLogEntry()
    {
        return new LogEntry(LogEntry.PlayerLogEntryPart(caller) + LogEntry.NormalLogEntryPart(" created a vote : " + text));
    }

    public override void UnExecuteServerCommand(bool executeOnAllClients)
    {
        base.UnExecuteServerCommand(executeOnAllClients);
    }
}

