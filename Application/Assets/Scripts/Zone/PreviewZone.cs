using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PreviewZone : MonoBehaviour
{
    public static PreviewZone Instance { get; private set; }

    public PreviewZoneCard previewZoneCardPrefab;

    public GameObject mainObject;

    public Transform cardHolderTransform;

    private SidewayScrollable scrollableComponent;
    private DropZone dropZoneComponent;

    private int placeholderSiblingIndex;
    private bool placeholderIsActive;

    private Zone zoneToDisplay;


    void Awake()
    {
        if (Instance == null) Instance = this;
        mainObject.SetActive(false);

        scrollableComponent = GetComponent<SidewayScrollable>();
        dropZoneComponent = GetComponent<DropZone>();

    }


    public void Init(List<CardView> cardViewsToAdd, Zone zoneToDisplay)
    {
        this.zoneToDisplay = zoneToDisplay;
        mainObject.SetActive(true);
        Vector3 startingPosition = new Vector3(GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2, 0, 0);

        for (int i = 0; i < cardViewsToAdd.Count; i++)
        {
            PreviewZoneCard previewZoneCardToSpawn = Instantiate(previewZoneCardPrefab, cardHolderTransform);
            /*var newPos = -startingPosition + new Vector3(1.5f + (i) * 2f, 0, 0);*/
            previewZoneCardToSpawn.transform.position = cardViewsToAdd[i].transform.position;
            previewZoneCardToSpawn.SetCardView(cardViewsToAdd[i]);
            previewZoneCardToSpawn.DroppableComponent.DroppedOn(dropZoneComponent);
        }
        UpdateCardPositions(OnCardsReady);

    }

    //This is called when the cards are in the correct position, after a delay due to the animation
    private void OnCardsReady()
    {
        foreach (Transform previewCard in cardHolderTransform)
        {
            scrollableComponent.OnChildAdded(previewCard.GetComponent<SpriteRenderer>());
        }
        scrollableComponent.InitButtons();
    }

    public void Close()
    {
        for (int i = 0; i < cardHolderTransform.childCount; i++)
        {
            Destroy(cardHolderTransform.GetChild(i).gameObject);
        }

        placeholderIsActive = false;

        mainObject.SetActive(false);
    }



    public void OnDroppableHoverExit(Droppable droppable)
    {
        droppable.GetComponent<ReassignableDroppable>().CurrentDraggedItem = droppable.GetComponent<PreviewZoneCard>().cardView.gameObject;

        Close();
    }
    public void OnItemAboveBeforeDrag(Droppable droppable)
    {
        string warningString = "Are you sure you want to take away this card from " + LogEntry.ZoneLogEntryPart(zoneToDisplay) + "?";
        string voteString = "Allow " + LogEntry.PlayerLogEntryPart(Player.LocalPlayer) + " to take away a card from " + LogEntry.ZoneLogEntryPart(zoneToDisplay) + "?";
        zoneToDisplay.TakeAwayPermission.Check(Player.LocalPlayer, zoneToDisplay.ownerNetId,
            () => { droppable.IsDraggable = true; },
            () => { droppable.IsDraggable = false; }, warningString, voteString);
    }



    public void OnItemAboveBeginDrag(Droppable droppable)
    {
        scrollableComponent.OnChildRemoved(droppable.GetComponent<SpriteRenderer>());
        placeholderIsActive = true;
    }

    public void OnItemAboveDrag(Droppable droppable)
    {
        int newIndex = cardHolderTransform.childCount;
        for (int i = 0; i < cardHolderTransform.childCount; i++)
        {
            if (droppable.transform.position.x < cardHolderTransform.GetChild(i).position.x)
            {
                newIndex = i;

                /*if (placeholder.transform.GetSiblingIndex() < newIndex)
                    newIndex--;*/

                break;
            }
        }


        placeholderSiblingIndex = newIndex;

        UpdateCardPositions();

        droppable.GetComponent<SpriteRenderer>().sortingOrder = placeholderSiblingIndex;
    }

    public void OnItemAboveEndDrag(Droppable droppable)
    {
    }

    public void OnItemDropped(Droppable droppable)
    {
        droppable.transform.parent = cardHolderTransform;

        if (placeholderIsActive)
        {
            //draggedItem.GetComponent<CardView>().siblingIndex = placeholder.transform.GetSiblingIndex();
            droppable.transform.SetSiblingIndex(placeholderSiblingIndex);
        }

        scrollableComponent.OnChildAdded(droppable.GetComponent<SpriteRenderer>());

        placeholderIsActive = false;

        //droppable.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        UpdateCardPositions();

        zoneToDisplay.DropCard(droppable.GetComponent<PreviewZoneCard>().cardView, placeholderSiblingIndex);

    }

    private void UpdateCardPositions(TweenCallback onComplete = null)
    {
        Vector3 startingPosition = new Vector3(GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2, 0, 0);

        Tweener lastAnimation = null;

        //First child is the glow
        for (int i = 0; i < cardHolderTransform.childCount; i++)
        {
            int index = i;
            //transform.GetChild(i).transform.localPosition = -startingPosition + new Vector3(/*transform.GetChild(i).GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2*/ 1.5f + i * 1f, 0, 0);

            if (placeholderIsActive && i >= placeholderSiblingIndex) index++;
            var newPos = -startingPosition + new Vector3(1f + index * 0.8f, 0, 0);
            lastAnimation = cardHolderTransform.GetChild(i).DOLocalMove(newPos, 0.1f);
            cardHolderTransform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = index;
        }
        if (onComplete != null && lastAnimation != null)
        {
            lastAnimation.onComplete = onComplete;
        }
    }
}
