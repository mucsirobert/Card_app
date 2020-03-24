using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class DeckView : Zone
{
    [SerializeField]
    private int numberOfVisibleCards = 20;

    private Vector3 pointerOffset;
    public CardView cardPrefab;
    //public List<Sprite> cardList;
    //public List<Sprite> cardList;
    private List<GameObject> cards;
    private List<int> cardNums = new List<int>();

    public DeckMeta DeckInfo { get; set; }
    public DeckLayout DeckData { get; set; }

    private Sequence shuffleAnimation;

    [SyncVar]
    public Permission.PermissionType ownerShufflePermissionType;
    [SyncVar]
    public Permission.PermissionType ownerDealPermissionType;
    [SyncVar]
    public Permission.PermissionType othersShufflePermissionType;
    [SyncVar]
    public Permission.PermissionType othersDealPermissionType;

    public Permission ShufflePermission { get; set; }
    public Permission DealPermission { get; set; }

    public List<int> CardNums
    {
        get
        {
            return cardNums;
        }

        set
        {
            cardNums = value;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        Debug.Log("Deck start client");

        if (isServer)
        {
            InitDeck();
        }

    }


    protected override void Start()
    {
        base.Start();
        Debug.Log(CardNums.Count);
        /*if (isServer)
        {
            InitDeck();
        }*/


        Debug.Log("Deck Start " + cardsHolderTransform.childCount);
        //Need to reorder cards based on the soriting order on the clients, because the cards are not always spawned in order (possible bug in HLAPI?)
        ReorderCards();

        shuffleAnimation = DOTween.Sequence();
        ShufflePermission = new Permission(ownerShufflePermissionType, othersShufflePermissionType);
        DealPermission = new Permission(ownerDealPermissionType, othersDealPermissionType);

    }

    protected override bool CanTakeAwayCards()
    {
        return !shuffleAnimation.IsActive() || !shuffleAnimation.IsPlaying();
    }

    [Server]
    public void InitDeck()
    {
        int sortingOrder = 0;

        for (int i = 0; i < DeckInfo.numberOfCards; i++)
        {
            for (int j = 0; j < DeckData.cardNums[i]; j++)
            {
                CardView cardToSpawn = Instantiate(cardPrefab, cardsHolderTransform.position, cardsHolderTransform.rotation);
                cardToSpawn.cardSpriteIndex = i;
                cardToSpawn.cardSpritePath = DeckInfo.deckImagePath;
                cardToSpawn.backSpritePath = DeckInfo.deckBackPath;
                cardToSpawn.currentZoneObject = gameObject;
                cardToSpawn.sortingOrder = sortingOrder;
                NetworkServer.Spawn(cardToSpawn.gameObject);
                cardNums.Add(sortingOrder);

                sortingOrder++;
            }

        }

    }



    [Server]
    public void ShuffleCards()
    {
        cardNums.Clear();
        int num = 0;
        foreach (Transform child in cardsHolderTransform)
        {
            cardNums.Add(num++);
        }

        Shuffle(cardNums);

        RpcSetCardOrders(cardNums.ToArray());
    }


    /*public void SetCardOrders(List<int> cardNums)
    {

        List<Transform> childs = new List<Transform>();
        foreach (Transform child in cardsHolderTransform)
        {
            childs.Add(child);
        }
        cardsHolderTransform.DetachChildren();

        for (int i = 0; i < childs.Count; i++)
        {
            CardView card = childs[cardNums[i]].GetComponent<CardView>();
            card.transform.SetParent(cardsHolderTransform);
            card.sortingOrder = i;
            card.SiblingIndex = i;
        }

        RpcSetCardsSpriteRendererEnabled();
    }

    [ClientRpc]
    private void RpcSetCardsSpriteRendererEnabled()
    {
        SetCardsSpriteRendererEnabled();
    }*/

    [ClientRpc]
    public void RpcSetCardOrders(int[] cardNumsParam)
    {
        Debug.Log("RpcSetCardOrders");
        this.cardNums = new List<int>(cardNumsParam);
        List<Transform> childs = new List<Transform>();
        foreach (Transform child in cardsHolderTransform)
        {
            childs.Add(child);
        }
        cardsHolderTransform.DetachChildren();

        for (int i = 0; i < childs.Count; i++)
        {
            CardView card = childs[cardNums[i]].GetComponent<CardView>();
            card.transform.SetParent(cardsHolderTransform);
            card.SpriteRenderer.sortingOrder = i;
            card.SiblingIndex = i;
        }

        SetCardsSpriteRendererEnabled();
    }

    private void ReorderCards()
    {
        List<CardView> cards = new List<CardView>();
        foreach (Transform child in cardsHolderTransform)
        {
            cards.Add(child.GetComponent<CardView>());
        }
        cardsHolderTransform.DetachChildren();

        cards.Sort((CardView card1, CardView card2) => card1.sortingOrder.CompareTo(card2.sortingOrder));

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.SetParent(cardsHolderTransform);
        }
    }


    public void PlayShuffleAnimation()
    {
        shuffleAnimation = DOTween.Sequence();

        int shuffleCardIndex = 0;
        for (int i = 0; i < cardsHolderTransform.childCount; i++)
        {
            if (i % 2 == 0)
            {
                shuffleAnimation.Insert(0, cardsHolderTransform.GetChild(i).DOLocalMoveX(-2f, 0.2f));
            }
            else
            {
                shuffleAnimation.Insert(0, cardsHolderTransform.GetChild(i).DOLocalMoveX(2f, 0.2f));
            }
            if (i >= cardsHolderTransform.childCount - numberOfVisibleCards)
            {
                if (shuffleCardIndex % 2 == 0)
                {
                    shuffleAnimation.Insert(0.25f + 0.07f * shuffleCardIndex, cardsHolderTransform.GetChild(i).DOLocalMove(Vector3.zero, 0.2f).SetEase(Ease.OutQuad));
                } else
                {
                    shuffleAnimation.Insert(0.25f + 0.07f * (shuffleCardIndex -1 ), cardsHolderTransform.GetChild(i).DOLocalMove(Vector3.zero, 0.2f).SetEase(Ease.OutQuad));
                }
                shuffleCardIndex++;
            }
            else
            {
                shuffleAnimation.Insert(0.25f, cardsHolderTransform.GetChild(i).DOLocalMove(Vector3.zero, 0f).SetEase(Ease.OutQuad));
            }
        }

    }

    public override void OnCardRemoved(CardView card)
    {
        base.OnCardRemoved(card);
        SetCardsSpriteRendererEnabled();
    }


    //https://stackoverflow.com/questions/273313/randomize-a-listt
    private static System.Random rng = new System.Random();

    private void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


    public override void DropCardOnClients(CardView card, int siblingIndex = -1)
    {
        base.DropCardOnClients(card, siblingIndex);


        card.KillAnimation();
        card.MoveTo(Vector3.zero, cardMoveSpeed).OnComplete(() =>
        {
            UpdateCardSortingLayer(card);
           
            SetCardsSpriteRendererEnabled();
        });
    }
    private void SetCardsSpriteRendererEnabled()
    {
        //Debug.Log("SetCardsSpriteRendererEnabled");
        for (int i = 0; i < cardsHolderTransform.childCount; i++)
        {
            CardView card = cardsHolderTransform.GetChild(i).GetComponent<CardView>();
            if (card.SpriteRenderer.sortingOrder < cardsHolderTransform.childCount - numberOfVisibleCards)
            {
                card.SpriteRenderer.enabled = false;
            } else
            {
                card.SpriteRenderer.enabled = true;
            }
        }
    }

    private void DealCardsToPlayers(int numberOfCards, Player[] playersToDealTo)
    {
        if (cardsHolderTransform.childCount <= 0)
            return;

        List<CardView> cardsToDeal = new List<CardView>();
        for (int i = 0; i < playersToDealTo.Length * numberOfCards && i < cardsHolderTransform.childCount; i++)
        {
            cardsToDeal.Add(cardsHolderTransform.GetChild(cardsHolderTransform.childCount - (i + 1)).GetComponent<CardView>());
        }
        CommandProcessor.Instance.ExecuteClientCommand(new CommandDealCardToPlayer(Player.LocalPlayer, playersToDealTo, cardsToDeal.ToArray()));
    }


    public override void OnMenuItemClicked(ContextMenuItem menuItem)
    {
        if (menuItem.id == 1)
        {
            //Shuffle
            if (cardsHolderTransform.childCount > 1)
            {
                string warningString = "Are you sure you want to shuffle cards on " + LogEntry.ZoneLogEntryPart(this) + "?";
                string voteString = "Allow " + LogEntry.PlayerLogEntryPart(Player.LocalPlayer) + " to shuffle cards on " + LogEntry.ZoneLogEntryPart(this) + "?";
                ShufflePermission.Check(Player.LocalPlayer, ownerNetId,
                    () =>
                    {
                        CommandProcessor.Instance.ExecuteClientCommand(new CommandShuffleDeck(Player.LocalPlayer, this));
                    }, null, warningString, voteString);
            }
        }
        else if (menuItem.id == 2)
        {
            //Deal
            string warningString = "Are you sure you want to deal cards from " + LogEntry.ZoneLogEntryPart(this) + "?";
            string voteString = "Allow " + LogEntry.PlayerLogEntryPart(Player.LocalPlayer) + " to deal cards from " + LogEntry.ZoneLogEntryPart(this) + "?";
            ShufflePermission.Check(Player.LocalPlayer, ownerNetId, OnDealMenuClicked, null, warningString, voteString);
        }
        else
        {
            base.OnMenuItemClicked(menuItem);
        }
    }

    private void OnDealMenuClicked()
    {
        string[] playerNames = new string[Player.Players.Count];
        for (int i = 0; i < playerNames.Length; i++)
        {
            playerNames[i] = Player.Players[i].playerName;
        }

        DealCardsDialog.Show("how many cards to deal?", cardsHolderTransform.childCount, playerNames, (int number, bool[] checkData) =>
        {
            List<Player> playersToDealTo = new List<Player>();
            for (int i = 0; i < Player.Players.Count; i++)
            {
                if (checkData[i])
                {
                    playersToDealTo.Add(Player.Players[i]);
                }
            }

            if (playersToDealTo.Count > 0 && number > 0)
            {
                DealCardsToPlayers(number, playersToDealTo.ToArray());
            }
        });
    }




}

