using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public CardData MyCardData { get; set; }

    public SaveData()
    {
        MyCardData = new CardData();
    }
}


[Serializable]
public class CardData
{
    public Dictionary<string, int> ownedCards { get; set; }
    public Dictionary<string, int> deckCards { get; set; }

    public CardData()
    {
        ownedCards = new Dictionary<string, int>();
        deckCards = new Dictionary<string, int>();
    }
}