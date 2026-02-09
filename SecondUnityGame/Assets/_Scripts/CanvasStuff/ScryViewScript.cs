using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScryViewScript : MonoBehaviour
{
    public List<GameObject> myScryCardsTop, myScryCardsBottom;
    public static ScryViewScript instance;
    [SerializeField] GameObject simpleCardPrefab;
    Transform scryCardTopElement, scryCardBottomElement;
    Button closeScryWindowButton;

    RectTransform scryBoundRect, canvasRect;
    bool setScalingNextFrame;

    private void Awake()
    {
        instance = this;

        scryBoundRect = GetComponent<RectTransform>();
        canvasRect = transform.parent.parent.GetComponent<RectTransform>();
        scryCardTopElement = transform.Find("ScryCardsTop");
        scryCardBottomElement = transform.Find("ScryCardsBottom");
        closeScryWindowButton = transform.Find("DoneButton").GetComponent<Button>();
        closeScryWindowButton.onClick.AddListener(() => CloseScryView());
    }


    private void Update()
    {
        if (setScalingNextFrame)
        {
            setScalingNextFrame = false;
            ScaleUIBasedOnCardCount();
        }
    }

    public void FetchAllCards()
    {
        myScryCardsTop.Clear();
        myScryCardsBottom.Clear();
        for (int i = 0; i < transform.Find("ScryCardsTop").childCount; i++)
        {
            myScryCardsTop.Add(transform.Find("ScryCardsTop").GetChild(i).gameObject);
        }
        for (int i = 0; i < transform.Find("ScryCardsBottom").childCount; i++)
        {
            myScryCardsBottom.Add(transform.Find("ScryCardsBottom").GetChild(i).gameObject);
        }
    }

    public void ScaleUIBasedOnCardCount()
    {
        float halfWidth = MainCanvasSingleton.instance.GetComponent<RectTransform>().rect.width / 2;
        FetchAllCards();

        myScryCardsTop.ForEach(t => t.transform.SetParent(t.transform.position.x<halfWidth ? scryCardTopElement : scryCardBottomElement));
        myScryCardsBottom.ForEach(t => t.transform.SetParent(t.transform.position.x < halfWidth ? scryCardTopElement : scryCardBottomElement));

        FetchAllCards();

        // Korrigiere die Reihenfolge in der Hierarchy basierend auf der Position der Karten
        var sortedHandcardsTop = myScryCardsTop.OrderBy(t => t.transform.position.x).ToList();
        for (int i = 0; i < sortedHandcardsTop.Count; i++) sortedHandcardsTop[i].transform.SetSiblingIndex(i);

        var sortedHandcardsBottom = myScryCardsBottom.OrderBy(t => t.transform.position.x).ToList();
        for (int i = 0; i < sortedHandcardsBottom.Count; i++) sortedHandcardsBottom[i].transform.SetSiblingIndex(i);

        FetchAllCards();

        // Schiebe die Karten an die richtigen Positionen, erst für die Top Karten
        if (myScryCardsTop.Count % 2 == 0)     // Wenn Kartenanzahl gerade
        {
            for (int i = 0; i < myScryCardsTop.Count; i++)
            {
                myScryCardsTop[i].transform.localPosition = new Vector2(((myScryCardsTop.Count / 2 - i) * -100) + 50, 0);
            }
        }
        else                                // Wenn Kartenanzahl ungerade
        {
            for (int i = 0; i < myScryCardsTop.Count; i++)
            {
                myScryCardsTop[i].transform.localPosition = new Vector2((((myScryCardsTop.Count) / 2 - i) * -100), 0);
            }
        }

        // Schiebe die Karten an die richtigen Positionen, dann für die Bottom Karten.
        if (myScryCardsBottom.Count % 2 == 0)     // Wenn Kartenanzahl gerade
        {
            for (int i = 0; i < myScryCardsBottom.Count; i++)
            {
                myScryCardsBottom[i].transform.localPosition = new Vector2(((myScryCardsBottom.Count / 2 - i) * -100) + 50, 0);
            }
        }
        else                                // Wenn Kartenanzahl ungerade
        {
            for (int i = 0; i < myScryCardsBottom.Count; i++)
            {
                myScryCardsBottom[i].transform.localPosition = new Vector2((((myScryCardsBottom.Count) / 2 - i) * -100), 0);
            }
        }

    }

    public void OpenScryView(int maxScryAmount)
    {
        FetchAllCards();
        for (int i = myScryCardsTop.Count - 1; i >= 0; i--) Destroy(myScryCardsTop[i]);
        for (int i = myScryCardsBottom.Count - 1; i >= 0; i--) Destroy(myScryCardsBottom[i]);

        int scryAmount = Mathf.Min(maxScryAmount, CardManager.instance.cardDeck1.Count);
        for (int i = 0; i < scryAmount; i++)
        {
            GameObject newSimpleCard = GameObject.Instantiate(simpleCardPrefab, transform.Find("ScryCardsTop"));
            newSimpleCard.GetComponent<MainCardScript>().myCardToken = CardManager.instance.cardDeck1[i];
            newSimpleCard.GetComponent<MainCardScript>().UpdateCardUI();
        }
        gameObject.SetActive(true);
        ScaleUIBasedOnCardCount();
        setScalingNextFrame = true;
    }

    public void CloseScryView()
    {
        FetchAllCards();

        FinishScrying();

        gameObject.SetActive(false);
        for (int i = myScryCardsTop.Count - 1; i >= 0; i--) Destroy(myScryCardsTop[i]);
        for (int i = myScryCardsBottom.Count - 1; i >= 0; i--) Destroy(myScryCardsBottom[i]);
    }

    public void MoveCard(GameObject card)
    {
        card.transform.position = ClampPositionInScryWindow(Input.mousePosition);
    }

    Vector2 ClampPositionInScryWindow(Vector2 canvasPos)
    {
        Vector3[] corners = new Vector3[4];
        scryBoundRect.GetWorldCorners(corners);

        Vector2 min = canvasRect.InverseTransformPoint(corners[0]) + scryBoundRect.transform.position;
        Vector2 max = canvasRect.InverseTransformPoint(corners[2]) + scryBoundRect.transform.position;

        float x = Mathf.Clamp(canvasPos.x, min.x, max.x);
        float y = Mathf.Clamp(canvasPos.y, min.y, max.y);
        return new Vector2(x, y);
    }

    void FinishScrying()
    {
        FetchAllCards();
        myScryCardsTop.ForEach(t => CardManager.instance.RemoveCardFromDeck(t.GetComponent<MainCardScript>().myCardToken));
        myScryCardsBottom.ForEach(t => CardManager.instance.RemoveCardFromDeck(t.GetComponent<MainCardScript>().myCardToken));

        myScryCardsBottom.ForEach(t => CardManager.instance.AddCardToDeck(t.GetComponent<MainCardScript>().myCardToken));
        myScryCardsTop.ForEach(t => CardManager.instance.AddCardToDeck(t.GetComponent<MainCardScript>().myCardToken, 0));
    }
}
