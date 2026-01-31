using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    // Card Management Stuff
    public GameObject previewCard;
    GameObject deckAndDiscardPileViewer;

    GameObject lastDiscardedCard;
    List<DefaultCardScriptable> discardPile = new List<DefaultCardScriptable>();

    [SerializeField] List<DefaultCardScriptable> cardDeck1;

    //Dictionary<int, GameObject> handCards;

    // UI Stuff
    RawImage deck1Image, discardPileImage;
    [SerializeField] GameObject mySimpleCardPrefab;

    Dictionary<int, Texture> deckBackImages = new Dictionary<int, Texture>();
    [SerializeField] Texture deckBackImage1, deckBackImage2, deckBackImage3, deckBackImage4, deckBackImage5;

    [SerializeField] List<DefaultCardScriptable> listOfAllCards = new List<DefaultCardScriptable>();

    private void Awake()
    {
        instance = this;
        SceneManager.sceneLoaded += InitRefs;
        if (SceneManager.GetActiveScene().name == "WorldMap") gameObject.SetActive(false);
        else gameObject.SetActive(true);

        deckBackImages.Add(1, deckBackImage1);
        deckBackImages.Add(2, deckBackImage2);
        deckBackImages.Add(3, deckBackImage3);
        deckBackImages.Add(4, deckBackImage4);
        deckBackImages.Add(5, deckBackImage5);
    }

    private void Start()
    {

    }

    public void InitRefs(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "WorldMap") gameObject.SetActive(false);
        else
        {
            Transform mCan = GameObject.Find("MainCanvas").transform;
            previewCard = mCan.Find("PreviewSlot").gameObject;
            deckAndDiscardPileViewer = mCan.Find("CardCanvas").Find("DeckAndPileView").Find("Scroll").gameObject;

            lastDiscardedCard = mCan.Find("CardCanvas").Find("UsedCards").Find("DiscardPile").Find("SimpleCard").gameObject;

            deck1Image = mCan.Find("CardCanvas").Find("Decks").Find("Deck1").GetComponent<RawImage>();
            discardPileImage = mCan.Find("CardCanvas").Find("UsedCards").Find("DiscardPile").GetComponent<RawImage>();

            gameObject.SetActive(true);

            Transform addCardButton = MainCanvasSingleton.instance.transform.Find("Buttons").Find("AddCardToHandButton");
            Transform removeCardButton = MainCanvasSingleton.instance.transform.Find("Buttons").Find("RemoveHandCard");
            Transform drawCardFromDeckButton = MainCanvasSingleton.instance.transform.Find("Buttons").Find("DrawCardFromDeck");
            Transform discardCardButton = MainCanvasSingleton.instance.transform.Find("Buttons").Find("DiscardCard");
            Transform scryCardButton = MainCanvasSingleton.instance.transform.Find("Buttons").Find("ScryCards");
            Transform closeDeckAndDiscardViewWindowButton = MainCanvasSingleton.instance.transform.Find("CardCanvas").Find("DeckAndPileView").Find("Scroll").Find("Button");

            addCardButton?.GetComponent<Button>().onClick.AddListener(() => AddSimpleCardToHandForDebugging());
            removeCardButton?.GetComponent<Button>().onClick.AddListener(() => HandCardScript.instance.RemoveRandomCard());
            drawCardFromDeckButton?.GetComponent<Button>().onClick.AddListener(() => DrawNextCardFromDeck());
            discardCardButton?.GetComponent<Button>().onClick.AddListener(() => DiscardRandomCard());
            scryCardButton?.GetComponent<Button>().onClick.AddListener(() => ScryCardsTest());
            closeDeckAndDiscardViewWindowButton?.GetComponent<Button>().onClick.AddListener(() => ShowAndHideDiscardPile());

            deck1Image.gameObject.GetComponent<Button>().onClick.AddListener(() => ShowAndHideDeck());
            discardPileImage.gameObject.GetComponent<Button>().onClick.AddListener(() => ShowAndHideDiscardPile());

            deckAndDiscardPileViewer.SetActive(false);
            for (int i = 0; i < deckAndDiscardPileViewer.transform.Find("Content").childCount; i++)
            {
                deckAndDiscardPileViewer.transform.Find("Content").GetChild(i).gameObject.SetActive(false);
            }

            if (ListOfAllCards.instance != null) LoadDeck();

            UpdateDeckUI();
            UpdateDiscardPileUI();
        }
    }

    public void CloseSceneDeInitRefs()
    {
        Transform addCardButton = MainCanvasSingleton.instance.transform.Find("Buttons").Find("AddCardToHandButton");
        Transform removeCardButton = MainCanvasSingleton.instance.transform.Find("Buttons").Find("RemoveHandCard");
        Transform drawCardFromDeckButton = MainCanvasSingleton.instance.transform.Find("Buttons").Find("DrawCardFromDeck");
        Transform discardCardButton = MainCanvasSingleton.instance.transform.Find("Buttons").Find("DiscardCard");
        Transform scryCardButton = MainCanvasSingleton.instance.transform.Find("Buttons").Find("ScryCards");
        Transform closeDeckAndDiscardViewWindowButton = MainCanvasSingleton.instance.transform.Find("CardCanvas").Find("DeckAndPileView").Find("Scroll").Find("Button");

        addCardButton?.GetComponent<Button>().onClick.RemoveListener(() => AddSimpleCardToHandForDebugging());
        removeCardButton?.GetComponent<Button>().onClick.RemoveListener(() => HandCardScript.instance.RemoveRandomCard());
        drawCardFromDeckButton?.GetComponent<Button>().onClick.RemoveListener(() => DrawNextCardFromDeck());
        discardCardButton?.GetComponent<Button>().onClick.RemoveListener(() => DiscardRandomCard());
        scryCardButton?.GetComponent<Button>().onClick.RemoveListener(() => ScryCardsTest());
        closeDeckAndDiscardViewWindowButton?.GetComponent<Button>().onClick.RemoveListener(() => ShowAndHideDiscardPile());

        discardPile.Clear();
        UpdateDiscardPileUI();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= InitRefs;
    }

    public void LoadDeck()
    {
        cardDeck1.Clear();
        foreach (KeyValuePair<DefaultCardScriptable, int> keyVal in ListOfAllCards.instance.myDeckList)
        {
            for (int i = 0; i < keyVal.Value; i++) cardDeck1.Add(keyVal.Key);
        }

        ShuffleDeck();
    }

    public void AddSimpleCardToHandForDebugging()
    {
        GameObject newCard = mySimpleCardPrefab;
        newCard.GetComponent<MainCardScript>().myCardToken = ListOfAllCards.instance.allCardsList[Random.Range(0, ListOfAllCards.instance.allCardsList.Count)];//listOfAllCards[Random.Range(0, listOfAllCards.Count)];
        HandCardScript.instance.AddCard(newCard, false);
    }

    public void AddCardToDeckForDebugging()
    {
        AddCardToDeck(listOfAllCards[Random.Range(0, listOfAllCards.Count)]);
    }

    public void AddCardToDeck(DefaultCardScriptable cardScript)
    {
        cardDeck1.Add(cardScript);
        UpdateDeckUI();
    }

    public void DrawNextCardFromDeck()
    {
        Debug.Log("Drawing Card From Deck");
        if (cardDeck1.Count == 0) return;

        DefaultCardScriptable cardScript = cardDeck1[0];
        GameObject newHandCard = mySimpleCardPrefab;
        newHandCard.GetComponent<MainCardScript>().myCardToken = cardScript;
        bool hasAddedCard = HandCardScript.instance.AddCard(newHandCard);
        if (hasAddedCard) RemoveCardFromDeck(0);
    }

    public void DrawMultipleCards(int noOfCards)
    {
        DrawNextCardFromDeck();
        for (int i = 1; i < noOfCards; i++)
        {
            StartCoroutine(DrawNextCardDelay(i*0.13f));
        }
    }

    IEnumerator DrawNextCardDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        DrawNextCardFromDeck();
    }

    public void DiscardRandomCard()
    {
        if (HandCardScript.instance.transform.childCount != 0)
        {
            HandCardScript.instance.DiscardCard(HandCardScript.instance.transform.GetChild(0).gameObject);
            HandCardScript.instance.transform.GetChild(0).GetComponent<MainCardScript>().MarkForDiscard();
        }
    }

    public void DiscardSpecificCard(GameObject oldCard)
    {
        HandCardScript.instance.DiscardCard(oldCard);
        oldCard.GetComponent<MainCardScript>().MarkForDiscard();
    }

    public void DiscardHand()
    {
        HandCardScript.instance.FetchAllCards();
        if (HandCardScript.instance.myHandCards.Count == 0) return;

        HandCardScript.instance.myHandCards.ForEach(t => t.GetComponent<MainCardScript>().MarkForDiscard());

        //DiscardSpecificCard(HandCardScript.instance.myHandCards.Last());
        for (int i = HandCardScript.instance.myHandCards.Count-1; i >= 0; i--)
        {
            StartCoroutine(DiscardNextCardDelay(0.15f*i, HandCardScript.instance.myHandCards[i]));
        }
    }

    IEnumerator DiscardNextCardDelay(float waitTime, GameObject card)
    {
        yield return new WaitForSeconds(waitTime);
        DiscardSpecificCard(card);
    }

    public void AddCardToDiscardPile(DefaultCardScriptable cardScript)
    {
        discardPile.Add(cardScript);
        UpdateDiscardPileUI();
    }

    public void RemoveCardFromDiscardPile(int position)
    {
        discardPile.RemoveAt(position);
        UpdateDiscardPileUI();
    }

    public void RemoveCardFromDiscardPile(DefaultCardScriptable cardScript)
    {
        discardPile.Remove(cardScript);
        UpdateDiscardPileUI();
    }

    public void RemoveCardFromDeck(int index)
    {
        cardDeck1.RemoveAt(index);
        UpdateDeckUI();
    }

    public void RemoveCardFromDeck(DefaultCardScriptable cardScript)
    {
        cardDeck1.Remove(cardScript);
        UpdateDeckUI();
    }

    public void ShowAndHideDiscardPile()
    {
        Debug.Log("Showing Discard Pile");
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
                deckAndDiscardPileViewer.transform.Find("Content").GetChild(i).GetComponent<MainCardScript>().myCardToken = discardPile[i];
                deckAndDiscardPileViewer.transform.Find("Content").GetChild(i).GetComponent<MainCardScript>().FetchFields();
                deckAndDiscardPileViewer.transform.Find("Content").GetChild(i).GetComponent<MainCardScript>().UpdateCardUI();
            }
        }
        else
        {
            deckAndDiscardPileViewer.SetActive(false);
        }
    }

    public void ShuffleDeck()
    {
        List<DefaultCardScriptable> shuffledDeck = cardDeck1;
        int maxCount = cardDeck1.Count;

        for (int k = 0; k < maxCount - 1; k++)
        {
            var r = Random.Range(k, maxCount);
            var tmp = shuffledDeck[k];
            shuffledDeck[k] = shuffledDeck[r];
            shuffledDeck[r] = tmp;
        }

        cardDeck1 = shuffledDeck;
    }

    public void ShowAndHideDeck()
    {
        if (!deckAndDiscardPileViewer.activeSelf)
        {
            deckAndDiscardPileViewer.SetActive(true);

            List<DefaultCardScriptable> shuffledDeck = cardDeck1;
            int maxCount = cardDeck1.Count;

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

            for (int i = 0; i < maxCount; i++)
            {
                deckAndDiscardPileViewer.transform.Find("Content").GetChild(i).gameObject.SetActive(true);
                deckAndDiscardPileViewer.transform.Find("Content").GetChild(i).GetComponent<MainCardScript>().myCardToken = shuffledDeck[i];
                deckAndDiscardPileViewer.transform.Find("Content").GetChild(i).GetComponent<MainCardScript>().FetchFields();
                deckAndDiscardPileViewer.transform.Find("Content").GetChild(i).GetComponent<MainCardScript>().UpdateCardUI();
            }
        }
        else
        {
            deckAndDiscardPileViewer.SetActive(false);
        }
    }

    void UpdateDeckUI()
    {
        if (cardDeck1.Count == 0) deck1Image.gameObject.SetActive(false);

        if (cardDeck1.Count >= 1)
        {
            deck1Image.texture = deckBackImages[Mathf.Clamp(cardDeck1.Count, 0, 5)];
            deck1Image.gameObject.SetActive(true);
        }
    }

    void UpdateDiscardPileUI()
    {
        if (discardPile.Count == 0)
        {
            discardPileImage.gameObject.SetActive(false);
            lastDiscardedCard.SetActive(false);
        }

        if (discardPile.Count >= 1)
        {
            discardPileImage.gameObject.SetActive(false);
            lastDiscardedCard.SetActive(true);
            lastDiscardedCard.GetComponent<MainCardScript>().myCardToken = discardPile[discardPile.Count - 1];
            lastDiscardedCard.GetComponent<MainCardScript>().UpdateCardUI();
            //lastDiscardedCard.GetComponent<CanvasGroup>().blocksRaycasts = false;

            discardPileImage.texture = deckBackImages[Mathf.Clamp(discardPile.Count, 0, 5)];


            if (discardPile.Count >= 1)
            {
                discardPileImage.gameObject.SetActive(true);
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


    public void ScryCards(int numberOfCards)
    {
        Debug.Log("Scrying");
        MainCanvasSingleton.instance.transform.Find("CardCanvas").Find("ScryView").GetComponent<ScryViewScript>().OpenScryView();
        //ScryViewScript.instance.OpenScryView();
    }

    public void ScryCardsTest()
    {
        ScryCards(5);
    }
}