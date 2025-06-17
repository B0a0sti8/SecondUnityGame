using UnityEngine;
using UnityEngine.UI;

public class PlayerToken : MonoBehaviour
{
    private PlayerTokenScriptable myToken;

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


    public void TakeDamage(int damageAmount)
    {
        Debug.Log("Player Token is taking Damage: " + damageAmount);
        currentLife -= damageAmount;

        Debug.Log("Current Life: " + currentLife + " maximum Life: " + maxLife);

        UpdateHealthbar();
    }

    public void UpdateHealthbar()
    {
        healthBar.GetComponent<Image>().fillAmount = (float)currentLife / (float)maxLife;
    }
}
