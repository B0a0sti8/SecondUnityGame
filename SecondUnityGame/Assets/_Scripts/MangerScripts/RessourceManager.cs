using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RessourceManager : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject manaBar;

    int currentPlayerMana, maxPlayerMana, currentPlayerLife, maxPlayerLife;

    public int woodAmount, stoneAmount, foodAmount, reagentsAmount, knowledgeAmount, coinAmount;
    [SerializeField] TextMeshProUGUI woodText, stoneText, foodText, reagentsText, knowledgeText, coinText;

    public static RessourceManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        maxPlayerMana = 100;
        maxPlayerLife = 100;
        currentPlayerLife = maxPlayerLife;
        currentPlayerMana = maxPlayerMana;
    }

    void UpdateRessourceUI()
    {
        woodText.text = woodAmount.ToString();
        stoneText.text = stoneAmount.ToString();
        foodText.text = foodAmount.ToString();
        reagentsText.text = reagentsAmount.ToString();
        knowledgeText.text = knowledgeAmount.ToString();
        coinText.text = coinAmount.ToString();
    }

    void UpdateManaBar()
    {
        manaBar.transform.Find("Mana").GetComponent<Image>().fillAmount = (float)currentPlayerMana / (float)maxPlayerMana;
        manaBar.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = currentPlayerMana + " / " + maxPlayerMana;
    }

    void UpdateHealthBar()
    {
        healthBar.transform.Find("Health").GetComponent<Image>().fillAmount = (float)currentPlayerLife / (float)maxPlayerLife;
        healthBar.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = currentPlayerLife + " / " + maxPlayerLife;
    }

    public void TakeDamageOrHealing_Player(int damage)
    {
        Debug.Log("PlayerTakingDamage");
        currentPlayerLife -= damage;

        UpdateHealthBar();
    }
}
