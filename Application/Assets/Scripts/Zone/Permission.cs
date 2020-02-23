using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Permission {

    public enum PermissionType
    {
       ALLOW = 0, WARNING, VOTE, DENY
    }

    public static string[] permissionTypeNames = new string[] { "Allow", "Warning", "Vote", "Deny" };

    public PermissionType OwnerPermissionType { get; set; }
    public PermissionType OthersPermissionType { get; set; }


    public Permission(PermissionType ownerPermissionType, PermissionType otherPermissionType)
    {
        this.OwnerPermissionType = ownerPermissionType;
        this.OthersPermissionType = otherPermissionType;
    }

    public void Check(Player player, NetworkInstanceId ownerNetId, UnityAction yesEvent, UnityAction noEvent, string warningString, string voteString)
    {
        if (yesEvent == null) yesEvent = () => { };
        if (noEvent == null) noEvent = () => { };

        PermissionType permissionType;
        if (ownerNetId == player.netId)
        {
            permissionType = OwnerPermissionType;
        } else
        {
            permissionType = OthersPermissionType;
        }

        if (permissionType == PermissionType.ALLOW)
        {
            yesEvent();
        } else if (permissionType == PermissionType.WARNING)
        {
            QuestionPanel.Show(warningString, yesEvent, noEvent);
        }
        else if (permissionType == PermissionType.VOTE)
        {
            VoteManager.Instance.CreateVote(voteString, yesEvent, noEvent);
        }
        else
        {
            noEvent();
        }

           
    }


}
