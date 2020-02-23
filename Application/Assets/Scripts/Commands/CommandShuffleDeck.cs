using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CommandShuffleDeck : Command {

    private Player caller;
    private DeckView deck;

    private List<int> cardNums;
    private List<int> shuffledCardNums;




    public CommandShuffleDeck(Player caller, DeckView deck)
    {
        this.caller = caller;
        this.deck = deck;
    }

    public CommandShuffleDeck(CommandShuffleDeckMessage message)
    {
        this.caller = ClientScene.FindLocalObject(message.callerNetId).GetComponent<Player>();
        this.deck = ClientScene.FindLocalObject(message.deckNetId).GetComponent<DeckView>();
        this.Multi = message.multi;       
    }


    public override void SendToServer()
    {
        CommandProcessor.Instance.CmdShuffleDeck(getCommandMessage());
    }


    public override void ExecuteOnServer(bool executeOnAllClients)
    {
        base.ExecuteOnServer(executeOnAllClients);
        if (shuffledCardNums != null)
            deck.RpcSetCardOrders(shuffledCardNums.ToArray());
        else
        {
            cardNums = new List<int>();
            deck.ShuffleCards();
            var originalCardNums = new List<int>(deck.CardNums);
            for (int i = 0; i < originalCardNums.Count; i++)
            {
                int index = originalCardNums.FindIndex(item => item == i);
                cardNums.Add(index);
            }
        }

        CommandProcessor.Instance.RpcShuffleDeck(getCommandMessage(), executeOnAllClients);
    }

    public CommandShuffleDeckMessage getCommandMessage()
    {
        return new CommandShuffleDeckMessage(caller.netId, deck.netId, Multi);
    }

    public override void UnExecuteServerCommand(bool executeOnAllClients)
    {
        base.UnExecuteServerCommand(executeOnAllClients);
        shuffledCardNums = new List<int>(deck.CardNums);
        deck.RpcSetCardOrders(cardNums.ToArray());
    }

    public override void ExecuteOnClient(bool executeOnAllClients)
    {
        deck.PlayShuffleAnimation();
    }

    public override LogEntry GetLogEntry()
    {
        return new LogEntry(LogEntry.PlayerLogEntryPart(caller) + LogEntry.NormalLogEntryPart(" shuffled ") + LogEntry.ZoneLogEntryPart(deck));
    }
}


public class CommandShuffleDeckMessage : MessageBase, ICommandMessage
{
    public NetworkInstanceId callerNetId;
    public NetworkInstanceId deckNetId;
    public bool multi;


    public CommandShuffleDeckMessage() { }

    public CommandShuffleDeckMessage(NetworkInstanceId callerNetId, NetworkInstanceId deckNetId, bool multi)
    {
        this.callerNetId = callerNetId;
        this.deckNetId = deckNetId;
        this.multi = multi;
    }

    public Command GetCommand()
    {
        return new CommandShuffleDeck(this);
    }
}
