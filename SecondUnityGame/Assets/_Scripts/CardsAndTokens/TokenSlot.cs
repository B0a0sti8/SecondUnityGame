using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TokenSlot : MonoBehaviour
{
    public bool hasToken;
    GameObject mouseOverMarker, movementMarker;
    GameObject myTokenSprite;
    public CardPrefabScriptable myCardToken;

    void Awake()
    {
        hasToken = false;
        mouseOverMarker = transform.Find("MouseOverMarker").gameObject;
        movementMarker = transform.Find("MovementMarker").gameObject;
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

    public void SetToken(CardPrefabScriptable myNewCardToken, bool isPlayedAsCard = false)
    {
        myCardToken = myNewCardToken;
        Debug.Log(myCardToken);
        myTokenSprite.GetComponent<SpriteRenderer>().sprite = myCardToken.tokenSprite;
        myTokenSprite.SetActive(true);

        if (isPlayedAsCard)
        {
            myCardToken.currentEnergy = myCardToken.maxEnergy;
            MouseClickAndGrabManager.instance.RemoveGrabbedItem();
        }
        hasToken = true;
    }


    public void RemoveToken()
    {
        hasToken = false;
        myCardToken = null;
        myTokenSprite.GetComponent<SpriteRenderer>().sprite = null;
        myTokenSprite.SetActive(false);
    }

    private void OnMouseUpAsButton()
    {
        if (myCardToken != null)
        {
            MouseClickAndGrabManager.instance.TokenClicked(this.gameObject);
        }
    }

}