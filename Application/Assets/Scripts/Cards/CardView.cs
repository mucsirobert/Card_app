using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class CardView : Entity
{
    [SyncVar]
    public string cardSpritePath;

    [SyncVar]
    public string backSpritePath;

    [SyncVar]
    public int cardSpriteIndex;


    public int SiblingIndex
    {
        get { return transform.GetSiblingIndex(); }
        set
        {
            transform.SetSiblingIndex(value);
            ExSiblingIndex = transform.GetSiblingIndex();
        }
    }


    [SyncVar(hook = "OnSortingOrderChanged")]
    public int sortingOrder;

    [SyncVar]
    public GameObject currentZoneObject;

    public bool isFaceingUp = false;

    private bool isLocallyFacingUp = false;

    public Sprite Sprite { get; private set; }

    private Sprite backSprite;

    public bool ExIsFacingUp { get; set; }

    public Zone CurrentZone { get; private set; }

    public int ExSiblingIndex { get; set; }

    private Droppable droppableComponent;

    public SpriteRenderer SpriteRenderer { get; private set; }

    private BoxCollider2D BoxCollider2D { get; set; }

    private Vector2 originalBoxColliderSize;

    public string dragSortingLayer;

    private Tweener currentAnimation;

    private static Dictionary<string, Sprite[]> cardSpritesDictionary;
    private static Dictionary<string, Sprite> backSpritesDictionary;

    public bool IsDraggable {
        get
        {
            return droppableComponent.IsDraggable;
        }
        set
        {
            droppableComponent.IsDraggable = value;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();


        CurrentZone = currentZoneObject.GetComponent<Zone>();
        CurrentZone.DropCardOnClients(this);

        droppableComponent.DropZoneBelow = CurrentZone.GetComponent<DropZone>();
    }

    protected override void Awake()
    {
        base.Awake();

        SpriteRenderer = GetComponent<SpriteRenderer>();
        droppableComponent = GetComponent<Droppable>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        originalBoxColliderSize = BoxCollider2D.size;
    }

    protected override void Start()
    {
        base.Start();

        if (cardSpritesDictionary == null)
        {
            cardSpritesDictionary = new Dictionary<string, Sprite[]>();
        }
        //Don't want to load the resource for every card
        Sprite[] sprites;
        if (cardSpritesDictionary.TryGetValue(cardSpritePath, out sprites))
        {
            Sprite = sprites[cardSpriteIndex];
        } else
        {
            //The spritesesources are not loaded yet, let's load it
            sprites = Resources.LoadAll<Sprite>(cardSpritePath);
            cardSpritesDictionary.Add(cardSpritePath, sprites);
            Sprite = sprites[cardSpriteIndex];
        }
        name = Sprite.name;
        if (backSpritesDictionary == null)
        {
            backSpritesDictionary = new Dictionary<string, Sprite>();
        }

        Sprite backSprite;
        if (backSpritesDictionary.TryGetValue(backSpritePath, out backSprite))
        {
            this.backSprite = backSprite;
        }
        else
        {
            //The spritesesources are not loaded yet, let's load it
            backSprite = Resources.Load<Sprite>(backSpritePath);
            backSpritesDictionary.Add(backSpritePath, backSprite);
            this.backSprite = backSprite;
        }

        

        if (isFaceingUp || isLocallyFacingUp)
            SpriteRenderer.sprite = Sprite;
        else
            SpriteRenderer.sprite = backSprite;

        SpriteRenderer.sortingOrder = sortingOrder;

        ExSiblingIndex = transform.GetSiblingIndex();
        ExIsFacingUp = isFaceingUp;

    }

    public void OnBeforeDrag()
    {
        droppableComponent.IsDraggable = currentAnimation == null || !currentAnimation.IsActive() || !currentAnimation.IsPlaying() ;
    }

    public void OnBeginDrag()
    {
        SpriteRenderer.sortingLayerName = dragSortingLayer;
        SpriteRenderer.maskInteraction = SpriteMaskInteraction.None;

        //This is needet because of the sprite mask on horizontal zones
        BoxCollider2D.size = originalBoxColliderSize;
        BoxCollider2D.offset = Vector2.zero;
        BoxCollider2D.enabled = true;

    }

    public void OnDropFailed()
    {
        ReturnToLastZone();
    }

    public void OnEnterZone(DropZone dropPlace)
    {
        //SpriteRenderer.sortingOrder = 9999;//dropPlace.transform.childCount;
    }

    public void FlipCard()
    {
        CommandProcessor.Instance.ExecuteClientCommand(new CommandSetFacingUp(Player.LocalPlayer, this, !isFaceingUp));
    }

    public override void OnTouch(Vector3 touchPosition)
    {
        if (CurrentZone != null)
        {
            CurrentZone.OnTouch(touchPosition);
            CurrentZone.OnCardTouched(this);
        }
 
        //ModalPanel.Instance().Choice("Do you want to flip the card?", () => SetFacingUp(!isFaceingUp));
        //SetFacingUp(!isFaceingUp);
    }

    public override void OnLongTouch(Vector3 touchPosition)
    {
        if (CurrentZone != null)
        {
            CurrentZone.OnLongTouch(touchPosition);
        }
    }

    /*public void SetFacingUp(bool b)
    {
        isFaceingUp = b;
    }*/

    public void SetFacingUp(bool b)
    {
        isFaceingUp = b;
        if (isFaceingUp || isLocallyFacingUp)
            SpriteRenderer.sprite = Sprite;
        else
            SpriteRenderer.sprite = backSprite;

        ExIsFacingUp = isFaceingUp;
    }

    public void SetLocallyFacingUp(bool b)
    {
        isLocallyFacingUp = b;
        if (isFaceingUp || isLocallyFacingUp)
            SpriteRenderer.sprite = Sprite;
        else
            SpriteRenderer.sprite = backSprite;
    }


    public void OnSortingOrderChanged(int sortingOrder)
    {
        SpriteRenderer.sortingOrder = sortingOrder;
    }

    public virtual void DroppedOnZone(Zone zone)
    {
        droppableComponent.DroppedOn(zone.DropZoneComponent);

        CurrentZone = zone;
        ExSiblingIndex = transform.GetSiblingIndex();
        ExIsFacingUp = isFaceingUp;
    }

    public void UpdateExSiblingIndex()
    {
        ExSiblingIndex = transform.GetSiblingIndex();
    }

    public void ReturnToLastZone()
    {
        CurrentZone.DropCardOnClients(this, ExSiblingIndex);
    }

    public void KillAnimation()
    {
        if (currentAnimation != null)
        {
            currentAnimation.Kill();
        }
    }

    public Tweener MoveTo(Vector3 position, float duration)
    {
        if (currentAnimation != null && currentAnimation.IsActive() && currentAnimation.IsPlaying())
        {
            currentAnimation.ChangeEndValue(position, -1, true);
        }
        else
        {
            currentAnimation = transform.DOLocalMove(position, duration);
        }

        return currentAnimation;
    }

}
