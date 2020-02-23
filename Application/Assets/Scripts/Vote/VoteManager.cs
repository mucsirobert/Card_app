using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class VoteManager : NetworkBehaviour {

    private Vote currentVote;
    private static VoteManager voteManager;
    private VoteAction voteAction;

    private VotePanel currentDialog;

    public static VoteManager Instance
    {
        get
        {
            if (!voteManager)
            {
                voteManager = FindObjectOfType(typeof(VoteManager)) as VoteManager;
                if (!voteManager)
                    Debug.LogError("There needs to be one active VoteManager script on a GameObject in your scene.");
            }

            return voteManager;
        }
    }

    public void CreateVote(string voteString, UnityAction yesAction, UnityAction noAction)
    {
        voteAction = new VoteAction(yesAction, noAction);

        //MyNetwork.Instance.CreateVote(voteString);
        CommandProcessor.Instance.ExecuteClientCommand(new CommandCreateVote(voteString));
    }

    [Server]
    public void CreateVoteOnServer(Player owner, string voteString)
    {
        currentVote = new Vote(owner);

        RpcCreateVoteWindow(voteString);
    }


    [ClientRpc]
    public void RpcCreateVoteWindow(string voteString)
    {
        currentDialog = VotePanel.Show(voteString,
            () => { /*MyNetwork.Instance.PlayerVoteFor(Vote.Options.YES);*/
                //ModalPanel.Instance().Info("Waiting for other players to vote...");
                CommandProcessor.Instance.ExecuteClientCommand(new CommandPlayerVoteFor(Vote.Options.Yes));
                //currentDialog.Close();
                currentDialog.LocalPlayerVoted();
            },
            () => { CommandProcessor.Instance.ExecuteClientCommand(new CommandPlayerVoteFor(Vote.Options.No));
                //ModalPanel.Instance().Info("Waiting for other players to vote...");
                //currentDialog.Close();
                currentDialog.LocalPlayerVoted();
            });
    }

    [Server]
    public void PlayerVoteFor(Player player, Vote.Options option)
    {
        currentVote.AddVote(player, option);

        RpcPlayerVoteFor(player.gameObject, (byte)option);

        if (currentVote.Ended)
        {
            var result = currentVote.GetResult();
            if (result == Vote.Options.Yes)
                RpcVotePassed();
            else if (result == Vote.Options.No)
                RpcVoteFailed();
            TargetOnVoteResult(currentVote.Owner.connectionToClient, (byte) currentVote.GetResult());
            currentVote = null;
        }

    }

    [ClientRpc]
    public void RpcPlayerVoteFor(GameObject player, byte option)
    {
        currentDialog.PlayerVoted(player.GetComponent<Player>(), (Vote.Options) option);
    }

    [TargetRpc]
    private void TargetOnVoteResult(NetworkConnection connection, byte v)
    {
        if (voteAction != null)
        {
            voteAction.ExecuteAction((Vote.Options)v);
            voteAction = null;
        }
    }

    [ClientRpc]
    private void RpcVotePassed()
    {
        currentDialog.VotePassed();

    }

    [ClientRpc]
    private void RpcVoteFailed()
    {
        currentDialog.VoteFailed();

    }


}
