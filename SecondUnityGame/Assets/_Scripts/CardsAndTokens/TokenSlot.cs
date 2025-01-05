using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TokenSlot : MonoBehaviour
{
    public bool hasToken;
    GameObject mouseOverMarker, movementMarker;
    GameObject myTokenSprite;
    public CardPrefabScriptable myCardToken;
    float energyRegenerationElapsed=0;

    public enum EnergyModificationSource
    {
        Moving, 
        Fighting,
        Gathering,
        Regeneration
    }

    void Awake()
    {
        hasToken = false;
        mouseOverMarker = transform.Find("MouseOverMarker").gameObject;
        movementMarker = transform.Find("MovementMarker").gameObject;
        myTokenSprite = transform.Find("TokenSprite").gameObject;

        mouseOverMarker.SetActive(false);
        myTokenSprite.SetActive(false);
    }

    private void Update()
    {
        if (hasToken)
        {
            energyRegenerationElapsed += Time.deltaTime;
            if (energyRegenerationElapsed >= 1)
            {
                energyRegenerationElapsed = 0;
                ModifyTokenEnergy(1, EnergyModificationSource.Regeneration);
            }
        }
    }

    public void ModifyTokenEnergy(int amount, EnergyModificationSource source)
    {
        myCardToken.currentEnergy += amount;
        Debug.Log("Removing " + amount  +" Energy: ");
        Debug.Log("Remaining Energy: " + myCardToken.currentEnergy);

        if (myCardToken.currentEnergy > myCardToken.maxEnergy) myCardToken.currentEnergy = myCardToken.maxEnergy;
        if (myCardToken.currentEnergy < 0) myCardToken.currentEnergy = 0;
    }

    public void SetToken(CardPrefabScriptable myNewCardToken, bool isPlayedAsCard = false)
    {
        myCardToken = myNewCardToken;
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
    private void OnMouseEnter()
    {
        mouseOverMarker.SetActive(true);
    }

    private void OnMouseExit()
    {
        mouseOverMarker.SetActive(false);
    }
}