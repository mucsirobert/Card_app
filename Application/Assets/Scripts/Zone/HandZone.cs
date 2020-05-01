using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HideHand))]
public class HandZone : HorizontalZone {

    [SerializeField]
    private ImageToggle showCardsIcon;

    public bool IsOwnHand { get; set; }

    private HideHand hideHandComponent;

    public Button ownShowHandButton;
    public Button otherShowHandButton;


    public Vector3 ShowPosition { get; set; }
    public Vector3 HiddenPosition { get; set; }

    private bool cardsAreVisibleToOthers;

    public bool CardsAreVisibleToOthers {
        get
        {
            return cardsAreVisibleToOthers;
        }
        set
        {
            cardsAreVisibleToOthers = value;
            showCardsIcon.Toggle(cardsAreVisibleToOthers);
        }
    }


    protected override void Awake()
    {
        base.Awake();

        hideHandComponent = GetComponent<HideHand>();
    }

    protected override void Start()
    {
        ownerTakeAwayPermissionType = Permission.PermissionType.ALLOW;
        ownerDropOntoPermissionType = Permission.PermissionType.ALLOW;
        ownerViewPermissionType = Permission.PermissionType.ALLOW;

        othersTakeAwayPermissionType = Permission.PermissionType.DENY;
        othersDropOntoPermissionType = Permission.PermissionType.DENY;
        othersViewPermissionType = Permission.PermissionType.DENY;
        base.Start();

        hideHandComponent.ShownPosition = ShowPosition;
        hideHandComponent.HiddenPosition = HiddenPosition;

        if (IsOwnHand)
        {
            otherShowHandButton.gameObject.SetActive(false);
            ownShowHandButton.onClick.AddListener(hideHandComponent.Toggle);
            showCardsIcon.gameObject.SetActive(true);
        }
        else
        {
            ownShowHandButton.gameObject.SetActive(false);
            otherShowHandButton.onClick.AddListener(hideHandComponent.Toggle);
            showCardsIcon.gameObject.SetActive(false);
        }

        defauldIsFacingUp = false;
    }

    protected override void PlaceCard(CardView card, int siblingIndex)
    {
        base.PlaceCard(card, siblingIndex);

        if (ownerNetId == Player.LocalPlayer.netId)
            card.SetLocallyFacingUp(true);

        card.SetFacingUp(CardsAreVisibleToOthers);
    }

    public void FlipCardsInHand()
    {
        if (cardsHolderTransform.childCount <= 0)
            return;

        if (!CardsAreVisibleToOthers)
        {
            QuestionPanel.Show("This will make cards in your hand visible to other players!", () =>
            {
                CommandProcessor.Instance.ExecuteClientCommand(new CommandShowHand(Player.LocalPlayer, this, true));
                CardsAreVisibleToOthers = true;
            }, null);
        } else {
            QuestionPanel.Show("This will make cards in your hand looking face down for other players!", () =>
            {
                CommandProcessor.Instance.ExecuteClientCommand(new CommandShowHand(Player.LocalPlayer, this, false));
                CardsAreVisibleToOthers = false;
            }, null);
        }

        
    }

    public List<CardView> GetCardsInhand()
    {
        List<CardView> cards = new List<CardView>();

        for (int i = 0; i < cardsHolderTransform.childCount; i++)
        {
            cards.Add(cardsHolderTransform.GetChild(i).GetComponent<CardView>());
        }

        return cards;
    }

    public override void OnCardTouched(CardView card)
    {
        //Don't want to call base method, because players cannot flip cards in their hands
    }

}
