using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

[RequireComponent(typeof(DropZone))]
public abstract class Zone : Entity { 

    [SyncVar(hook = "OnOwnerChanged")]
    public NetworkInstanceId ownerNetId;

    [SyncVar]
    public Permission.PermissionType ownerTakeAwayPermissionType;
    [SyncVar]
    public Permission.PermissionType ownerDropOntoPermissionType;
    [SyncVar]
    public Permission.PermissionType othersTakeAwayPermissionType;
    [SyncVar]
    public Permission.PermissionType othersDropOntoPermissionType;
    [SyncVar]
    public string zoneName;
    [SyncVar]
    public int numberOfCards;
    [SyncVar]
    public bool collapse = true;
    [SyncVar]
    // public Color zoneColor = Color.white;
    public Color zoneColor = new Color (108, 245, 108);

    [SyncVar]
    public bool defauldIsFacingUp;


    public Transform cardsHolderTransform;

    public string cardsSortingLayer;


    public int NumberOfCards { get; set; }

    public string zoneID;


    public Permission TakeAwayPermission { get; set; }
    public Permission DropOntoPermission { get; set; }

    protected GameObject mainCamera;

    public DropZone DropZoneComponent  { get; private set; }
    private ContextMenu contextMenuComponent;

    protected float cardMoveSpeed = 0.5f;
    protected float cardDropSpeed = 0.2f;

    private bool canTakeAwayCards;
    public List<CardView> cardList = new List<CardView>();

    [SerializeField]
    private SpriteRenderer cornerSprite;

    protected override void Awake()
    {
        base.Awake();

        DropZoneComponent = GetComponent<DropZone>();
        contextMenuComponent = GetComponent<ContextMenu>();
    }

    protected override void Start()
    {
        base.Start();

        zoneID = transform.position.x.ToString() + transform.position.y.ToString() + transform.position.z.ToString(); 

        /*takeAwayPermission = new Permission(Permission.AllowType.WARNING, Permission.AccessType.OWNERONLY);
        dropOntoPermission = new Permission(Permission.AllowType.WARNING, Permission.AccessType.OWNERONLY);*/
        TakeAwayPermission = new Permission(ownerTakeAwayPermissionType, othersTakeAwayPermissionType);
        DropOntoPermission = new Permission(ownerDropOntoPermissionType, othersDropOntoPermissionType);

        mainCamera = Camera.main.gameObject;

        cornerSprite.color = zoneColor;
        //InitContextMenu();
    }

    public void OnDrop(Droppable droppable)
    {

        /*DropZone parentZone = eventData.pointerDrag.transform.parent.GetComponent<DropZone>();
        if (parentZone != null)                     
        {                                           
            parentZone.OnCardRemoved();             
        }*/
        CardView card = droppable.GetComponent<CardView>();

        string warningString = "Are you sure you want to drop this card to " + LogEntry.ZoneLogEntryPart(this) + "?";
        string voteString = "Allow " + LogEntry.PlayerLogEntryPart(Player.LocalPlayer) + " to drop a card to " + LogEntry.ZoneLogEntryPart(this) + "?";
        DropOntoPermission.Check(Player.LocalPlayer, ownerNetId, 
            () => { OnItemDropped(card); },
            () => { OnCardDropFailed(card); }, warningString, voteString);                                                                 
    }        
    
    public virtual void OnItemDropped(CardView card)
    {        
    }

    public virtual void DropCard(CardView card, int siblingIndex)
    {
        PlaceCard(card, siblingIndex);
    }

    protected virtual void PlaceCard(CardView card, int siblingIndex)
    {
        Zone prevZone = card.CurrentZone;

        if (siblingIndex == -1)
        {
            siblingIndex = cardsHolderTransform.childCount;
        }

        canTakeAwayCards = false;
        card.transform.SetParent(cardsHolderTransform);
        card.transform.SetSiblingIndex(siblingIndex);
        //card.lastPosition = Vector3.zero;

        card.SetLocallyFacingUp(false);

        UpdateCardsSortingOrder();
        UpdateCardsExSiblingIndex();

        if (!cardList.Contains(card))
        {
            cardList.Add(card);
            card.currentZoneObject = gameObject;
        }
        card.DroppedOnZone(this);

        if (prevZone != null && prevZone != (Zone)this)
        {

            prevZone.OnCardRemoved(card);
        }
        UnityEngine.Debug.Log(gameObject.name);
    }

    public void DropCardOnClientsOnTop(CardView card)
    {
        DropCardOnClients(card, cardsHolderTransform.childCount);
    }
    public virtual void DropCardOnClients(CardView card, int siblingIndex = -1)
    {
        PlaceCard(card, siblingIndex);
    }

    protected void UpdateCardSortingLayer(CardView card)
    {
        card.SpriteRenderer.sortingLayerName = cardsSortingLayer;

    }

    public virtual void OnCardDropFailed(CardView card)
    {
        card.ReturnToLastZone();
    }
                                                    
                                                                                            
    public virtual void OnDroppableHoverEnter(Droppable droppable)
    {
    }

    public virtual void OnDroppableHoverExit(Droppable droppable)
    {

    }

    public virtual void OnCardRemoved(CardView card)
    {
        canTakeAwayCards = false;
        UpdateCardsExSiblingIndex();
        cardList.Remove(card);
        UnityEngine.Debug.Log("elveve");
    }

    public virtual void tempMethod(CardView card)
    {
        PlaceCard(card, card.SiblingIndex);
    }

    public virtual void OnItemAboveBeforeDrag(Droppable droppable)
    {
        if (!canTakeAwayCards)
        {
            string warningString = "Are you sure you want to take away this card from " + LogEntry.ZoneLogEntryPart(this) + "?";
            string voteString = "Allow " + LogEntry.PlayerLogEntryPart(Player.LocalPlayer) + " to take away a card from " + LogEntry.ZoneLogEntryPart(this) + "?";
            TakeAwayPermission.Check(Player.LocalPlayer, ownerNetId,
                () => { canTakeAwayCards = CanTakeAwayCards(); },
                () => { canTakeAwayCards = false; }, warningString, voteString);
        }
        droppable.IsDraggable = canTakeAwayCards;
    }

    protected virtual bool CanTakeAwayCards()
    {
        return true;
    }


    public virtual void OnItemAboveBeginDrag(Droppable droppable) {}
    public virtual void OnItemAboveDrag(Droppable droppable) {}
    public virtual void OnItemAboveEndDrag(Droppable droppable) {}

    void OnOwnerChanged(NetworkInstanceId ownerId)
    {
        this.ownerNetId = ownerId;
    }

    public override void  OnLongTouch(Vector3 touchPosition)
    {
        base.OnLongTouch(touchPosition);

        if (contextMenuComponent != null)
            contextMenuComponent.Show(touchPosition);
    }


    public virtual void UpdateCardsSortingOrder()
    {
        foreach (Transform child in cardsHolderTransform)
        {
            child.GetComponent<SpriteRenderer>().sortingOrder = child.GetSiblingIndex();
        }
    }

    protected void UpdateCardsExSiblingIndex()
    {
        for (int i = 1; i < cardsHolderTransform.childCount; i++)
        {
            Transform child = cardsHolderTransform.GetChild(i);
            child.GetComponent<CardView>().UpdateExSiblingIndex();
        }
    }

    public virtual void OnMenuItemClicked(ContextMenuItem menuItem) {
        if (menuItem.id == 0)
        {
            VoteManager.Instance.CreateVote("Let " + Player.LocalPlayer.playerName + " take ownership of " + zoneName + "?",
                () =>
                {
                    CommandProcessor.Instance.ExecuteClientCommand(new CommandSetZoneOwner(this, Player.LocalPlayer));
                    //MyNetwork.Instance.SetZoneOwner(this.netId, MyNetwork.Instance.Player.netId);
                    UnityEngine.Debug.Log("Owner set");
                },
                () => { UnityEngine.Debug.Log("Vote Not Passsed"); });
        }
        
    }

    public virtual void OnCardTouched(CardView card)
    {

    }

}
