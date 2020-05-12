using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckEditor : EditorEntity
{
    private static int zoneNumber = 0;

    [SerializeField]
    private DecksFile decksFile;
    public List<DeckEditor> clonedDecks = new List<DeckEditor>();

    public Permission.PermissionType OwnerShufflePermissionType { get; set; }
    public Permission.PermissionType OwnerDealPermissionType { get; set; }

    public Permission.PermissionType OthersShufflePermissionType { get; set; }
    public Permission.PermissionType OthersDealPermissionType { get; set; }


    private DeckMeta deckMeta;
    private DeckLayout deckLayout;

    public DeckMeta DeckMeta
    {
        get
        {
            return deckMeta;
        }
        set
        {
            deckMeta = value;
        }
    }

    public DeckLayout DeckLayout
    {
        get
        {
            return deckLayout;
        }
        set
        {
            deckLayout = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();

        if (string.IsNullOrEmpty(Name))
        {
            Name = "Deck" + (++zoneNumber);
        }

        if (deckLayout == null)
            deckLayout = decksFile.decks[0].deckMeta.defaultDeckLayout;
        if (deckMeta == null)
            deckMeta = decksFile.decks[0].deckMeta;
    }


    public override void Save(TableData tableData)
    {

        tableData.Add(new DeckEntityData(this.transform.position, Name, Color, deckMeta, deckLayout, OwnerTakeAwayPermissionType, OwnerShufflePermissionType, OwnerDealPermissionType, OthersTakeAwayPermissionType, OthersShufflePermissionType, OthersDealPermissionType, false, 999, false));
        foreach (DeckEditor item in clonedDecks)
        {
            tableData.Add(new DeckEntityData(item.transform.position, Name, Color, deckMeta, deckLayout, OwnerTakeAwayPermissionType, OwnerShufflePermissionType, OwnerDealPermissionType, OthersTakeAwayPermissionType, OthersShufflePermissionType, OthersDealPermissionType, false, 999, false));


        }
    }

    public void copyProperties(DeckEditor deck) {
        deck.OwnerTakeAwayPermissionType = this.OwnerTakeAwayPermissionType;
        deck.OthersTakeAwayPermissionType = this.OthersTakeAwayPermissionType;
        deck.FlipCardsWhenDropped = this.FlipCardsWhenDropped;
        deck.OwnerDropOntoPermissionType = this.OwnerDropOntoPermissionType;
        deck.OthersDropOntoPermissionType = this.OthersDropOntoPermissionType;
        deck.OwnerShufflePermissionType = this.OwnerShufflePermissionType;
        deck.OwnerDealPermissionType = this.OwnerDealPermissionType;
        deck.OthersShufflePermissionType = this.OthersShufflePermissionType;
        deck.OthersDealPermissionType = this.OthersDealPermissionType;
    }

    public override void OnMenuItemClicked(ContextMenuItem menuItem)
    {
        if (menuItem.id == 4) {
            DeckEditor cloned = Instantiate(this);
            copyProperties(cloned);
            cloned.transform.position = new Vector3(0, 0, 0f);
            clonedDecks.Add(cloned);
        }
        if (menuItem.id == 1) {
            //Zone settings
            DeckPermissionSettings.Show(this);
        } else if (menuItem.id == 2)
        {
            //Card settings
            DeckSettings.Show(this);
        } else {
            //Remove
            base.OnMenuItemClicked(menuItem);
        };
    }
}
