using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerToken : DefaultToken
{
    public PlayerTokenScriptable myToken;
    [SerializeField] private Transform listOfAllAbilities;

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
        }

    }

    public void SetToken(PlayerTokenScriptable newToken)
    {
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
}
