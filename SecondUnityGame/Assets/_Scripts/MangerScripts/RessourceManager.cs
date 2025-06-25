using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RessourceManager : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject manaBar;

    int currentPlayerMana, maxPlayerMana, currentPlayerLife, maxPlayerLife;

    public float woodAmount, stoneAmount, foodAmount, reagentsAmount, knowledgeAmount, coinAmount;
    [SerializeField] TextMeshProUGUI woodText, stoneText, foodText, reagentsText, knowledgeText, coinText;

    public static RessourceManager instance;

    Transform myLevelVisuals;
    [SerializeField] GameObject resourcePopUpObject;
    [SerializeField] Sprite woodSprite, stoneSprite, foodSprite, reagentSprite;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        myLevelVisuals = GameObject.Find("Level").transform.Find("CombatVisuals").Find("Canvas");

        maxPlayerMana = 100;
        maxPlayerLife = 100;
        currentPlayerLife = maxPlayerLife;
        currentPlayerMana = maxPlayerMana;

        woodAmount = 20;
        stoneAmount = 20;

        UpdateResourceUI();
    }

    void UpdateResourceUI()
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
        //Debug.Log("PlayerTakingDamage");
        currentPlayerLife -= damage;

        UpdateHealthBar();
    }

    public bool HasResources(float woodCost = 0, float stoneCost = 0, float foodCost = 0, float reagentCost = 0)
    {
        if (woodCost > woodAmount) return false;
        if (stoneCost > stoneAmount) return false;
        if (foodCost > foodAmount) return false;
        if (reagentCost > reagentsAmount) return false;

        return true;
    }

    public void AddOrRemoveResources(int woodCost = 0, int stoneCost = 0, int foodCost = 0, int reagentCost = 0, GameObject source = null)
    {
        woodAmount += woodCost;
        stoneAmount += stoneCost;
        foodAmount += foodCost;
        reagentsAmount += reagentCost;

        if (source != null)
        {
            GameObject myRes = Instantiate(resourcePopUpObject, myLevelVisuals);
            Vector3 randPos = new Vector3(Random.Range(0, 2f), Random.Range(0, 2f), 0);
            myRes.transform.position = source.transform.position + new Vector3(0, 3, 0) + randPos;
            myRes.transform.Find("ResourceSprite").GetComponent<Image>().sprite = woodSprite;
            
            if (woodCost >= 0) 
            {
                myRes.GetComponent<FadeOverTime>().myTextColor = Color.green;
                myRes.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = "+ " + woodCost.ToString();
            } 
            else
            {
                myRes.GetComponent<FadeOverTime>().myTextColor = Color.red;
                myRes.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = "+ " + woodCost.ToString();
            }
        }
        UpdateResourceUI();
    }
}
