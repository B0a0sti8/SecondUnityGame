using System.Collections.Generic;
using UnityEngine;

public class ListOfAllCards : MonoBehaviour
{
    public Dictionary<DefaultCardScriptable, int> ownedCards;
    public List<DefaultCardScriptable> allCardsList;

    public static ListOfAllCards instance;

    public Dictionary<DefaultCardScriptable, int> myDeckList;

    private void Awake()
    {
        myDeckList = new Dictionary<DefaultCardScriptable, int>();
        instance = this;
        ownedCards = new Dictionary<DefaultCardScriptable, int>();
        //allCardsList = new List<DefaultCardScriptable>();
    }

    public DefaultCardScriptable FindCardByName(string myCardName)
    {
        foreach (DefaultCardScriptable card in allCardsList)
        {
            if (card.cardName == myCardName)
            {
                return card;
            }
        }
        return null;
    }
}
