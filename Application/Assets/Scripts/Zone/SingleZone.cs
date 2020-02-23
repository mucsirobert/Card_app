using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class SingleZone : Zone {

    private PreviewZone previewZone;

    [SyncVar]
    public Permission.PermissionType ownerViewPermissionType;
    [SyncVar]
    public Permission.PermissionType othersViewPermissionType;

    public Permission ViewPermission { get; set; }

    [SyncVar]
    public bool canOnlyHoldOneCard;


    protected override void Start()
    {
        base.Start();
        previewZone = PreviewZone.Instance;

        //viewPermission = new Permission(Permission.AllowType.WARNING, Permission.AccessType.OWNERONLY);

        ViewPermission = new Permission(ownerViewPermissionType, othersViewPermissionType);

    }

    public override void OnItemDropped(CardView card)
    {
        if (canOnlyHoldOneCard && cardsHolderTransform.childCount > 0)
        {
            card.ReturnToLastZone();
            return;
        }


        base.OnItemDropped(card);

        DropCard(card, cardsHolderTransform.childCount);

        card.KillAnimation();
        card.MoveTo(Vector3.zero, cardDropSpeed).OnComplete(() => {
            UpdateCardSortingLayer(card);
        });

    }

    public override void DropCard(CardView card, int siblingIndex)
    {
        CommandProcessor.Instance.ExecuteClientCommand((new CommandDropCardTo(Player.LocalPlayer, this, card, siblingIndex)));

        base.DropCard(card, siblingIndex);
    }

    protected override void PlaceCard(CardView card, int siblingIndex)
    {
        base.PlaceCard(card, siblingIndex);

        card.SetFacingUp(defauldIsFacingUp);
    }

    public override void DropCardOnClients(CardView card, int siblingIndex = -1)
    {
        base.DropCardOnClients(card, siblingIndex);

        card.KillAnimation();
        card.MoveTo(Vector3.zero, cardMoveSpeed).OnComplete(() => {
            UpdateCardSortingLayer(card);
        });
    }


    public override void OnTouch(Vector3 touchPosition)
    {
        if (cardsHolderTransform.childCount > 0)
        {
            if (canOnlyHoldOneCard)
            {
                //TODO: Create a OnCardTouched method and do this in it
                cardsHolderTransform.GetChild(0).GetComponent<CardView>().FlipCard();
            }
            else
            {
                string warningString = "Are you sure you want to view cards on " + LogEntry.ZoneLogEntryPart(this) + "?";
                string voteString = "Allow " + LogEntry.PlayerLogEntryPart(Player.LocalPlayer) + " to view cards on " + LogEntry.ZoneLogEntryPart(this) + "?";
                ViewPermission.Check(Player.LocalPlayer, ownerNetId,
                    ShowPreview, () => { },
                    warningString, voteString);
            }
        }

    }


    private void ShowPreview()
    {
        List<CardView> cardViews = new List<CardView>();

        for (int i = 0; i < cardsHolderTransform.childCount; i++)
        {
            cardViews.Add(cardsHolderTransform.GetChild(i).GetComponent<CardView>());
        }

        previewZone.Init(cardViews, this);
    }

    
}
