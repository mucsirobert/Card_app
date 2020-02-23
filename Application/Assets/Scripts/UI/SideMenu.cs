using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMenu : MonoBehaviour {

    [SerializeField]
    private RectTransform menu;

    private Vector3 menuOpenedPosition;

    [SerializeField]
    private Vector3 menuClosedPosition;

    [SerializeField]
    private Ease menuAnimationEase;

    private void Start()
    {
        menuOpenedPosition = menu.anchoredPosition;
        menu.anchoredPosition = menuClosedPosition;
    }

    public void OnMenuButtonClicked(bool pressed)
    {
        menu.DOKill();
        if (!pressed)
        {
            menu.DOAnchorPosX(menuOpenedPosition.x, 0.5f).SetEase(menuAnimationEase);
        }
        else
        {
            menu.DOAnchorPosX(menuClosedPosition.x, 0.5f).SetEase(menuAnimationEase);
        }

    }

}
