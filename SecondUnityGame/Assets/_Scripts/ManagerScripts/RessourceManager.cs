using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

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
        float myTime = 0;

        woodAmount += woodCost;
        stoneAmount += stoneCost;
        foodAmount += foodCost;
        reagentsAmount += reagentCost;

        if (source != null)
        {
            if (woodCost != 0) { StartCoroutine(SpawnResourceToolTip(myTime, source, woodCost, woodSprite)); myTime += 0.35f; }
            if (stoneCost != 0) { StartCoroutine(SpawnResourceToolTip(myTime, source, stoneCost, stoneSprite)); myTime += 0.35f; }
            if (foodCost != 0) { StartCoroutine(SpawnResourceToolTip(myTime, source, foodCost, foodSprite)); myTime += 0.35f; }
            if (reagentCost != 0) { StartCoroutine(SpawnResourceToolTip(myTime, source, reagentCost, reagentSprite)); myTime += 0.35f; }
        }
        UpdateResourceUI();
    }


    IEnumerator SpawnResourceToolTip(float waitTime, GameObject source, float amount, Sprite resSpr)
    {
        yield return new WaitForSeconds(waitTime);

        GameObject myRes = Instantiate(resourcePopUpObject, myLevelVisuals);
        Vector3 randPos = new Vector3(Random.Range(0, 2f), Random.Range(0, 2f), 0);
        myRes.transform.position = source.transform.position + new Vector3(0, 3, 0) + randPos;
        myRes.transform.Find("ResourceSprite").GetComponent<Image>().sprite = resSpr;

        if (amount >= 0)
        {
            myRes.GetComponent<FadeOverTime>().myTextColor = Color.green;
            myRes.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = "+ " + amount.ToString();
        }
        else
        {
            myRes.GetComponent<FadeOverTime>().myTextColor = Color.red;
            myRes.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = "+ " + amount.ToString();
        }
    }
}
