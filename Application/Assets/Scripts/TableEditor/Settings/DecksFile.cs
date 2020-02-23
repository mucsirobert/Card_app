using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecksFile", menuName = "Decks/DecksFile", order = 1)]
public class DecksFile : ScriptableObject {

    public List<DeckNamesItem> decks;

    public List<string> GetDeckNames()
    {
        List<string> deckNames = new List<string>();

        foreach (var item in decks)
        {
            deckNames.Add(item.deckName);
        }

        return deckNames;
    }
}

[System.Serializable]
public class DeckNamesItem
{
    public string deckName;
    public DeckMeta deckMeta;
}
