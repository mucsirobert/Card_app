using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CommandShowHand : Command
{

    private Player caller;
    private HandZone hand;
    private bool cardsAreFacingUp;
    private bool exCardsAreFacingUp;

    private CardView[] cards;
    private bool[] isFacingUps;
    private bool[] exIsFacingUps;




    public CommandShowHand(Player caller, HandZone hand, bool cardsAreFacingUp)
    {
        this.caller = caller;
        this.hand = hand;
        this.cardsAreFacingUp = cardsAreFacingUp;
        exCardsAreFacingUp = hand.CardsAreVisibleToOthers;

        cards = hand.GetCardsInhand().ToArray();
        isFacingUps = new bool[cards.Length];
        exIsFacingUps = new bool[cards.Length];

        for (int i = 0; i < cards.Length; i++)
        {
            isFacingUps[i] = cardsAreFacingUp;
            exIsFacingUps[i] = cards[i].ExIsFacingUp;
        }
    }

    public CommandShowHand(CommandShowHandMessage message)
    {
        this.caller = message.caller.GetComponent<Player>();
        this.hand = message.hand.GetComponent<HandZone>();
        this.cardsAreFacingUp = message.cardsAreFacingUp;
        this.exCardsAreFacingUp = message.exCardsAreFacingUp;
        this.exIsFacingUps = message.exIsFacingUps;
        this.isFacingUps = message.isFacingUps;


        cards = new CardView[message.cards.Length];
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i] = message.cards[i].GetComponent<CardView>();
        }
    }


    public override void SendToServer()
    {
        CommandProcessor.Instance.CmdShowHand(GetCommandMessage());
    }


    public override void ExecuteOnServer(bool executeOnAllClients)
    {
        base.ExecuteOnServer(executeOnAllClients);
        CommandProcessor.Instance.RpcShowHand(GetCommandMessage(), executeOnAllClients);
    }

    public CommandShowHandMessage GetCommandMessage()
    {
        var cardObjects = new GameObject[cards.Length];

        for (int i = 0; i < cards.Length; i++)
        {
            cardObjects[i] = cards[i].gameObject;
        }

        var message = new CommandShowHandMessage {
            caller = caller.gameObject,
            hand = hand.gameObject,
            cards = cardObjects,
            isFacingUps = isFacingUps,
            exIsFacingUps = exIsFacingUps,
            cardsAreFacingUp = cardsAreFacingUp,
            exCardsAreFacingUp = exCardsAreFacingUp
        };
        return message;
    }

    public override void UnExecuteServerCommand(bool executeOnAllClients)
    {
        base.UnExecuteServerCommand(executeOnAllClients);

        var cardObjects = new GameObject[cards.Length];

        for (int i = 0; i < cards.Length; i++)
        {
            cardObjects[i] = cards[i].gameObject;
        }

        var message = new CommandShowHandMessage
        {
            caller = caller.gameObject,
            hand = hand.gameObject,
            cards = cardObjects,
            isFacingUps = exIsFacingUps,
            //exIsFacingUps = exIsFacingUps,
            cardsAreFacingUp = exCardsAreFacingUp,
            //exCardsAreFacingUp = exCardsAreFacingUp
        };

        CommandProcessor.Instance.RpcShowHand(message, executeOnAllClients);
    }

    public override void ExecuteOnClient(bool executeOnAllClients)
    {
        if (!executeOnAllClients)
        {
            if (caller.isLocalPlayer) return;
        }

        hand.CardsAreVisibleToOthers = cardsAreFacingUp;

        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].SetFacingUp(isFacingUps[i]);
        }

    }

    public override LogEntry GetLogEntry()
    {
        if (cardsAreFacingUp)
        {
            return new LogEntry(LogEntry.PlayerLogEntryPart(caller) + LogEntry.NormalLogEntryPart(" showed his cards"));
        }
        else
        {
            return new LogEntry(LogEntry.PlayerLogEntryPart(caller) + LogEntry.NormalLogEntryPart(" turned his cards face down"));
        }
    }
}


public class CommandShowHandMessage : MessageBase, ICommandMessage
{
    public GameObject caller;
    public GameObject hand;
    public GameObject[] cards;
    public bool[] isFacingUps;
    public bool[] exIsFacingUps;

    public bool cardsAreFacingUp;
    public bool exCardsAreFacingUp;
    public bool multi;


    public CommandShowHandMessage() { }

   /* public CommandShowHandMessage(GameObject caller, bool cardsAreFacingUp, GameObject[] cards, bool isFacingUp, bool multi)
    {
        this.callerNetId = callerNetId;
        this.cardNetId = cardNetId;
        this.isFacingUp = isFacingUp;
        this.multi = multi;
    }*/

    public Command GetCommand()
    {
        return new CommandShowHand(this);
    }
}
