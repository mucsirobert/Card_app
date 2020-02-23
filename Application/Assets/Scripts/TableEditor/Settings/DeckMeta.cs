using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckMeta", menuName = "Decks/DeckMeta", order = 1)]
public class DeckMeta : ScriptableObject {

    public int numberOfCards;

    public string deckImagePath;

    public string deckBackPath;

    public DeckLayout defaultDeckLayout; 
}
