using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CommandProcessor : NetworkBehaviour {

    Stack<CommandStackEntry> undoStack;
    Stack<CommandStackEntry> redoStack;

    public static CommandProcessor Instance { get; set; }

    public Button undoButton;
    public Button redoButton;


    private void Start()
    {

       
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Instance = this;
        
        undoButton = GameObject.FindGameObjectWithTag("UndoButton1").GetComponent<Button>();
        
        redoButton = GameObject.FindGameObjectWithTag("RedoButton").GetComponent<Button>();
  
        
    }

    private void OnDestroy()
    {
        if (!isLocalPlayer) return;

        Instance = null;
    }

   

    private CommandProcessor()
    {
        undoStack = new Stack<CommandStackEntry>();
        redoStack = new Stack<CommandStackEntry>();
    }

    [Server]
    public void UndoLastServerCommand()
    {

        if (undoStack.Count == 0)
            return;

        
        CommandStackEntry lastEntry = undoStack.Pop();
        LogManager.Instance.MakeEntryActive(lastEntry.logEntry, false);
        Command lastCommand = lastEntry.command;
        redoStack.Push(lastEntry);
        lastCommand.UnExecuteServerCommand(true);

        var entry = lastCommand.GetLogEntry();

        entry.TypeText = "Undo";
        LogManager.Instance.AddEntry(entry);

        RpcSetRedoButtonInteractable(true);
        if (undoStack.Count == 0) RpcSetUndoButtonInteractable(false);

        LogManager.Instance.RpcShowLogPopup(entry.Text);
        
    }

    [Server]
    public void ResetServerCommand()
    {

        if (undoStack.Count == 0)
            return;

        while (undoStack.Count != 0)
        {
            UndoLastServerCommand();
        }

    }

    [Command]
    public void CmdUndoLastServerCommand()
    {
        Instance.UndoLastServerCommand();
    }


    [Command]
    public void CmdRedoLastServerCommand()
    {
        Instance.RedoLastServerCommand();
    }

    [Command]
    public void CmdResetServerCommand()
    {
        Instance.ResetServerCommand();
    }

    private void RedoLastServerCommand()
    {
        if (redoStack.Count == 0)
            return;

        CommandStackEntry lastEntry = redoStack.Pop();
        LogManager.Instance.MakeEntryActive(lastEntry.logEntry, true);
        Command lastCommand = lastEntry.command;
        undoStack.Push(lastEntry);
        lastCommand.ExecuteOnServer(true);

        var entry = lastCommand.GetLogEntry();
        entry.TypeText = "Redo";
        LogManager.Instance.AddEntry(entry);

        RpcSetUndoButtonInteractable(true);
        if (redoStack.Count == 0) RpcSetRedoButtonInteractable(false);

        LogManager.Instance.RpcShowLogPopup(entry.Text);

    }

    [Server]
    public void ExecuteServerCommand(Command cmd, bool executeOnAllClients)
    {
        cmd.ExecuteOnServer(executeOnAllClients);
        var entry = cmd.GetLogEntry();
        LogManager.Instance.AddEntry(entry);
        LogManager.Instance.MakeEntryActive(entry, true);
        LogManager.Instance.RpcShowLogPopup(entry.Text);
        
        undoStack.Push(new CommandStackEntry(cmd, entry));
        redoStack.Clear();

        RpcSetUndoButtonInteractable(true);
        RpcSetRedoButtonInteractable(false);
    }

    [Server]
    public void ExecuteServerCommandWithoutUndo(Command cmd, bool executeOnAllClients)
    {
        cmd.ExecuteOnServer(executeOnAllClients);
        var entry = cmd.GetLogEntry();

        LogManager.Instance.AddEntry(entry);
        
        
    }

    [ClientRpc]
    private void RpcSetUndoButtonInteractable(bool interactabe)
    {
        Instance.undoButton.interactable = interactabe;
    }
    [ClientRpc]
    private void RpcSetRedoButtonInteractable(bool interactabe)
    {
        Instance.redoButton.interactable = interactabe;
    }

    public void ExecuteClientCommand(IClientCommand cmd)
    {
        cmd.SendToServer();
    }

    [Command]
    public void CmdDropCardTo(CommandDropCardToMessage commandMessage)
    {
        Instance.ExecuteServerCommand(commandMessage.GetCommand(), false);
    }

    [ClientRpc]
    public void RpcDropCardTo(CommandDropCardToMessage commandMessage, bool executeOnAllClients)
    {
        //if (isLocalPlayer) return;

        commandMessage.GetCommand().ExecuteOnClient(executeOnAllClients);
    }

    [ClientRpc]
    public void RpcUnexecuteDropCardTo(CommandDropCardToMessage commandMessage)
    {
        //if (isLocalPlayer) return;

        commandMessage.GetCommand().UnExecute();
    }

    [Command]
    public void CmdSetFacingUpOnClients(CommandSetFacingUpMessage commandMessage)
    {
        Instance.ExecuteServerCommand(commandMessage.GetCommand(), true);
    }

    [ClientRpc]
    public void RpcSetFacingUpOnClients(CommandSetFacingUpMessage commandMessage, bool executeOnAllClients)
    {
        /*if (isLocalPlayer)
            return;*/
        commandMessage.GetCommand().ExecuteOnClient(executeOnAllClients);
    }

    [Command]
    public void CmdShowHand(CommandShowHandMessage commandMessage)
    {
        Instance.ExecuteServerCommand(commandMessage.GetCommand(), true);
    }

    [ClientRpc]
    public void RpcShowHand(CommandShowHandMessage commandMessage, bool executeOnAllClients)
    {
        commandMessage.GetCommand().ExecuteOnClient(executeOnAllClients);
    }

    [Command]
    public void CmdDealCardToPlayer(CommandDealCardToPlayerMessage commandMessage)
    {
        //RpcDealCardToPlayer(playerId, draggableEntityId);
        Instance.ExecuteServerCommand(commandMessage.GetCommand(), true );
    }

    [ClientRpc]
    public void RpcDealCardToPlayer(CommandDealCardToPlayerMessage commandMessage, bool executeOnAllClients)
    {
        //ClientScene.FindLocalObject(playerId).GetComponent<Player>().DealCardToPlayer(ClientScene.FindLocalObject(draggableEntityId).GetComponent<DraggableEntity>());
        commandMessage.GetCommand().ExecuteOnClient(executeOnAllClients);
    }

    [Command]
    public void CmdShuffleDeck(CommandShuffleDeckMessage commandMessage)
    {
        //NetworkServer.FindLocalObject(deckId).GetComponent<DeckView>().ShuffleCards();
        Instance.ExecuteServerCommand(commandMessage.GetCommand(), true);
    }

    [ClientRpc]
    public void RpcShuffleDeck(CommandShuffleDeckMessage commandMessage, bool executeOnAllClients)
    {
        commandMessage.GetCommand().ExecuteOnClient(executeOnAllClients);
    }

    [Command]
    public void CmdCreateVote(GameObject caller, string v)
    {
        Instance.ExecuteServerCommandWithoutUndo(new CommandCreateVote(caller.GetComponent<Player>(), v), true);
        //VoteManager.Instance.CreateVoteOnServer(ClientScene.FindLocalObject(callerId).GetComponent<Player>(), v);
    }

    

    [Command]
    public void CmdPlayerVoteFor(GameObject caller, Vote.Options voteData)
    {
        Instance.ExecuteServerCommandWithoutUndo(new CommandPlayerVoteFor(caller.GetComponent<Player>(), voteData), true);

    }

    [Command]
    public void CmdSetZoneOwner(GameObject caller, GameObject zone, GameObject owner)
    {
        Instance.ExecuteServerCommandWithoutUndo(new CommandSetZoneOwner(caller.GetComponent<Player>(), zone.GetComponent<Zone>(), owner.GetComponent<Player>()), true);
    }

}

public class CommandStackEntry
{
    public Command command;
    public LogEntry logEntry;

    public CommandStackEntry(Command command, LogEntry logEntry)
    {
        this.command = command;
        this.logEntry = logEntry;
    }
}