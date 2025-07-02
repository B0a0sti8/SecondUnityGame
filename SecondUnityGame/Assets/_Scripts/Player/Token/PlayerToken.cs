using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerToken : DefaultToken
{
    public PlayerTokenScriptable myToken;
    [SerializeField] private Transform listOfAllAbilities;
    public List<Transform> myPassiveTriggerAbilities = new List<Transform>();
    public List<Transform> myAbilities = new List<Transform>();

    GameObject energyBar;

    // Eigenschafte und Werte für Kampf
    public int attackRange;

    public override void Start()
    {
        base.Start();

        healthBar = transform.Find("Canvas").Find("Healthbar").Find("Health").gameObject;
        energyBar = transform.Find("Canvas").Find("EnergyBar").Find("Energy").gameObject;
        transform.Find("Canvas").Find("EnergyBar").gameObject.SetActive(false);
        if (myToken.cardTypeString == "Building") healthBar.transform.parent.gameObject.SetActive(false);

        listOfAllAbilities = GameObject.Find("Systems").transform.Find("ListOfAllPlayerTokenAbilities");

        foreach (string ab in myToken.tokenAbilities)
        {
            GameObject ability = Instantiate(listOfAllAbilities.Find(ab).gameObject, transform.Find("Abilities"));
            ability.name = ab;
            myAbilities.Add(ability.transform);
        }

        foreach (string ab in myToken.tokenPassiveTriggerAbilities)
        {
            GameObject ability = Instantiate(listOfAllAbilities.Find(ab).gameObject, transform.Find("Abilities"));
            ability.name = ab;
            myPassiveTriggerAbilities.Add(ability.transform);
        }

        tokenName = myToken.cardName;
        tokenDescription = myToken.description;
    }

    public void SetToken(PlayerTokenScriptable newToken)
    {
        TurnAndEnemyManager.instance.allPlayerSlotsWithTokens.Add(gameObject);

        myToken = newToken;
        UpdatePlayerToken();

        currentLife = maxLife.GetValue();
        currentEnergy = (int)Mathf.Round(maxEnergy.GetValue());
    }

    public override void UpdateHealthbar()
    {
        if (myToken.cardTypeString == "Building") return;
        base.UpdateHealthbar();
    }

    public void UpdateEnergybar()
    {
        energyBar.GetComponent<Image>().fillAmount = (float)currentEnergy / maxEnergy.GetValue();
    }

    public void ManageEnergy(int energy)
    {
        currentEnergy -= energy;

        Debug.Log("Paying " + energy + ". Remaining: " + currentEnergy);
        UpdateEnergybar();
    }

    void UpdatePlayerToken()
    {
        transform.Find("Picture").GetComponent<SpriteRenderer>().sprite = myToken.tokenSprite;
        maxLife.baseValue = myToken.maxLife;
        maxEnergy.baseValue = myToken.maxEnergy;
        dmgHealVal.baseValue = myToken.baseDmgHealValue;
        receiveDmgHealVal.baseValue = myToken.baseRecDmgHealValue;
        attackRange = myToken.attackRange;
    }

    public void TriggerPassiveAbilities()
    {
        for (int i = 0; i < myPassiveTriggerAbilities.Count; i++)
        {
            StartCoroutine(TriggerSinglePassive(i * 0.2f, myPassiveTriggerAbilities[i]));
        }
    }

    IEnumerator TriggerSinglePassive(float waitingTime, Transform ability)
    {
        yield return new WaitForSeconds(waitingTime);
        ability.GetComponent<PlayerTokenAbilityPrefab>().ApplyPassiveTriggerEffect();
    }
}
