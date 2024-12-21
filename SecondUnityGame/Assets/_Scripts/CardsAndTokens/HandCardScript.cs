using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HandCardScript : MonoBehaviour
{
    List<GameObject> myHandCards;
    int maximumHandCards = 15;
    [SerializeField] GameObject mySimpleCardPrefab;

    public static HandCardScript instance;

    private void Awake()
    {
        instance = this;
        myHandCards = new List<GameObject>();
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
        if (myHandCards.Count <= 9) GetComponent<HorizontalLayoutGroup>().spacing = 2.3f;
        else
        {
            float cardWidth = transform.GetChild(0).GetComponent<RectTransform>().rect.width;
            float newSpacing = GetComponent<HorizontalLayoutGroup>().spacing = - (myHandCards.Count - 9) * cardWidth / (myHandCards.Count - 1);
            GetComponent<HorizontalLayoutGroup>().spacing = newSpacing;
        }
    }

    public void AddCard(GameObject newCard)
    {
        FetchAllCards();
        if (myHandCards.Count >= maximumHandCards) return;

        GameObject myNewCard = Instantiate(newCard, transform);
        myHandCards.Add(myNewCard);

        ScaleUIBasedOnCardCount();
    }

    public void RemoveCard(GameObject oldCard)
    {

        FetchAllCards();

        myHandCards.Remove(oldCard);

        ScaleUIBasedOnCardCount();
    }


    public void AddSimpleCardForDebugging()
    {
        AddCard(mySimpleCardPrefab);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Spacebar pressed...");
            AddSimpleCardForDebugging();
        }
    }
}
