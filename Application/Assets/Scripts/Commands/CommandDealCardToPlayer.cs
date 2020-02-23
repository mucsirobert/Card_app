using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CommandDealCardToPlayer : Command {

    private Player caller;
    private Player[] playersToDealTo;
    private CardView[] cardsToDeal;

    private Zone exHolder;
    private int[] exSiblingIndexes;
    private bool[] exIsFacingUps;
    private bool[] isFacingUps;


    public CommandDealCardToPlayer(Player caller, Player[] playersToDealTo, CardView[] cardsToDeal)
    {
        if (playersToDealTo.Length == 0)
        {
            Debug.LogError("No player's were given!");
            return;
        }
        if (cardsToDeal.Length == 0)
        {
            Debug.LogError("No cards were given!");
            return;
        }

        this.caller = caller;
        exHolder = cardsToDeal[0].CurrentZone;
        this.cardsToDeal = cardsToDeal;
        this.playersToDealTo = playersToDealTo;
        exSiblingIndexes = new int[cardsToDeal.Length];
        exIsFacingUps = new bool[cardsToDeal.Length];
        isFacingUps = new bool[cardsToDeal.Length];
        for (int i = 0; i < cardsToDeal.Length; i++)
        {
            exSiblingIndexes[i] = cardsToDeal[i].ExSiblingIndex;
            exIsFacingUps[i] = cardsToDeal[i].ExIsFacingUp;
            isFacingUps[i] = cardsToDeal[i].isFaceingUp;
        }
    }

    public CommandDealCardToPlayer(CommandDealCardToPlayerMessage message)
    {
        this.caller = message.caller.GetComponent<Player>();

        this.playersToDealTo = new Player[message.playersToDealTo.Length];
        for (int i = 0; i < message.playersToDealTo.Length; i++)
        { 
            this.playersToDealTo[i] = message.playersToDealTo[i].GetComponent<Player>();
        }
        this.cardsToDeal = new CardView[message.cardsToDeal.Length];
        exSiblingIndexes = new int[cardsToDeal.Length];
        for (int i = 0; i < this.cardsToDeal.Length; i++)
        {
            this.cardsToDeal[i] = message.cardsToDeal[i].GetComponent<CardView>();
            exSiblingIndexes[i] = cardsToDeal[i].ExSiblingIndex;
        }
        exIsFacingUps = message.exIsFacingUps;
        isFacingUps = message.isFaceingUps;
        exHolder = cardsToDeal[0].CurrentZone;
        this.Multi = message.multi;
    }


    public override void SendToServer()
    {
        CommandProcessor.Instance.CmdDealCardToPlayer(getCommandMessage());
    }


    public override void ExecuteOnServer(bool executeOnAllClients)
    {
        base.ExecuteOnServer(executeOnAllClients);
        CommandProcessor.Instance.RpcDealCardToPlayer(getCommandMessage(), executeOnAllClients);
    }

    public CommandDealCardToPlayerMessage getCommandMessage()
    {
        return new CommandDealCardToPlayerMessage(caller, playersToDealTo, cardsToDeal, Multi, isFacingUps, exIsFacingUps);
    }

    public override void UnExecuteServerCommand(bool executeOnAllClients)
    {
        base.UnExecuteServerCommand(executeOnAllClients);

        for (int cardIndex = cardsToDeal.Length - 1; cardIndex >= 0; cardIndex--)
        {
            CommandProcessor.Instance.RpcUnexecuteDropCardTo(new CommandDropCardToMessage(caller.netId, exHolder.netId, cardsToDeal[cardIndex].netId, exSiblingIndexes[cardIndex], Multi, isFacingUps[cardIndex], exIsFacingUps[cardIndex]));
        }
        
    }

    public override void ExecuteOnClient(bool executeOnAllClients)
    {
        if (!executeOnAllClients)
        {
            if (caller.isLocalPlayer) return;
        }

        int playerIndex = 0;
        
        for (int cardIndex = 0; cardIndex < cardsToDeal.Length; cardIndex++)
        {
            playerIndex = cardIndex % playersToDealTo.Length;
            playersToDealTo[playerIndex].DealCardToPlayer(cardsToDeal[cardIndex]);

        }
        
    }

    public override LogEntry GetLogEntry()
    {

        if (playersToDealTo.Length == Player.Players.Count)
        {
            return new LogEntry(LogEntry.PlayerLogEntryPart(caller) + LogEntry.NormalLogEntryPart(" dealt " + cardsToDeal.Length/ playersToDealTo.Length + " cards to everybody from ") + LogEntry.ZoneLogEntryPart(exHolder));
        } else {
            string logText = "";
            logText = logText + LogEntry.PlayerLogEntryPart(caller) + LogEntry.NormalLogEntryPart(" dealt " + cardsToDeal.Length/ playersToDealTo.Length + " cards to ");

            for (int i = 0; i < playersToDealTo.Length; i++)
            {
                logText = logText + LogEntry.PlayerLogEntryPart(playersToDealTo[i]);
                if (i != playersToDealTo.Length-1)
                {
                    logText = logText + LogEntry.NormalLogEntryPart(", ");
                }
            }
            logText = logText + LogEntry.NormalLogEntryPart(" from ") + LogEntry.ZoneLogEntryPart(exHolder);
            return new LogEntry(logText);
        }
    }
}


public class CommandDealCardToPlayerMessage : MessageBase, ICommandMessage
{
    public GameObject caller;
    public GameObject[] playersToDealTo;
    public GameObject[] cardsToDeal;
    public bool multi;
    internal bool[] exIsFacingUps;
    internal bool[] isFaceingUps;

    public CommandDealCardToPlayerMessage() { }

    public CommandDealCardToPlayerMessage(Player caller, Player[] playersToDealTo, CardView[] cardsToDeal, bool multi, bool[] isFaceingUps, bool[] exIsFacingUps)
    {
        this.caller = caller.gameObject;
        this.playersToDealTo = new GameObject[playersToDealTo.Length];
        for (int i = 0; i <playersToDealTo.Length; i++)
        {
            this.playersToDealTo[i] = playersToDealTo[i].gameObject;
        }
        this.cardsToDeal = new GameObject[cardsToDeal.Length];
        for (int i = 0; i < cardsToDeal.Length; i++)
        { 
            this.cardsToDeal[i] = cardsToDeal[i].gameObject;
        }
        this.multi = multi;
        this.exIsFacingUps = exIsFacingUps;
        this.isFaceingUps = isFaceingUps;
    }

    public Command GetCommand()
    {
        return new CommandDealCardToPlayer(this);
    }
}
