using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TokenSlot : MonoBehaviour
{
    public bool hasToken;
    GameObject mouseOverMarker, movementMarker;
    GameObject myTokenSprite;

    Image healthBar, energyBar;
    GameObject uICanvas;


    public CardPrefabScriptable myCardToken;
    float energyRegenerationElapsed=0;

    RessourceManager.ressourceType myResType;

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

        uICanvas = transform.Find("TokenSlotWorldCanvas").gameObject;

        healthBar = uICanvas.transform.Find("HealthBar").Find("Health").GetComponent<Image>();
        energyBar = uICanvas.transform.Find("EnergyBar").Find("Energy").GetComponent<Image>();

        mouseOverMarker.SetActive(false);
        myTokenSprite.SetActive(false);
        uICanvas.SetActive(false);
    }

    private void Update()
    {
        EnergyRegeneration();
        TryGatherRessources();
    }

    void EnergyRegeneration()
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

    public void SetRessource(RessourceManager.ressourceType newResType)
    {
        myResType = newResType;
    }

    void TryGatherRessources()
    {
        if (currentEnergy < 10) return;
        if (!(hasToken && myCardToken.isEnemy == false && (myCardToken.unitTypeString == "Military Unit" || myCardToken.unitTypeString == "Building Unit"))) return;
        if (myResType == RessourceManager.ressourceType.none) return;

        int myAmount = 1;             // könnte später durch Modifikatoren geändert werden
        if (myCardToken.unitTypeString == "Building Unit") myAmount = 2; // manche Einheiten sammeln doppelt so schnell

        // Könnte man auch deutlich leichter machen ( einfach resType gathern...). So hat man später aber mehr Freiheiten. Z.b. chance auf reagenzien, wenn man Kräuter farmt.
        switch (myResType)
        {
            case RessourceManager.ressourceType.wood:
                RessourceManager.instance.GatherRessource(myResType, myAmount); 
                break;
            case RessourceManager.ressourceType.stone:
                RessourceManager.instance.GatherRessource(myResType, myAmount);
                break;
            case RessourceManager.ressourceType.food:
                RessourceManager.instance.GatherRessource(myResType, myAmount);
                break;
            case RessourceManager.ressourceType.reagents:
                RessourceManager.instance.GatherRessource(myResType, myAmount);
                break;
            case RessourceManager.ressourceType.knowledge:
                RessourceManager.instance.GatherRessource(myResType, myAmount);
                break;
            case RessourceManager.ressourceType.coin:
                RessourceManager.instance.GatherRessource(myResType, myAmount);
                break;
            default:
                break;
        }

        ModifyTokenEnergy(-5, EnergyModificationSource.Gathering);
    }

    public void ModifyTokenEnergy(int amount, EnergyModificationSource source)
    {
        currentEnergy += amount;
        //Debug.Log("Removing " + amount  +" Energy: ");

        if (currentEnergy > myCardToken.maxEnergy) currentEnergy = myCardToken.maxEnergy;
        if (currentEnergy < 0) currentEnergy = 0;

        UpdateLifeAndEnergyBar();
    }

    public void ModifyTokenLife(int amount)
    {
        currentLife += amount;

        if (currentLife > myCardToken.maxLife) currentLife = myCardToken.maxLife;
        if (currentLife < 0) currentLife = 0;

        UpdateLifeAndEnergyBar();
    }

    void UpdateLifeAndEnergyBar()
    {
        healthBar.fillAmount = (float)currentLife / (float)myCardToken.maxLife;
        energyBar.fillAmount = (float)currentEnergy / (float)myCardToken.maxEnergy;
    }

    public void SetToken(CardPrefabScriptable myNewCardToken, bool isPlayedAsCard = false, int remainingEnergy = 1)
    {
        myCardToken = myNewCardToken;
        myTokenSprite.GetComponent<SpriteRenderer>().sprite = myCardToken.tokenSprite;
        myTokenSprite.SetActive(true);

        if (isPlayedAsCard)
        {
            currentEnergy = myCardToken.maxEnergy;
            currentLife = myCardToken.maxLife;
            MouseClickAndGrabManager.instance.RemoveGrabbedItem();
        }
        else
        {
            ModifyTokenEnergy(remainingEnergy, EnergyModificationSource.Moving);
        }
        hasToken = true;
        uICanvas.SetActive(true);
        UpdateLifeAndEnergyBar();

        if (myNewCardToken.isEnemy) GridAndMovementManager.instance.enemyAndAllyMap[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y)] = 2;
        else GridAndMovementManager.instance.enemyAndAllyMap[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y)] = 1;
    }

    public void RemoveToken()
    {
        hasToken = false;
        myCardToken = null;
        myTokenSprite.GetComponent<SpriteRenderer>().sprite = null;
        myTokenSprite.SetActive(false);
        uICanvas.SetActive(false);
        GridAndMovementManager.instance.enemyAndAllyMap[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y)] = 0;
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