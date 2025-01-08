using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    // Card Management Stuff
    public GameObject previewCard;
    [SerializeField] GameObject deckAndDiscardPileViewer;

    [SerializeField] GameObject lastDiscardedCard;
    List<CardPrefabScriptable> discardPile = new List<CardPrefabScriptable>();

    List<CardPrefabScriptable> cardDeck1 = new List<CardPrefabScriptable>();
    List<CardPrefabScriptable> cardDeck2 = new List<CardPrefabScriptable>();
    List<CardPrefabScriptable> cardDeck3 = new List<CardPrefabScriptable>();
    Dictionary<int, GameObject> handCards;

    Dictionary<int, List<CardPrefabScriptable>> allDeckList = new Dictionary<int, List<CardPrefabScriptable>>();

    // UI Stuff
    [SerializeField] RawImage deck1Image, deck2Image, deck3Image, discardPileImage;
    Dictionary<int, RawImage> allDeckImages = new Dictionary<int, RawImage>();
    [SerializeField] GameObject mySimpleCardPrefab;

    Dictionary<int, Texture> deckBackImages = new Dictionary<int, Texture>();
    [SerializeField] Texture deckBackImage1, deckBackImage2, deckBackImage3, deckBackImage4, deckBackImage5;

    #region Liste aller Karten
    [SerializeField] CardPrefabScriptable Archer;
    [SerializeField] CardPrefabScriptable Solder;

    [SerializeField] List<CardPrefabScriptable> listOfAllCards = new List<CardPrefabScriptable>();

    #endregion

    private void Awake()
    {
        instance = this;
        deckAndDiscardPileViewer.SetActive(false);
        for (int i = 0; i < deckAndDiscardPileViewer.transform.Find("Content").childCount; i++)
        {
            deckAndDiscardPileViewer.transform.Find("Content").GetChild(i).gameObject.SetActive(false);
        }
    }

    private void Start()
    { 
        deckBackImages.Add(1, deckBackImage1);
        deckBackImages.Add(2, deckBackImage2);
        deckBackImages.Add(3, deckBackImage3);
        deckBackImages.Add(4, deckBackImage4);
        deckBackImages.Add(5, deckBackImage5);

        allDeckList.Add(1, cardDeck1);
        allDeckList.Add(2, cardDeck2);
        allDeckList.Add(3, cardDeck3);

        allDeckImages.Add(1, deck1Image);
        allDeckImages.Add(2, deck2Image);
        allDeckImages.Add(3, deck3Image);

        for (int i = 0; i < 3; i++)
        {
            UpdateDeckUI(i + 1);
        }

        UpdateDiscardPileUI();
    }

    public void AddSimpleCardToHandForDebugging()
    {
        GameObject newCard = mySimpleCardPrefab;
        newCard.GetComponent<MainCardScript>().myCardScriptable = listOfAllCards[Random.Range(0, listOfAllCards.Count)];
        HandCardScript.instance.AddCard(newCard);
    }

    public void AddCardToDeckForDebugging(int deckNumber)
    {
        AddCardToDeck(listOfAllCards[Random.Range(0, listOfAllCards.Count)], deckNumber);
    }

    public void DrawCardFromDeckForDebugging(int decknumber)
    {
        int deckSize = allDeckList[decknumber].Count;
        if (deckSize == 0) return;

        DrawCardFromDeck(allDeckList[decknumber][Random.Range(0, deckSize)], decknumber);
    }

    public void DrawCardFromDeck(CardPrefabScriptable cardScript, int decknumber)
    {
        GameObject newHandCard = mySimpleCardPrefab;
        newHandCard.GetComponent<MainCardScript>().myCardScriptable = cardScript;
        bool hasAddedCard = HandCardScript.instance.AddCard(newHandCard);
        if (hasAddedCard) RemoveCardFromDeck(cardScript, decknumber);
    }

    public void AddCardToDeck(CardPrefabScriptable cardScript, int decknumber)
    {
        allDeckList[decknumber].Add(cardScript);
        UpdateDeckUI(decknumber);
    }

    public void AddCardToDiscardPile(CardPrefabScriptable cardScript)
    {
        discardPile.Add(cardScript);
        UpdateDiscardPileUI();
    }

    public void RemoveCardFromDiscardPile(int position)
    {
        discardPile.RemoveAt(position);
        UpdateDiscardPileUI();
    }

    public void RemoveCardFromDiscardPile(CardPrefabScriptable cardScript)
    {
        discardPile.Remove(cardScript);
        UpdateDiscardPileUI();
    }

    public void RemoveCardFromDeck(CardPrefabScriptable cardScript, int decknumber)
    {
        allDeckList[decknumber].Remove(cardScript);
        UpdateDeckUI(decknumber);
    }

    public void ShowAndHideDiscardPile()
    {
        if (!deckAndDiscardPileViewer.activeSelf)
        {
            deckAndDiscardPileViewer.SetActive(true);

            for (int i = 0; i < deckAndDiscardPileViewer.transform.Find("Content").childCount; i++)
            {
                deckAndDiscardPileViewer.transform.Find("Content").GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < discardPile.Count; i++)
            {
                deckAndDiscardPileViewer.transform.Find("Content").GetChild(i).gameObject.SetActive(true);
                deckAndDiscardPileViewer.transform.Find("Content").GetChild(i).GetComponent<MainCardScript>().myCardScriptable = discardPile[i];
            }
        }
        else
        {
            deckAndDiscardPileViewer.SetActive(false);
        }
    }

    public void ShowAndHideDeck(int deckNumber)
    {
        if (!deckAndDiscardPileViewer.activeSelf)
        {
            deckAndDiscardPileViewer.SetActive(true);

            List<CardPrefabScriptable> shuffledDeck = allDeckList[deckNumber];
            int maxCount = shuffledDeck.Count;

            for (int k = 0; k < maxCount - 1; k++)
            {
                var r = Random.Range(k, maxCount);
                var tmp = shuffledDeck[k];
                shuffledDeck[k] = shuffledDeck[r];
                shuffledDeck[r] = tmp;
            }

            for (int i = 0; i < deckAndDiscardPileViewer.transform.Find("Content").childCount; i++)
            {
                deckAndDiscardPileViewer.transform.Find("Content").GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < allDeckList[deckNumber].Count; i++)
            {
                deckAndDiscardPileViewer.transform.Find("Content").GetChild(i).gameObject.SetActive(true);
                deckAndDiscardPileViewer.transform.Find("Content").GetChild(i).GetComponent<MainCardScript>().myCardScriptable = shuffledDeck[i];
                deckAndDiscardPileViewer.transform.Find("Content").GetChild(i).GetComponent<MainCardScript>().FetchFields();
                deckAndDiscardPileViewer.transform.Find("Content").GetChild(i).GetComponent<MainCardScript>().UpdateCardUI();
            }
        }
        else
        {
            deckAndDiscardPileViewer.SetActive(false);
        }
    }

    void UpdateDeckUI(int decknumber)
    {
        if (allDeckList[decknumber].Count == 0)
        {
            allDeckImages[decknumber].gameObject.SetActive(false);
        }

        if (allDeckList[decknumber].Count >= 1)
        {
            allDeckImages[decknumber].texture = deckBackImages[Mathf.Clamp(allDeckList[decknumber].Count, 0, 5)];
            allDeckImages[decknumber].gameObject.SetActive(true);
        }
    }

    private void UpdateDiscardPileUI()
    {
        Debug.Log("Updating UI for Discard pile");
        if (discardPile.Count == 0)
        {
            discardPileImage.gameObject.SetActive(false);
            lastDiscardedCard.SetActive(false);
        }

        if (discardPile.Count >= 1)
        {
            discardPileImage.gameObject.SetActive(false);
            lastDiscardedCard.SetActive(true);
            Debug.Log("Discard Pile newest card: " + discardPile[discardPile.Count - 1]);
            lastDiscardedCard.GetComponent<MainCardScript>().myCardScriptable = discardPile[discardPile.Count - 1];
            lastDiscardedCard.GetComponent<MainCardScript>().UpdateCardUI();
            lastDiscardedCard.GetComponent<CanvasGroup>().blocksRaycasts = false;

            discardPileImage.texture = deckBackImages[Mathf.Clamp(discardPile.Count, 0, 5)];
            discardPileImage.gameObject.SetActive(true);

            if (discardPile.Count >= 2)
            {

            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Spacebar pressed...");
            AddSimpleCardToHandForDebugging();
        }
    }
}