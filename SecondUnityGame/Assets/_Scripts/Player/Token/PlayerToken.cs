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

    // Eigenschafte und Werte für Kampf
    public int maxEnergy;
    public int attackRange;

    private void Start()
    {
        healthBar = transform.Find("Canvas").Find("Healthbar").Find("Health").gameObject;
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
    }

    public void SetToken(PlayerTokenScriptable newToken)
    {
        TurnAndEnemyManager.instance.allPlayerSlotsWithTokens.Add(gameObject);

        myToken = newToken;
        UpdatePlayerToken();

        currentLife = maxLife;
    }

    public override void UpdateHealthbar()
    {
        if (myToken.cardTypeString == "Building") return;
        base.UpdateHealthbar();
    }

    void UpdatePlayerToken()
    {
        transform.Find("Picture").GetComponent<SpriteRenderer>().sprite = myToken.tokenSprite;
        maxLife = myToken.maxLife;
        maxEnergy = myToken.maxEnergy;
        attackValue = myToken.attackValue;
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
