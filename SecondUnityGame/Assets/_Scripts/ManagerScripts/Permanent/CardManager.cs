using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

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
    }

    private void Start()
    {
        InitRefs(SceneManager.GetActiveScene(), LoadSceneMode.Additive);

        deckAndDiscardPileViewer.SetActive(false);
        for (int i = 0; i < deckAndDiscardPileViewer.transform.Find("Content").childCount; i++)
        {
            deckAndDiscardPileViewer.transform.Find("Content").GetChild(i).gameObject.SetActive(false);
        }

        if (ListOfAllCards.instance != null) LoadDeck();

        deckBackImages.Add(1, deckBackImage1);
        deckBackImages.Add(2, deckBackImage2);
        deckBackImages.Add(3, deckBackImage3);
        deckBackImages.Add(4, deckBackImage4);
        deckBackImages.Add(5, deckBackImage5);

        UpdateDeckUI();
        UpdateDiscardPileUI();
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
            deck1Image.gameObject.GetComponent<Button>().onClick.AddListener(() => ShowAndHideDeck());
            discardPileImage.gameObject.GetComponent<Button>().onClick.AddListener(() => ShowAndHideDiscardPile());
        }
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
        HandCardScript.instance.AddCard(newCard);
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
        if (cardDeck1.Count == 0) return;

        DefaultCardScriptable cardScript = cardDeck1[0];
        GameObject newHandCard = mySimpleCardPrefab;
        newHandCard.GetComponent<MainCardScript>().myCardToken = cardScript;
        bool hasAddedCard = HandCardScript.instance.AddCard(newHandCard);
        if (hasAddedCard) RemoveCardFromDeck(0);
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


            if (discardPile.Count >= 2)
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
}