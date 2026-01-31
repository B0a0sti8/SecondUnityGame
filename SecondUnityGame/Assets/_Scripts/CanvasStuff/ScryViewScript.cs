using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScryViewScript : MonoBehaviour
{
    public List<GameObject> myScryCards;
    public static ScryViewScript instance;

    private void Awake()
    {
        instance = this;
    }

    public void FetchAllCards()
    {
        myScryCards.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            myScryCards.Add(transform.GetChild(i).gameObject);
        }
    }

    public void ScaleUIBasedOnCardCount()
    {
        Debug.Log("Scry 2 scale");
        FetchAllCards();
        Debug.Log("Fetched Cards");
        // Korrigiere die Reihenfolge in der Hierarchy basierend auf der Position der Karten
        var sortedHandcards = myScryCards.OrderBy(t => t.transform.position.x).ToList();
        for (int i = 0; i < sortedHandcards.Count; i++)
        {
            sortedHandcards[i].transform.SetSiblingIndex(i);
        }

        FetchAllCards();

        // Schiebe die Karten an die richtigen Positionen
        if (myScryCards.Count % 2 == 0)     // Wenn Kartenanzahl gerade
        {
            for (int i = 0; i < myScryCards.Count; i++)
            {
                myScryCards[i].transform.localPosition = new Vector2(((myScryCards.Count / 2 - i) * -100) + 50, 0);
            }
        }
        else                                // Wenn Kartenanzahl ungerade
        {
            for (int i = 0; i < myScryCards.Count; i++)
            {
                myScryCards[i].transform.localPosition = new Vector2((((myScryCards.Count) / 2 - i) * -100), 0);
            }
        }
    }

    public void OpenScryView()
    {
        gameObject.SetActive(true);
        Debug.Log("Scry 1");
        ScaleUIBasedOnCardCount();
    }
    public void CloseScryView()
    {
        gameObject.SetActive(false);
    }
}
