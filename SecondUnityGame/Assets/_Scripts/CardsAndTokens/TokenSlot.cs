using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TokenSlot : MonoBehaviour
{
    public bool hasToken;
    GameObject mouseOverMarker;
    GameObject myTokenSprite;
    CardPrefabScriptable myCardToken;

    //protected enum AreaType
    //{
    //    water,
    //    grassLight,
    //    grassDark,
    //    sand,
    //    forest
    //}

    //protected AreaType myAreaType = AreaType.water;

    void Awake()
    {
        hasToken = false;
        mouseOverMarker = transform.Find("MouseOverMarker").gameObject;
        myTokenSprite = transform.Find("TokenSprite").gameObject;

        mouseOverMarker.SetActive(false);
        myTokenSprite.SetActive(false);
    }

    private void OnMouseEnter()
    {
        mouseOverMarker.SetActive(true);
    }

    private void OnMouseExit()
    {
        mouseOverMarker.SetActive(false);
    }

    public void SetToken(GameObject myNewCardToken)
    {
        myCardToken = myNewCardToken.GetComponent<MainCardScript>().myCardScriptable;
        myTokenSprite.GetComponent<SpriteRenderer>().sprite = myCardToken.tokenSprite;
        myTokenSprite.SetActive(true);

        MouseClickAndGrabManager.instance.RemoveGrabbedItem();
        hasToken = true;
    }

    public void RemoveToken()
    {

    }

    private void OnMouseUpAsButton()
    {
        Debug.Log("Schnitzel");

        if (myCardToken != null)
        {
            MouseClickAndGrabManager.instance.TokenClicked(this.gameObject);
        }
    }

}