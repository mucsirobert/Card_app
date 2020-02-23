using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LogPopUpText : AutoScrollLayoutItem<string> {
    [SerializeField]
    private Text textComponent;

    protected override void Awake()
    {
        base.Awake();

        textComponent.text = "";
    }

    public override void SetData(string data)
    {
        base.SetData(data);

        textComponent.text = data;
    }


}
