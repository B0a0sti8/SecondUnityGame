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
        Debug.Log("Awake Function");
        myDeckList = new Dictionary<DefaultCardScriptable, int>();
        instance = this;
        ownedCards = new Dictionary<DefaultCardScriptable, int>();
        allCardsList = new List<DefaultCardScriptable>();
    }

    private void Start()
    {
        Debug.Log("I AM List: " + ListOfAllCards.instance);
        Debug.Log(myDeckList.Count);
    }
}
