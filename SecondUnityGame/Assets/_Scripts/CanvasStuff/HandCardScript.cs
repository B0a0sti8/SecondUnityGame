using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;

public class HandCardScript : MonoBehaviour
{
    public List<GameObject> myHandCards;
    int maximumHandCards = 10;
    //[SerializeField] GameObject mySimpleCardPrefab;

    public static HandCardScript instance;

    private void Awake()
    {
        instance = this;
        myHandCards = new List<GameObject>();
    }

    private void Start()
    {
        FetchAllCards();
        ScaleUIBasedOnCardCount();
    }

    public void FetchAllCards()
    {
        myHandCards.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            myHandCards.Add(transform.GetChild(i).gameObject);
        }
    }

    public void ScaleUIBasedOnCardCount()
    {
        FetchAllCards();
        // Korrigiere die Reihenfolge in der Hierarchy basierend auf der Position der Karten
        var sortedHandcards = myHandCards.OrderBy(t => t.transform.position.x).ToList();
        for (int i = 0; i < sortedHandcards.Count; i++)
        {
            sortedHandcards[i].transform.SetSiblingIndex(i);
        }

        FetchAllCards();

        // Schiebe die Karten an die richtigen Positionen
        if (myHandCards.Count % 2 == 0)     // Wenn Kartenanzahl gerade
        {
            for (int i = 0; i < myHandCards.Count; i++)
            {
                myHandCards[i].transform.localPosition = new Vector2(((myHandCards.Count/2 - i) * -100) + 50, Mathf.Pow(((myHandCards.Count) / 2 - 0.5f - i), 2) * -5 - 20);
                myHandCards[i].transform.localEulerAngles = new Vector3(0, 0, ((myHandCards.Count) / 2 - i) * 5 - 2.5f);
            }
        }
        else                                // Wenn Kartenanzahl ungerade
        {
            for (int i = 0; i < myHandCards.Count; i++)
            {
                myHandCards[i].transform.localPosition = new Vector2((((myHandCards.Count)/2 - i) * -100), Mathf.Pow(((myHandCards.Count) / 2 - i), 2) * -5 - 20);
                myHandCards[i].transform.localEulerAngles = new Vector3(0, 0, ((myHandCards.Count) / 2 - i) * 5);
            }
        }
    }

    public bool AddCard(GameObject newCard, bool isDrawnFromDeck = true)
    {
        FetchAllCards();
        if (myHandCards.Count >= maximumHandCards) return false;
        else
        {
            if (isDrawnFromDeck)
            {
                GameObject myNewCard = Instantiate(newCard, transform);
                myHandCards.Add(myNewCard);
                ScaleUIBasedOnCardCount();

                myNewCard.GetComponent<MainCardScript>().targetPosition = myNewCard.transform.position;
                myNewCard.transform.position = transform.parent.Find("Decks").Find("Deck1").position;
                myNewCard.GetComponent<MainCardScript>().isMovingSomewhereDuration = 0.05f;
                myNewCard.GetComponent<MainCardScript>().isMovingSomewhere = true;
            }
            else
            {
                GameObject myNewCard = Instantiate(newCard, transform);
                myHandCards.Add(myNewCard);

                ScaleUIBasedOnCardCount();
            }

            return true;
        }
    }

    public void RemoveCard(GameObject oldCard, bool isRandomRemove = false)
    {
        FetchAllCards();
        myHandCards.Remove(oldCard);
        ScaleUIBasedOnCardCount();
    }

    public void DiscardCard(GameObject oldCard)
    {
        Debug.Log("Discarding");
        FetchAllCards();
        Vector3 tarPos = transform.parent.Find("UsedCards").Find("DiscardPile").position;
        oldCard.transform.SetParent(MainCanvasSingleton.instance.transform.Find("CardCanvas").Find("Other").Find("DiscardDummyParent"));
        oldCard.GetComponent<MainCardScript>().targetPosition = tarPos;
        oldCard.GetComponent<MainCardScript>().isMovingSomewhereDuration = 0.10f;
        oldCard.GetComponent<MainCardScript>().isMovingSomewhere = true;
        oldCard.GetComponent<MainCardScript>().isDiscarded = true;
        myHandCards.Remove(oldCard);
        ScaleUIBasedOnCardCount();
    }

    public void RemoveRandomCard()
    {
        if (transform.childCount != 0)
        {
            RemoveCard(transform.GetChild(0).gameObject, true);
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}
