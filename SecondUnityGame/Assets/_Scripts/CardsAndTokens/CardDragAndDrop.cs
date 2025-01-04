using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    RectTransform myTransform;
    Canvas myCardCanvas;
    CanvasGroup myCanvasGroup;

    public Vector2 myStartPosition;

    private void Awake()
    {
        myTransform = transform.GetComponent<RectTransform>();
        myCardCanvas = GameObject.Find("CardCanvas").GetComponent<Canvas>();
        myCanvasGroup = GetComponent<CanvasGroup>();

        myStartPosition = new Vector2(0, 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        myCanvasGroup.alpha = 0.3f;
        myCanvasGroup.blocksRaycasts = false;
        MouseClickAndGrabManager.instance.myGrabbedItem = this.gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        myTransform.anchoredPosition += eventData.delta / myCardCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        myCanvasGroup.alpha = 1f;
        myCanvasGroup.blocksRaycasts = true;

        transform.position = myStartPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Mouse über Karte gedrückt");
        myStartPosition = transform.position;
    }

    private void OnDestroy()
    {
        HandCardScript.instance.RemoveCard(this.gameObject);
    }

}
