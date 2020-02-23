using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VoteAction  {
    private UnityAction yesAction;
    private UnityAction noAction;


    public VoteAction(UnityAction yesAction,UnityAction noAction)
    {
        this.yesAction = yesAction;
        this.noAction = noAction;
    }

    public void ExecuteAction(Vote.Options option)
    {
        if (option == Vote.Options.Yes && yesAction != null)
            yesAction();
        else  if (option == Vote.Options.No && noAction != null)
        {
            noAction();
        }
    }

}
