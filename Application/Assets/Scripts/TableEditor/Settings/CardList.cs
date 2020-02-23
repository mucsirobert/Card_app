using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardList : MonoBehaviour
{
    public CardListItem cardListItemPrefab;

    public void Load(DeckMeta deckInfo, List<int> numberOfEachCard)
    {
        DestroyChildren();
        Sprite[] sprites = Resources.LoadAll<Sprite>(deckInfo.deckImagePath);

        for (int i = 0; i < deckInfo.numberOfCards; i++)
        {
            CardListItem cardListItem = Instantiate(cardListItemPrefab, this.transform);
            cardListItem.Sprite = sprites[i];
            if (numberOfEachCard.Count == deckInfo.numberOfCards)
                cardListItem.NumberOfCards = numberOfEachCard[i];
            else
                cardListItem.NumberOfCards = 1;
        }
    }

    public List<int> getNumberOfEachCard()
    {
        List<int> numberOfEachCard = new List<int>();
        for (int i = 0; i < transform.childCount; i++)
        {
            CardListItem item = transform.GetChild(i).GetComponent<CardListItem>();
            numberOfEachCard.Add(item.NumberOfCards);
        }
        return numberOfEachCard;
    }
    private void DestroyChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void PlusButtonClicked()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            CardListItem item = transform.GetChild(i).GetComponent<CardListItem>();
            item.NumberOfCards++;
        }
    }

    public void MinusButtonClicked()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            CardListItem item = transform.GetChild(i).GetComponent<CardListItem>();
            item.NumberOfCards = Math.Max(0, item.NumberOfCards-1);
        }
    }
}
