using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CommandSetFacingUp : Command
{
    private Player caller;
    private CardView card;
    private Zone zone;
    private bool isFacingUp;

    private bool exIsFacingUp;




    public CommandSetFacingUp(Player caller, CardView card, bool isFacingUp)
    {
        exIsFacingUp = card.ExIsFacingUp;
        this.caller = caller;
        this.card = card;
        zone = card.CurrentZone;
        this.isFacingUp = isFacingUp;
    }

    public CommandSetFacingUp(CommandSetFacingUpMessage message)
    {
        this.caller = message.caller.GetComponent<Player>();
        this.card = message.card.GetComponent<CardView>();
        this.zone = message.zone.GetComponent<Zone>();
        this.isFacingUp = message.isFacingUp;
        this.Multi = message.multi;
        exIsFacingUp = card.ExIsFacingUp;
    }


    public override void SendToServer()
    {
        CommandProcessor.Instance.CmdSetFacingUpOnClients(getCommandMessage());
    }


    public override void ExecuteOnServer(bool executeOnAllClients)
    {
        base.ExecuteOnServer(executeOnAllClients);
        CommandProcessor.Instance.RpcSetFacingUpOnClients(getCommandMessage(), executeOnAllClients);
    }

    public CommandSetFacingUpMessage getCommandMessage()
    {
        return new CommandSetFacingUpMessage(caller.gameObject, card.gameObject, zone.gameObject, isFacingUp, Multi);
    }

    public override void UnExecuteServerCommand(bool executeOnAllClients)
    {
        base.UnExecuteServerCommand(executeOnAllClients);
        CommandProcessor.Instance.RpcSetFacingUpOnClients(new CommandSetFacingUpMessage(caller.gameObject, card.gameObject, zone.gameObject, exIsFacingUp, Multi), executeOnAllClients);
    }

    public override void ExecuteOnClient(bool executeOnAllClients)
    {
        if (!executeOnAllClients)
        {
            if (caller.isLocalPlayer) return;
        }

        card.SetFacingUp(isFacingUp);
    }

    public override LogEntry GetLogEntry()
    {
        return  new LogEntry(LogEntry.PlayerLogEntryPart(caller) + LogEntry.NormalLogEntryPart(" flipped a card in " + LogEntry.ZoneLogEntryPart(zone)));
    }
}


public class CommandSetFacingUpMessage : MessageBase, ICommandMessage
{
    public GameObject caller;
    public GameObject card;
    public GameObject zone;
    public bool isFacingUp;
    public bool multi;


    public CommandSetFacingUpMessage() { }

    public CommandSetFacingUpMessage(GameObject caller, GameObject card, GameObject zone, bool isFacingUp, bool multi)
    {
        this.caller = caller;
        this.card = card;
        this.zone = zone;
        this.isFacingUp = isFacingUp;
        this.multi = multi;
    }

    public Command GetCommand()
    {
        return new CommandSetFacingUp(this);
    }
}
