using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TurnAndEnemyManager : MonoBehaviour
{
    public bool isPlayerTurn = true;
    [SerializeField] TextMeshProUGUI turnIndicatorText;
    float enemyTurnTimer_Debug = 1f;

    [SerializeField] GameObject levelObject;
    List<GameObject> allEnemySlots = new List<GameObject>();

    [SerializeField] List<GameObject> allEnemiesInLevel = new List<GameObject>();

    float timeBetweenEnemies;
    float timeBetweenEnemiesElapsed;
    int enemyActionCounter;
    int enemySpawnCounter;
    int enemySpawnAmountPerTurn;

    List<GameObject> allEnemySlotsWithTokens;
    public List<GameObject> allPlayerSlotsWithTokens;

    public static TurnAndEnemyManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < levelObject.transform.Find("EnemySlots").childCount; i++)
        {
            allEnemySlots.Add(levelObject.transform.Find("EnemySlots").GetChild(i).gameObject);
        }
        timeBetweenEnemies = 0.5f;
        timeBetweenEnemiesElapsed = 0f;
        enemyActionCounter = 0;
        enemySpawnCounter = 0;
        enemySpawnAmountPerTurn = 2;

        allEnemySlotsWithTokens = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
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

        for (int i = 0; i < allEnemySlots.Count; i++)
        {
            if (allEnemySlots[i].GetComponentInChildren<EnemyToken>() != null)
            {
                allEnemySlotsWithTokens.Add(allEnemySlots[i]);
            }
        }

        StartEnemyTurn();
    }

    public void EndEnemyTurn()
    {
        // Update die Buffs für alle Enemy Tokens
        for (int i = 0; i < allEnemySlotsWithTokens.Count; i++)
        {
            if (allEnemySlotsWithTokens[i].GetComponentInChildren<DefaultToken>() != null)
            {
                allEnemySlotsWithTokens[i].GetComponentInChildren<DefaultToken>().UpdateBuffs();
            }
        }

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
        CardManager.instance.DrawNextCardFromDeck();

        foreach (GameObject playerToken in allPlayerSlotsWithTokens)
        {
            playerToken.GetComponent<PlayerToken>().TriggerPassiveAbilities();
        }
    }

    void PlayEnemyTurn(GameObject enemyTokenSlot)
    {
        EnemyToken enemyToken = enemyTokenSlot.GetComponentInChildren<EnemyToken>();
        enemyToken.EvaluateBuffsAndDebuffs();
        enemyToken.ChooseTargetsAndUseSkills();
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
        else return emptySlots[Random.Range(0, emptySlots.Count)];
    }

    void SpawnRandomEnemyInRandomSlot()
    {
        GameObject mySlot = FindEmptyEnemySlot();
        GameObject myToken = allEnemiesInLevel[Random.Range(0, allEnemiesInLevel.Count)];

        if (mySlot != null && myToken != null) Instantiate(myToken, mySlot.transform);

    }
}
