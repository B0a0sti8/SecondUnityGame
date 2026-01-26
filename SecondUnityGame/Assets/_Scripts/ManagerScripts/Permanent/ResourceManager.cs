using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResourceManager : MonoBehaviour
{
    GameObject healthBar;
    GameObject manaBar;

    int currentPlayerMana, maxPlayerMana, currentPlayerLife, maxPlayerLife;

    public float woodAmount, stoneAmount, foodAmount, reagentsAmount, levelKnowledgeAmount, coinAmount;
    TextMeshProUGUI woodText, stoneText, foodText, reagentsText, knowledgeText, coinText;

    public int permanentKnowledge;

    public static ResourceManager instance;

    Transform myLevelVisuals;
    [SerializeField] GameObject resourcePopUpObject;
    [SerializeField] Sprite woodSprite, stoneSprite, foodSprite, reagentSprite;

    private void Awake()
    {
        instance = this;
        SceneManager.sceneLoaded += InitRefs;
        if (SceneManager.GetActiveScene().name == "WorldMap") gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= InitRefs;
    }

    public void InitRefs(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "WorldMap") gameObject.SetActive(false);
        else
        {
            gameObject.SetActive(true);
            Transform statsAndRes = GameObject.Find("MainCanvas").transform.Find("StatsAndRessources");

            healthBar = statsAndRes.Find("HealthBar").gameObject;
            manaBar = statsAndRes.Find("ManaBar").gameObject;

            woodText = statsAndRes.Find("RessourcePanel").Find("Wood").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            stoneText = statsAndRes.Find("RessourcePanel").Find("Stone").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            foodText = statsAndRes.Find("RessourcePanel").Find("Food").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            reagentsText = statsAndRes.Find("RessourcePanel").Find("Reagents").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            knowledgeText = statsAndRes.Find("RessourcePanel").Find("Knowledge").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            coinText = statsAndRes.Find("RessourcePanel").Find("Coin").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

            myLevelVisuals = GameObject.Find("Level").transform.Find("CombatVisuals").Find("Canvas");

            maxPlayerMana = 100;
            maxPlayerLife = 100;
            currentPlayerLife = maxPlayerLife;
            currentPlayerMana = maxPlayerMana;

            woodAmount = 6;
            stoneAmount = 6;
            foodAmount = 6;
            reagentsAmount = 6;

            TalentTreeManager.instance.Talent4_StartingResources();

            UpdateResourceUI();
        }
    }

    private void Start()
    {
        InitRefs(SceneManager.GetActiveScene(), LoadSceneMode.Additive);
    }

    void UpdateResourceUI()
    {
        woodText.text = woodAmount.ToString();
        stoneText.text = stoneAmount.ToString();
        foodText.text = foodAmount.ToString();
        reagentsText.text = reagentsAmount.ToString();
        knowledgeText.text = levelKnowledgeAmount.ToString();
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

        if (currentPlayerLife <= 0)
        {
            LevelGameManager.instance.FinishLevelDefeat();
        }
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

    public void AddKnowledge(int amount)
    {
        levelKnowledgeAmount += amount;
        UpdateResourceUI();

        if (levelKnowledgeAmount >= LevelGameManager.instance.neededKnowledgeAmount)
        {
            if (!LevelGameManager.instance.isBossLevel) LevelGameManager.instance.FinishLevelSuccess();
        }
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

    public void ManagePermanentKnowledge(int amount)
    {
        permanentKnowledge += amount;
    }
}
