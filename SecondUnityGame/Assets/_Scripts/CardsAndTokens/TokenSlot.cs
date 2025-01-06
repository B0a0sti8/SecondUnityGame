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

    public int currentEnergy;
    public int currentLife;

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
        currentEnergy += amount;
        //Debug.Log("Removing " + amount  +" Energy: ");
        //Debug.Log("Remaining Energy: " + currentEnergy);

        if (currentEnergy > myCardToken.maxEnergy) currentEnergy = myCardToken.maxEnergy;
        if (currentEnergy < 0) currentEnergy = 0;
    }

    public void SetToken(CardPrefabScriptable myNewCardToken, bool isPlayedAsCard = false)
    {
        myCardToken = myNewCardToken;
        myTokenSprite.GetComponent<SpriteRenderer>().sprite = myCardToken.tokenSprite;
        myTokenSprite.SetActive(true);

        if (isPlayedAsCard)
        {
            currentEnergy = myCardToken.maxEnergy;
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
        if (hasToken)
        {
            CardManager.instance.previewCard.GetComponent<MainCardScript>().myCardScriptable = this.myCardToken;
            CardManager.instance.previewCard.SetActive(true);
            CardManager.instance.previewCard.GetComponent<MainCardScript>().FetchFields();
            CardManager.instance.previewCard.GetComponent<MainCardScript>().UpdateCardUI(currentLife, currentEnergy);
        }
    }

    private void OnMouseExit()
    {
        mouseOverMarker.SetActive(false);
        if (CardManager.instance.previewCard.activeSelf) CardManager.instance.previewCard.SetActive(false);
    }
}