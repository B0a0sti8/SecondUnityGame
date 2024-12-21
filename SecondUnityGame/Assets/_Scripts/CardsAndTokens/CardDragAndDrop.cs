using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    RectTransform myTransform;
    Canvas myCardCanvas;

    private void Awake()
    {
        myTransform = transform.GetComponent<RectTransform>();
        myCardCanvas = GameObject.Find("CardCanvas").GetComponent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        myTransform.anchoredPosition += eventData.delta / myCardCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Mouse über Karte gedrückt");
    }
}
