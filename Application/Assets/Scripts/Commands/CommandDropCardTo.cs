using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CommandDropCardTo : Command
{
    private Player caller;
    private Zone holder;
    private CardView card;
    private int siblingIndex;

    private Zone exHolder;
    private int exSiblingIndex;
    private bool exIsFacingUp;
    private bool isFacingUp;


    public CommandDropCardTo(Player caller, Zone holder, CardView card, int siblingIndex)
    {
        this.caller = caller;
        this.holder = holder;
        this.card = card;
        this.siblingIndex = siblingIndex;
        exHolder = card.CurrentZone;
        exSiblingIndex = card.ExSiblingIndex;
        exIsFacingUp = card.ExIsFacingUp;
        isFacingUp = card.isFaceingUp;

    }

    public CommandDropCardTo(CommandDropCardToMessage message)
    {
        this.caller = ClientScene.FindLocalObject(message.callerNetId).GetComponent<Player>();
        this.holder = ClientScene.FindLocalObject(message.holderNetId).GetComponent<Zone>();
        this.card = ClientScene.FindLocalObject(message.cardNetId).GetComponent<CardView>();
        this.siblingIndex = message.siblingIndex;
        this.Multi = message.multi;
        exHolder = card.CurrentZone;
        exSiblingIndex = card.ExSiblingIndex;
        exIsFacingUp = message.exIsFacingUp;
        isFacingUp = message.isFacingUp;
    }

    public override void SendToServer()
    {
        CommandProcessor.Instance.CmdDropCardTo(GetCommandMessage());
    }


    public override void ExecuteOnServer(bool executeOnAllClients)
    {
        base.ExecuteOnServer(executeOnAllClients);
        CommandProcessor.Instance.RpcDropCardTo(GetCommandMessage(), executeOnAllClients);
    }

    public CommandDropCardToMessage GetCommandMessage()
    { 
        return new CommandDropCardToMessage(caller.netId, holder.netId, card.netId, siblingIndex, Multi, exIsFacingUp, isFacingUp);
    }

    public override void UnExecuteServerCommand(bool executeOnAllClients)
    {
        base.UnExecuteServerCommand(executeOnAllClients);
        CommandProcessor.Instance.RpcUnexecuteDropCardTo(new CommandDropCardToMessage(caller.netId, exHolder.netId, card.netId, exSiblingIndex, Multi, exIsFacingUp, isFacingUp));
    }

    public override void UnExecute()
    {
        base.UnExecute();

        holder.DropCardOnClients(card, siblingIndex);
        card.SetFacingUp(isFacingUp);
    }

    public override void ExecuteOnClient(bool executeOnAllClients)
    {
        if (!executeOnAllClients)
        {
            if (caller.isLocalPlayer) return;
        }

        holder.DropCardOnClients(card, siblingIndex);
    }

    public override LogEntry GetLogEntry()
    {
        if (holder == exHolder)
        {
            return new LogEntry(LogEntry.PlayerLogEntryPart(caller) + LogEntry.NormalLogEntryPart(" moved a card inside ") + LogEntry.ZoneLogEntryPart(exHolder));
        }
        else
        {
            return new LogEntry(LogEntry.PlayerLogEntryPart(caller) + LogEntry.NormalLogEntryPart(" moved a card from ") + LogEntry.ZoneLogEntryPart(exHolder) + LogEntry.NormalLogEntryPart(" to ") + LogEntry.ZoneLogEntryPart(holder));
        }
    }
}


public class CommandDropCardToMessage : MessageBase, ICommandMessage
{
    public NetworkInstanceId callerNetId;
    public NetworkInstanceId holderNetId;
    public NetworkInstanceId cardNetId;
    public int siblingIndex;
    public bool multi;
    public bool exIsFacingUp;
    internal bool isFacingUp;

    public CommandDropCardToMessage() { }

    public CommandDropCardToMessage(NetworkInstanceId callerNetId, NetworkInstanceId holderNetId, NetworkInstanceId cardNetId, int siblingIndex, bool multi, bool exIsFacingUp, bool isFacingUp)
    {
        this.callerNetId = callerNetId;
        this.holderNetId = holderNetId;
        this.cardNetId = cardNetId;
        this.siblingIndex = siblingIndex;
        this.multi = multi;
        this.exIsFacingUp = exIsFacingUp;
        this.isFacingUp = isFacingUp;
    }

    public Command GetCommand()
    {
        return new CommandDropCardTo(this);
    }
}
