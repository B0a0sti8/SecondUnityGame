using UnityEngine;

public class DeckbuilderCardSlot : MonoBehaviour
{
    DefaultCardScriptable myCard;
    [SerializeField] GameObject amountMarkerPrefab;

    private void Start()
    {
        myCard = transform.Find("SimpleCard").GetComponent<MainCardScript>().myCardToken;
        UpdateMarkerAmount();
        UpdateCardAmountInDeck();
    }

    public void AddCardToDeck()
    {
        DeckbuildingManager.instance.AddCardToDecklist(myCard);
        UpdateCardAmountInDeck();
    }

    public void UpdateCardAmountInDeck()
    {
        myCard = transform.Find("SimpleCard").GetComponent<MainCardScript>().myCardToken;
        for (int i = 0; i < transform.Find("CardAmount").childCount; i++)
        {
            transform.Find("CardAmount").GetChild(i).Find("Check").gameObject.SetActive(false);
        }

        int myAmount = DeckbuildingManager.instance.GetCardAmount(myCard);

        for (int i = 0; i < myAmount; i++)
        {
            transform.Find("CardAmount").GetChild(i).Find("Check").gameObject.SetActive(true);
        }
    }

    void UpdateMarkerAmount()
    {
        foreach (RectTransform amountMarker in transform.Find("CardAmount").GetComponentsInChildren<RectTransform>())
        {
            if (amountMarker == transform.Find("CardAmount")) continue;

            Destroy(amountMarker.gameObject);
        }

        for (int i = 0; i < myCard.maxCardAmount; i++)
        {
            Instantiate(amountMarkerPrefab, transform.Find("CardAmount"));
        }
    }
}
