using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;

public class TurnAndEnemyManager : MonoBehaviour
{
    public bool isPlayerTurn = true;
    [SerializeField] TextMeshProUGUI turnIndicatorText;
    float enemyTurnTimer_Debug = 1f;

    public event EventHandler OnPlayerTurnStart;

    public bool isLevelFinished = false;

    float timeBetweenEnemies;
    float timeBetweenEnemiesElapsed;
    int enemyActionCounter;
    int enemySpawnCounter;
    int enemySpawnAmountPerTurn;

    public List<GameObject> allEnemySlotsWithTokens;
    public List<GameObject> allPlayerSlotsWithTokens;
    [SerializeField] List<GameObject> allEnemiesInLevel = new List<GameObject>();

    [SerializeField] GameObject levelObject;
    public List<GameObject> allEnemySlots = new List<GameObject>();

    public static TurnAndEnemyManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void DebuggingEventTrig(object sender, EventArgs e)
    {
        Debug.Log("Event was triggered");
    }

    private void Start()
    {
        OnPlayerTurnStart += DebuggingEventTrig;
        if (SceneManager.GetActiveScene().name == "WorldMap") gameObject.SetActive(false);
        else gameObject.SetActive(true);
        SceneManager.sceneLoaded += InitRefs;
        InitRefs(SceneManager.GetActiveScene(), LoadSceneMode.Additive);

        timeBetweenEnemies = 0.5f;
        timeBetweenEnemiesElapsed = 0f;
        enemyActionCounter = 0;
        enemySpawnCounter = 0;
    }

    private void InitRefs(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (SceneManager.GetActiveScene().name == "WorldMap") gameObject.SetActive(false);
        else
        {
            gameObject.SetActive(true);
            allEnemySlots.Clear();
            levelObject = GameObject.Find("Level");

            if (levelObject != null)
            {
                for (int i = 0; i < levelObject.transform.Find("EnemySlots").childCount; i++)
                {
                    allEnemySlots.Add(levelObject.transform.Find("EnemySlots").GetChild(i).gameObject);
                }
            }

            Debug.Log(LevelGameManager.instance);
            enemySpawnAmountPerTurn = LevelGameManager.instance.enemiesPerTurn;
            allEnemiesInLevel = LevelGameManager.instance.allEnemiesInLevel;
            allEnemySlotsWithTokens = new List<GameObject>();

            Button endPlayerTurnButton = MainCanvasSingleton.instance.transform.Find("Buttons").Find("EndPlayerTurnButton").GetComponent<Button>();
            endPlayerTurnButton?.onClick.AddListener(() => EndPlayerTurn());

            turnIndicatorText = MainCanvasSingleton.instance.transform.Find("StatsAndRessources").Find("TurnIndicator").GetComponent<TextMeshProUGUI>();

            StartPlayerTurn();
        } 
    }

    public void AtTheEndOfLevel()
    {
        isPlayerTurn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLevelFinished) return;

        if (!isPlayerTurn)
        {
            // Bei Rundenbeginn, warte etwas
            if (enemyTurnTimer_Debug > 0) 
            {
                enemyTurnTimer_Debug -= Time.deltaTime;
                return;
            } 

            // Jeder Tokenslot, der einen Token hat, spielt. Mit etwas Zeit dazwischen.
            if (enemyActionCounter < allEnemySlotsWithTokens.Count)
            {
                if (timeBetweenEnemiesElapsed < timeBetweenEnemies) timeBetweenEnemiesElapsed += Time.deltaTime;
                else
                {
                    PlayEnemyTurn(allEnemySlotsWithTokens[enemyActionCounter]);
                    enemyActionCounter++;
                    timeBetweenEnemiesElapsed = 0f;
                }

                return;
            }

            // Spawne neue Gegner
            if (enemySpawnCounter < enemySpawnAmountPerTurn)
            {
                if (timeBetweenEnemiesElapsed < timeBetweenEnemies) timeBetweenEnemiesElapsed += Time.deltaTime;
                else
                {
                    SpawnRandomEnemyInRandomSlot();
                    enemySpawnCounter++;
                    timeBetweenEnemiesElapsed = 0f;
                }

                return;
            }

            // Beende gegnerische Runde
            EndEnemyTurn();
        }
    }

    public void EndPlayerTurn()
    {
        if (!isPlayerTurn) return;

        isPlayerTurn = false;
        turnIndicatorText.text = "Enemy Turn";
        Debug.Log("Ended Player Turn");

        // Update die Buffs für alle Player Tokens
        for (int i = 0; i < allPlayerSlotsWithTokens.Count; i++)
        {
            if (allPlayerSlotsWithTokens[i].GetComponentInChildren<DefaultToken>() != null)
            {
                allPlayerSlotsWithTokens[i].GetComponentInChildren<DefaultToken>().UpdateBuffs();
            }
        }

        FindEnemyTokens();
        StartEnemyTurn();
    }

    public void EndEnemyTurn()
    {
        FindEnemyTokens();

        // Update die Buffs für alle Enemy Tokens
        for (int i = 0; i < allEnemySlotsWithTokens.Count; i++)
        {
            if (allEnemySlotsWithTokens[i].GetComponentInChildren<DefaultToken>() != null)
            {
                allEnemySlotsWithTokens[i].GetComponentInChildren<DefaultToken>().UpdateBuffs();
            }
        }

        CheckEnemyTargets();

        allEnemySlotsWithTokens.Clear();
        enemyTurnTimer_Debug = 1f;
        enemyActionCounter = 0;
        enemySpawnCounter = 0;

        isPlayerTurn = true;

        turnIndicatorText.text = "Player Turn";
        Debug.Log("Ended Enemy Turn");

        StartPlayerTurn();
    }

    void StartEnemyTurn()
    {

    }

    void StartPlayerTurn()
    {
        OnPlayerTurnStart?.Invoke(this, EventArgs.Empty);

        CardManager.instance.DiscardHand();
        StartCoroutine(DrawCardsWithDelay(1f));


        foreach (GameObject playerToken in allPlayerSlotsWithTokens)
        {
            playerToken.GetComponent<PlayerToken>().TriggerPassiveAbilities();
        }
    }

    IEnumerator DrawCardsWithDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        CardManager.instance.DrawMultipleCards(5);
    }

    void PlayEnemyTurn(GameObject enemyTokenSlot)
    {
        EnemyToken enemyToken = enemyTokenSlot.GetComponentInChildren<EnemyToken>();
        enemyToken.EvaluateBuffsAndDebuffs();
        enemyToken.ChooseTargetsAndUseSkills();
    }

    public void FindEnemyTokens()
    {
        allEnemySlotsWithTokens.Clear();
        for (int i = 0; i < allEnemySlots.Count; i++)
        {
            if (allEnemySlots[i].GetComponentInChildren<EnemyToken>() != null)
            {
                allEnemySlotsWithTokens.Add(allEnemySlots[i]);
            }
        }
    }

    GameObject FindEmptyEnemySlot()
    {
        List<GameObject> emptySlots = new List<GameObject>();
        emptySlots.Clear();

        for (int i = 0; i < allEnemySlots.Count; i++)
        {
            if (allEnemySlots[i].GetComponentInChildren<DefaultToken>() == null) emptySlots.Add(allEnemySlots[i]);
        }

        if (emptySlots.Count == 0) return null;
        else return emptySlots[UnityEngine.Random.Range(0, emptySlots.Count)];
    }

    void SpawnRandomEnemyInRandomSlot()
    {
        GameObject mySlot = FindEmptyEnemySlot();
        GameObject myToken = allEnemiesInLevel[UnityEngine.Random.Range(0, allEnemiesInLevel.Count)];

        if (mySlot != null && myToken != null)
        {
            GameObject newTok = Instantiate(myToken, mySlot.transform);
            newTok.GetComponent<EnemyToken>().mySlot = mySlot.GetComponent<EnemyTokenSlot>();
        }
    }

    public void CheckEnemyTargets()
    {
        foreach (GameObject enem in allEnemySlotsWithTokens)
        {
            if (enem.GetComponentInChildren<EnemyToken>() == null) continue;
            enem.GetComponentInChildren<EnemyToken>().SearchTargetsAndShowWarning();
        }
    }
}
