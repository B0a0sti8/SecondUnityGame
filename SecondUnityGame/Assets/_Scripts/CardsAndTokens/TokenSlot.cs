using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TokenSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    bool hasToken;

    public void OnDrop(PointerEventData eventData)
    {
        Transform myCard = MouseGrabManager.instance.myGrabbedItem.transform;
        if (myCard == null) return;
        if (hasToken) return;


        GetComponent<Image>().sprite = myCard.Find("Content").Find("Picture").GetComponent<Image>().sprite;
        MouseGrabManager.instance.myGrabbedItem = null;
        GameObject.Destroy(myCard.gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color mycolor = GetComponent<Image>().color;
        Color newColor = mycolor;
        newColor.a = 1f;
        GetComponent<Image>().color = newColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color mycolor = GetComponent<Image>().color;
        Color newColor = mycolor;
        newColor.a = 0.2f;
        GetComponent<Image>().color = newColor;
    }

    void Awake()
    {
        Color mycolor = GetComponent<Image>().color;
        Color newColor = mycolor;
        newColor.a = 0.2f;
        GetComponent<Image>().color = newColor;

        hasToken = false;
    }
}
