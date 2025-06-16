using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HandCardScript : MonoBehaviour
{
    List<GameObject> myHandCards;
    int maximumHandCards = 10;
    [SerializeField] GameObject mySimpleCardPrefab;

    public static HandCardScript instance;

    private void Awake()
    {
        instance = this;
        myHandCards = new List<GameObject>();
    }

    private void FetchAllCards()
    {
        myHandCards.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            myHandCards.Add(transform.GetChild(i).gameObject);
        }
    }

    private void ScaleUIBasedOnCardCount()
    {
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

    public bool AddCard(GameObject newCard)
    {
        FetchAllCards();
        if (myHandCards.Count >= maximumHandCards) return false;
        else
        {
            GameObject myNewCard = Instantiate(newCard, transform);
            myHandCards.Add(myNewCard);

            ScaleUIBasedOnCardCount();
            return true;
        }
    }

    public void RemoveCard(GameObject oldCard)
    {
        FetchAllCards();
        myHandCards.Remove(oldCard);
        ScaleUIBasedOnCardCount();
    }

    public void RemoveRandomCard()
    {
        if (transform.childCount != 0)
        {
            RemoveCard(transform.GetChild(0).gameObject);
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}
