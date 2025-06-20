using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilitySelectionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Color myColor;
    public GameObject myAbilityObject;

    private void Start()
    {
        myColor = GetComponent<Image>().color;
        myColor.a = 0.6f;
        GetComponent<Image>().color = myColor;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        myColor.a = 0.9f;
        GetComponent<Image>().color = myColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myColor.a = 0.6f;
        GetComponent<Image>().color = myColor;
    }


}
