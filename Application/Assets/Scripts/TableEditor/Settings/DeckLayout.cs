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

    /*public void SaveToJson(string fileName)
    {
        DirectoryInfo di = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "Decks"));
        if (!di.Exists)
            di.Create();

        Debug.Log(Path.Combine(Application.persistentDataPath, Path.Combine("Decks", fileName)));
        var sr = File.CreateText(Path.Combine(Application.persistentDataPath, Path.Combine("Decks", fileName)));
        sr.WriteLine(JsonUtility.ToJson(this));

        sr.Close();
    }

    public void LoadFromJson(TextAsset file)
    {
        cards = JsonUtility.FromJson<List<int>>(file.text);
    }*/
}
