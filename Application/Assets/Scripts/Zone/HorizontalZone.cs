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

    protected int placeholderSiblingIndex;
    protected bool placeholderIsActive;

    // i want to make an index as a placeholder in da zone
    public Transform zoneIndexHolder;
    /*protected Sprite sprite;
    */
    [SyncVar]
    public Permission.PermissionType ownerViewPermissionType;
    [SyncVar]
    public Permission.PermissionType othersViewPermissionType;

    public Permission ViewPermission { get; set; }

    protected SidewayScrollable scrollableComponent;

    private Vector3 spriteSize;

    private float cardsInsideMoveSpeed = 0.15f;

    public string draggedCardsSortingLayer = "HandCards";
    private bool expandable = true;

    public GameObject ph;

    protected float cardSpace = 1.6f;
    public List<GameObject> phs;
    public GameObject PlaceHoldersTransform;

    protected override void Awake()
    {
        base.Awake();

        scrollableComponent = GetComponent<SidewayScrollable>();
    }

    protected override void Start()
    {
        base.Start();
        
        //sprite = GetComponent<SpriteRenderer>().sprite;
        spriteSize = GetComponent<SpriteRenderer>().sprite.bounds.size;
        //        spriteSize.x = 9.0f;
        //DropCard(zoneIndexHolder.GetComponent<CardView>(), 0);
        ViewPermission = new Permission(ownerViewPermissionType, othersViewPermissionType);

        // phs[0].transform.position = new Vector3(1.6f, 0, 0);
        //phs[0].transform.position = new Vector3(0, 0, 0);
        //phs[0].transform.position = GetPositionForChild(0);
        //scrollableComponent.scrollableCards = this.numberOfCards;
        
        var PlaceHolders = Instantiate(PlaceHoldersTransform);
        PlaceHolders.transform.SetParent(this.transform);
        for (int i = 0; i < numberOfCards; i++)
        {
            phs.Add(Instantiate(ph));
            phs[i].transform.SetParent(this.transform);
            phs[i].transform.localPosition = GetPositionForChild(i);
            phs[i].transform.SetParent(PlaceHolders.transform);
        }
        //this.transform.DOScaleX(1.15f, 0.0001f);
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

        if(collapse)
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
        // Debug.Log("OnItemAboveDrag: btw wtf is this???");
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
        if (collapse)
            UpdateCardPositions(false, cardsInsideMoveSpeed);
        droppable.GetComponent<SpriteRenderer>().sortingOrder = placeholderSiblingIndex;
    }

    public override void OnItemAboveEndDrag(Droppable droppable)
    {
        Debug.Log("DOES IT EVEN EXIST?!");
        for(int i = 0; i < numberOfCards; i++)
        {
            //probably for detecting, that 2 cards in the same slot
            Debug.Log("i: " + i);
           Debug.Log(cardsHolderTransform.GetChild(i).transform.position.x);
        }
        base.OnItemAboveEndDrag(droppable);

    }

    public override void OnItemDropped(CardView card)
    {
        if (cardsHolderTransform.childCount < numberOfCards)
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

            Debug.Log(collapse);
            card.KillAnimation();
            if (collapse)
            {
                card.MoveTo(GetPositionForChild(card.transform.GetSiblingIndex()), cardDropSpeed).OnComplete(() =>
                {
                    Debug.Log(GetPositionForChild(card.transform.GetSiblingIndex()).x);
                    // if (GetPositionForChild(card.transform.GetSiblingIndex()).x > 9.0f)
                    scrollableComponent.OnChildAdded(card.SpriteRenderer);
                });
            }
            else
            {
                // this and the placeholders should depend on a variable that could be eliminated by a default true/false in hand zone
                // furthermore when the cardspace is not 1.6 then this is useless that case would be the same for hand zone 
                card.MoveTo(GetPositionForChild(GetIndexForChild(card.transform.position.x - (cardSpace / 2) + scrollableComponent.scrollTimes * cardSpace)/*card.transform.GetSiblingIndex()*/), cardDropSpeed).OnComplete(() =>
                        {
                            Debug.Log(GetPositionForChild(card.transform.GetSiblingIndex()).x);
                // if (GetPositionForChild(card.transform.GetSiblingIndex()).x > 9.0f)
                scrollableComponent.OnChildAdded(card.SpriteRenderer);
                        });
            }
        }
        else
        {
            OnCardDropFailed(card);
        }
    }

    public override void DropCard(CardView card, int siblingIndex)
    {
        if (cardsHolderTransform.childCount < numberOfCards)
        {
            CommandProcessor.Instance.ExecuteClientCommand(new CommandDropCardTo(Player.LocalPlayer, this, card, siblingIndex));

            base.DropCard(card, siblingIndex);
        }

    }

    protected override void PlaceCard(CardView card, int siblingIndex)
    {
        if (cardsHolderTransform.childCount < numberOfCards)
        {
            base.PlaceCard(card, siblingIndex);

            card.SetFacingUp(defauldIsFacingUp);

            if (collapse)
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
        if (collapse)
            UpdateCardPositions(false, cardsInsideMoveSpeed);
    }

    protected void UpdateCardPositions(bool killPreviousAnimations, float duration)
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

    protected Vector3 GetPositionForChild(int index)
    {
        float startingPositionX = spriteSize.x / 2;

        /*Debug.Log("index: " + index);

        Debug.Log("fv: " + GetIndexForChild(-startingPositionX + 1f + index * cardSpace));*/
        return new Vector3(-startingPositionX + 1f + index * cardSpace /* how much space the card needs in the zone */, 0, 0);
    }


    protected int GetIndexForChild(float dropX)
    {
        float startingPositionX = spriteSize.x / 2;

        for(int i = 0; i < numberOfCards+1; i++)
        {
            if (-startingPositionX + 1f + i * cardSpace < dropX && -startingPositionX + 1f + (i+1) * cardSpace >= dropX)
                return i;
        }

        return -1;
    }

    public override void OnCardRemoved(CardView card)
    {
        base.OnCardRemoved(card);

        if (collapse)
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

    public override void OnMenuItemClicked(ContextMenuItem menuItem)
    {
        base.OnMenuItemClicked(menuItem);
        if (menuItem.id == 1)
        {
            if (cardSpace == 1.6f)
            {
                for (int i = 0; i < numberOfCards; i++)
                {
                    phs[i].transform.DOLocalMoveZ(-50f, 1);
                }
                cardSpace = 0.8f;
            }
            else
            {
                for (int i = 0; i < numberOfCards; i++)
                {
                    phs[i].transform.DOLocalMoveZ(0f, 1);
                }
                cardSpace = 1.6f;
            }
            UpdateCardPositions(false, 1.0f);
            scrollableComponent.UpdateCardSpace();
        }
        if(menuItem.id == 2)
        {
            Expand();
        }
    }


    private void Expand()
    {
        
        int cnt, i, j;
        cnt = i = 0;
        foreach (Transform fchild in cardsHolderTransform)
        {
            j = 0;
            foreach (Transform schild in cardsHolderTransform)
            {
                if (fchild.transform.position.x == schild.transform.position.x)
                {
                    if (expandable)
                    {
                        schild.DOMoveY(schild.transform.position.y - (0.75f * cnt), 1f);
                    }
                    else
                    {
                        schild.DOMoveY(schild.transform.position.y + (0.75f * cnt), 1f);
                    }
                    cnt++;
                }
                /*else
                {
                    Debug.Log("cX: " + cX + "GetPositionForChild(it).x: " + GetPositionForChild(it).x);
                }*/
                j++;
            }
            i++;
            cnt = 0;
            if (expandable)
            {
                fchild.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
                fchild.GetComponent<SpriteRenderer>().sortingLayerName = "HandCanvas";
            }
            else
            {
                fchild.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                fchild.GetComponent<SpriteRenderer>().sortingLayerName = "HorizontalZoneCards";
            }
        }
        expandable = !expandable;
    }

}
