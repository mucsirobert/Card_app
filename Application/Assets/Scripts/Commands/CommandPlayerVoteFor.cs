using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPlayerVoteFor : Command {
    private Player caller;
    private Vote.Options option;

    public CommandPlayerVoteFor(Vote.Options option)
    {
        this.caller = Player.LocalPlayer;
        this.option = option;
    }

    public CommandPlayerVoteFor(Player caller, Vote.Options option)
    {
        this.caller = caller;
        this.option = option;
    }

    public override void SendToServer()
    {
        CommandProcessor.Instance.CmdPlayerVoteFor(caller.gameObject, option);
    }

    public override void ExecuteOnClient(bool executeOnAllClients)
    {
    }

    public override void ExecuteOnServer(bool executeOnAllClients)
    {
        base.ExecuteOnServer(executeOnAllClients);
        VoteManager.Instance.PlayerVoteFor(caller, option);
    }

    public override LogEntry GetLogEntry()
    {
        return new LogEntry(LogEntry.PlayerLogEntryPart(caller) + LogEntry.NormalLogEntryPart(" voted for \'" + option.ToString()+"\'"));
    }

    public override void UnExecuteServerCommand(bool executeOnAllClients)
    {
        base.UnExecuteServerCommand(executeOnAllClients);
    }
}
