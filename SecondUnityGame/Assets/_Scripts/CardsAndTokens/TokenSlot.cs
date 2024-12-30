using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TokenSlot : MonoBehaviour
{
    public bool hasToken;
    GameObject mouseOverMarker;
    GameObject myToken;

    protected enum AreaType
    {
        water,
        grassLight,
        grassDark,
        sand,
        forest
    }

    protected AreaType myAreaType = AreaType.water;

    void Awake()
    {
        hasToken = false;
        mouseOverMarker = transform.Find("MouseOverMarker").gameObject;
        myToken = transform.Find("Token").gameObject;

        mouseOverMarker.SetActive(false);
        myToken.SetActive(false);
    }

    private void OnMouseEnter()
    {
        mouseOverMarker.SetActive(true);
    }

    private void OnMouseExit()
    {
        mouseOverMarker.SetActive(false);
    }

    public void SetToken(GameObject myNewToken)
    {
        //if (MouseGrabManager.instance.myGrabbedItem == null) return;
        //Transform myCard = MouseGrabManager.instance.myGrabbedItem.transform;
        //if (myCard == null) return;
        //if (hasToken) return;

        myToken.GetComponent<SpriteRenderer>().sprite = myNewToken.GetComponent<MainCardScript>().tokenPicture;
        myToken.SetActive(true);

        MouseGrabManager.instance.RemoveGrabbedItem();
        hasToken = true;
    }
}