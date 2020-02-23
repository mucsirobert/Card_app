using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogPopUp : AutoScrollLayout<string> {

    [SerializeField]
    private LogPopUpText[] items;

    protected override void Start()
    {
        InitItems(items);

        base.Start();
    }

    public void ShowLastLog(string message)
    {
        AddNewItem(message);
    }


	
}
