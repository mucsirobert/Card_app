using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideHand : MonoBehaviour {
    public Vector3 HiddenPosition { get; set; }
    public Vector3 ShownPosition { get; set; }

    public bool IsHidden { get; set; }


    public void Hide()
    {
        IsHidden = true;
        transform.DOKill();
        transform.DOLocalMove(HiddenPosition, 0.3f);
    }

    public void Show()
    {
        IsHidden = false;
        transform.DOKill();
        transform.DOLocalMove(ShownPosition, 0.3f);
    }

    public void Toggle()
    {
        if (IsHidden) Show();
        else Hide();
    }
}


