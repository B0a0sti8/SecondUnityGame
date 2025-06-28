using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckbuildingManager : MonoBehaviour
{
    [SerializeField] GameObject deckElementPrefab;
    Transform deckObject;
    Dictionary<DefaultCardScriptable, int> deckList;

    Transform allAvailableCardObject;
    Dictionary<DefaultCardScriptable, GameObject> allAvailableCardSlots;

    Dictionary<DefaultCardScriptable, int> completeCardDict;

    TextMeshProUGUI cardCounter;
    public int minimumCardsInDeck = 40;

    public static DeckbuildingManager instance;

    private void Awake()
    {
        instance = this;
        deckList = new Dictionary<DefaultCardScriptable, int>();
    }

    private void Start()
    {
        cardCounter = transform.Find("DeckViewer").Find("ForeGround").Find("CardCounter").GetComponent<TextMeshProUGUI>();
        deckObject = transform.Find("DeckViewer").Find("ForeGround").Find("CardsScroll").Find("Panel");
        allAvailableCardObject = transform.Find("AllCardPanel").Find("Content");

        allAvailableCardSlots = new Dictionary<DefaultCardScriptable, GameObject>();
        for (int i = 0; i < allAvailableCardObject.childCount; i++)
        {
            DefaultCardScriptable myCard = allAvailableCardObject.GetChild(i).Find("SimpleCard").GetComponent<MainCardScript>().myCardToken;
            allAvailableCardSlots.Add(myCard, allAvailableCardObject.GetChild(i).gameObject);
        }

        UpdateDeckUI();
    }

    public bool AddCardToDecklist(DefaultCardScriptable newCard)
    {
        if (deckList.ContainsKey(newCard)) // Schau ob schon Karten dieses Typs im Deck sind
        {
            if (deckList[newCard] < newCard.maxCardAmount) // Schau ob das Maximum der Karten erreicht ist
            {
                deckList[newCard] += 1; // Füge eine Karte hinzu
                UpdateDeckUI();
                return true;
            }
        }
        else
        {
            deckList.Add(newCard, 1);
            UpdateDeckUI();
            return true;
        }
        return false;
    }

    public bool RemoveCardFromDecklist(DefaultCardScriptable oldCard)
    {
        if (oldCard == null) return false;

        if (deckList.ContainsKey(oldCard)) // Schau ob schon Karten dieses Typs im Deck sind
        {
            if (deckList[oldCard] > 1) // Schau ob mehr als eine Karte drinnen ist.
            {
                deckList[oldCard] -= 1; // Ziehe eine Karte ab
                UpdateDeckUI();
                allAvailableCardSlots[oldCard].GetComponent<DeckbuilderCardSlot>().UpdateCardAmountInDeck();
                return true;
            }
            else
            {
                deckList.Remove(oldCard);
                allAvailableCardSlots[oldCard].GetComponent<DeckbuilderCardSlot>().UpdateCardAmountInDeck();
                UpdateDeckUI();
                return true;
            }
        }
        return false;
    }

    void UpdateDeckUI()
    {
        foreach (RectTransform deckCard in deckObject.GetComponentsInChildren<RectTransform>())
        {
            if (deckCard == deckObject) continue;
            Destroy(deckCard.gameObject);
        }

        int myCardCount = 0;
        foreach (KeyValuePair<DefaultCardScriptable, int> keyVal in deckList)
        {
            Transform newDeckElement = Instantiate(deckElementPrefab, deckObject).transform;
            newDeckElement.Find("CardTitle").GetComponent<TextMeshProUGUI>().text = keyVal.Key.cardName;
            newDeckElement.Find("Amount").Find("Text").GetComponent<TextMeshProUGUI>().text = "x " + keyVal.Value.ToString();
            newDeckElement.Find("TypeSprite").GetComponent<Image>().sprite = keyVal.Key.cardTypeSprite;
            newDeckElement.GetComponent<DeckCardButton>().myCard = keyVal.Key;

            myCardCount += keyVal.Value;
        }

        cardCounter.text = myCardCount.ToString() + " / " + minimumCardsInDeck.ToString();
    }

    public int GetCardAmount(DefaultCardScriptable card)
    {
        if (deckList.ContainsKey(card)) return deckList[card];
        else return 0;
    }

    public void OpenClose()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
