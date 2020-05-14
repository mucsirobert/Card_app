using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class DeckLayout
{
    

    [SerializeField]
    public List<int> cardNums;

    public int deckIndex;

    public DeckLayout()
    {
        cardNums = new List<int>();
    }

    public DeckLayout Copy()
    {
        var ret = new DeckLayout
        {
            cardNums = this.cardNums,
            deckIndex = this.deckIndex
        };

        return ret;
    }

  
}
