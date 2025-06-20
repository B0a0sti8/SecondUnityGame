using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerToken : MonoBehaviour
{
    private PlayerTokenScriptable myToken;
    [SerializeField] private Transform listOfAllAbilities;

    // Eigenschafte und Werte für Kampf
    public int maxEnergy;
    public int maxLife;
    public int currentLife;
    public int attackValue;
    public int attackRange;

    public GameObject healthBar;

    private void Start()
    {
        currentLife = maxLife;

        healthBar = transform.Find("Canvas").Find("Healthbar").Find("Health").gameObject;
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
    }

    void UpdatePlayerToken()
    {
        transform.Find("Picture").GetComponent<SpriteRenderer>().sprite = myToken.tokenSprite;
        maxLife = myToken.maxLife;
        maxEnergy = myToken.maxEnergy;
        attackValue = myToken.attackValue;
        attackRange = myToken.attackRange;


    }


    public void TakeDamageOrHealing(int damageAmount)
    {
        Debug.Log("Player Token is taking Damage: " + damageAmount);
        currentLife -= damageAmount;

        Debug.Log("Current Life: " + currentLife + " maximum Life: " + maxLife);

        UpdateHealthbar();

        if (currentLife <= 0) Die();
    }

    public void UpdateHealthbar()
    {
        healthBar.GetComponent<Image>().fillAmount = (float)currentLife / (float)maxLife;
    }


    public void Die()
    {
        StartCoroutine((RemoveEnemy(0.2f)));
    }

    IEnumerator RemoveEnemy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
