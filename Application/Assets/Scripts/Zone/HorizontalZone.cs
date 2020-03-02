using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HorizontalZone : Zone
{

    //GameObject placeholder;

    private int placeholderSiblingIndex;
    private bool placeholderIsActive;

    [SyncVar]
    public Permission.PermissionType ownerViewPermissionType;
    [SyncVar]
    public Permission.PermissionType othersViewPermissionType;

    public Permission ViewPermission { get; set; }

    private SidewayScrollable scrollableComponent;

    private Vector3 spriteSize;

    private float cardsInsideMoveSpeed = 0.15f;

    public string draggedCardsSortingLayer = "HandCards";

    protected override void Awake()
    {
        base.Awake();

        scrollableComponent = GetComponent<SidewayScrollable>();
    }

    protected override void Start()
    {
        base.Start();

        spriteSize = GetComponent<SpriteRenderer>().sprite.bounds.size;
        ViewPermission = new Permission(ownerViewPermissionType, othersViewPermissionType);


        //this.transform.localPosition = Vector3.zero
    }

    public override void OnDroppableHoverEnter(Droppable droppable)
    {
        base.OnDroppableHoverEnter(droppable);

        placeholderIsActive = true;

        droppable.GetComponent<SpriteRenderer>().sortingLayerName = cardsSortingLayer;
    }

    public override void OnDroppableHoverExit(Droppable droppable)
    {
        base.OnDroppableHoverExit(droppable);

        placeholderIsActive = false;

        UpdateCardPositions(false, cardsInsideMoveSpeed);

        droppable.GetComponent<SpriteRenderer>().sortingLayerName = draggedCardsSortingLayer;
    }

    public override void OnItemAboveBeginDrag(Droppable droppable)
    {
        base.OnItemAboveBeginDrag(droppable);

        placeholderIsActive = true;
        droppable.GetComponent<SpriteRenderer>().sortingLayerName = cardsSortingLayer;

    }

    public override void OnItemAboveDrag(Droppable droppable)
    {
        base.OnItemAboveDrag(droppable);

        int newIndex = cardsHolderTransform.childCount;
        for (int i = 0; i < cardsHolderTransform.childCount; i++)
        {
            if (droppable.transform.position.x < cardsHolderTransform.GetChild(i).position.x)
            {
                newIndex = i;

                /*if (placeholderSiblingIndex < newIndex)
                    newIndex--;*/

                break;
            }
        }


        placeholderSiblingIndex = newIndex;

        UpdateCardPositions(false, cardsInsideMoveSpeed);

        droppable.GetComponent<SpriteRenderer>().sortingOrder = placeholderSiblingIndex;
    }

    public override void OnItemAboveEndDrag(Droppable droppable)
    {
        base.OnItemAboveEndDrag(droppable);

    }

    public override void OnItemDropped(CardView card)
    {
        if (cardsHolderTransform.childCount < 3)
        {
            base.OnItemDropped(card);

            int siblignIndexToDrop = transform.childCount;

            if (placeholderIsActive)
            {
                siblignIndexToDrop = placeholderSiblingIndex;
            }

            placeholderIsActive = false;

            DropCard(card, siblignIndexToDrop);

            //card.transform.DOKill();
            /*card.transform.DOLocalMove(GetPositionForChild(card.transform.GetSiblingIndex()), cardDropSpeed).OnKill(() => {
                scrollableComponent.OnChildAdded(card.SpriteRenderer);
            });*/
            card.KillAnimation();
            card.MoveTo(GetPositionForChild(card.transform.GetSiblingIndex()), cardDropSpeed).OnComplete(() =>
            {
                scrollableComponent.OnChildAdded(card.SpriteRenderer);
            });
        }
        else
        {
            OnCardDropFailed(card);
        }
    }

    public override void DropCard(CardView card, int siblingIndex)
    {
        if (cardsHolderTransform.childCount < 3)
        {
            CommandProcessor.Instance.ExecuteClientCommand(new CommandDropCardTo(Player.LocalPlayer, this, card, siblingIndex));

            base.DropCard(card, siblingIndex);
        }

    }

    protected override void PlaceCard(CardView card, int siblingIndex)
    {
        if (cardsHolderTransform.childCount < 3)
        {
            base.PlaceCard(card, siblingIndex);

            card.SetFacingUp(defauldIsFacingUp);

            UpdateCardPositions(false, cardsInsideMoveSpeed);

            UpdateCardSortingLayer(card);
        }
    }

    public override void DropCardOnClients(CardView card, int siblingIndex)
    {
        base.DropCardOnClients(card, siblingIndex);


        //card.transform.DOKill();
        /*card.transform.DOLocalMove(GetPositionForChild(card.transform.GetSiblingIndex()), cardMoveSpeed).OnKill(() => {
            scrollableComponent.OnChildAdded(card.SpriteRenderer);
        });*/
        card.KillAnimation();
        card.MoveTo(GetPositionForChild(card.transform.GetSiblingIndex()), cardMoveSpeed).OnComplete(() =>
        {
            scrollableComponent.OnChildAdded(card.SpriteRenderer);
        });
    }

    public override void OnCardDropFailed(CardView card)
    {
        base.OnCardDropFailed(card);

        placeholderIsActive = false;
        UpdateCardPositions(false, cardsInsideMoveSpeed);
    }

    private void UpdateCardPositions(bool killPreviousAnimations, float duration)
    {
        for (int i = 0; i < cardsHolderTransform.childCount; i++)
        {
            int index = i;
            //transform.GetChild(i).transform.localPosition = -startingPosition + new Vector3(/*transform.GetChild(i).GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2*/ 1.5f + i * 1f, 0, 0);

            if (placeholderIsActive && i >= placeholderSiblingIndex) index++;
            var newPos = GetPositionForChild(index);
            var card = cardsHolderTransform.GetChild(i).GetComponent<CardView>();
            if (killPreviousAnimations)
                card.KillAnimation();
            card.MoveTo(newPos, duration);
            card.SpriteRenderer.sortingOrder = index;
        }
    }

    private Vector3 GetPositionForChild(int index)
    {
        float startingPositionX = spriteSize.x / 2;

        return new Vector3(-startingPositionX + 1f + index * 0.8f, 0, 0);
    }

    public override void OnCardRemoved(CardView card)
    {
        base.OnCardRemoved(card);
        UpdateCardPositions(false, cardsInsideMoveSpeed);

        scrollableComponent.OnChildRemoved(card.SpriteRenderer);
    }

    public override void OnCardTouched(CardView card)
    {
        string warningString = "Are you sure you want to flip this card on " + LogEntry.ZoneLogEntryPart(this) + "?";
        string voteString = "Allow " + LogEntry.PlayerLogEntryPart(Player.LocalPlayer) + " to flip a card on " + LogEntry.ZoneLogEntryPart(this) + "?";
        ViewPermission.Check(Player.LocalPlayer, ownerNetId,
            () => {
                card.FlipCard();
            }, null, warningString, voteString);


    }

}
