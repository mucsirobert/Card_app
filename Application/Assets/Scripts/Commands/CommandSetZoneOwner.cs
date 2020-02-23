using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSetZoneOwner : Command
{
    private Player caller;
    private Zone zone;
    private Player owner;

    public CommandSetZoneOwner(Zone zone, Player owner)
    {
        this.caller = Player.LocalPlayer;
        this.zone = zone;
        this.owner = owner;
    }

    public CommandSetZoneOwner(Player caller, Zone zone, Player owner)
    {
        this.caller = caller;
        this.zone = zone;
        this.owner = owner;
    }

    public override void SendToServer()
    {
        CommandProcessor.Instance.CmdSetZoneOwner(caller.gameObject, zone.gameObject, owner.gameObject);
    }

    public override void ExecuteOnClient(bool executeOnAllClients)
    {
    }

    public override void ExecuteOnServer(bool executeOnAllClients)
    {
        base.ExecuteOnServer(executeOnAllClients);
        zone.ownerNetId = owner.netId;
    }

    public override LogEntry GetLogEntry()
    {
        return new LogEntry(LogEntry.PlayerLogEntryPart(caller) + LogEntry.NormalLogEntryPart(" took ownership of ") + LogEntry.ZoneLogEntryPart(zone));
    }

    public override void UnExecuteServerCommand(bool executeOnAllClients)
    {
        base.UnExecuteServerCommand(executeOnAllClients);
    }
}
