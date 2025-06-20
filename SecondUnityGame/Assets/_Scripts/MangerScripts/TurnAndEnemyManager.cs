using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TurnAndEnemyManager : MonoBehaviour
{
    bool isPlayerTurn = true;
    [SerializeField] TextMeshProUGUI turnIndicatorText;
    float enemyTurnTimer_Debug = 1f;

    [SerializeField] GameObject levelObject;
    List<GameObject> allEnemySlots = new List<GameObject>();

    public static TurnAndEnemyManager instance;

    float timeBetweenEnemies;
    float timeBetweenEnemiesElapsed;
    int enemyCounter;
    List<GameObject> allEnemySlotsWithTokens;

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
        timeBetweenEnemies = 0.4f;
        timeBetweenEnemiesElapsed = 0f;
        enemyCounter = 0;

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
            if (timeBetweenEnemiesElapsed < timeBetweenEnemies) timeBetweenEnemiesElapsed += Time.deltaTime;
            else
            {
                if (enemyCounter < allEnemySlotsWithTokens.Count)
                {
                    PlayEnemyTurn(allEnemySlotsWithTokens[enemyCounter]);
                    enemyCounter++;
                    timeBetweenEnemiesElapsed = 0f;
                }
                else
                {
                    EndEnemyTurn();
                }
            }
        }
    }

    public void EndPlayerTurn()
    {
        isPlayerTurn = false;
        turnIndicatorText.text = "Enemy Turn";
        Debug.Log("Ended Player Turn");

        for (int i = 0; i < allEnemySlots.Count; i++)
        {
            if (allEnemySlots[i].GetComponentInChildren<EnemyToken>() != null)
            {
                allEnemySlotsWithTokens.Add(allEnemySlots[i]);
            }
        }
    }

    public void EndEnemyTurn()
    {
        allEnemySlotsWithTokens.Clear();
        enemyTurnTimer_Debug = 1f;
        enemyCounter = 0;

        isPlayerTurn = true;
        turnIndicatorText.text = "Player Turn";
        Debug.Log("Ended Enemy Turn");
    }

    void PlayEnemyTurn(GameObject enemyTokenSlot)
    {
        EnemyToken enemyToken = enemyTokenSlot.GetComponentInChildren<EnemyToken>();
        enemyToken.EvaluateBuffsAndDebuffs();
        enemyToken.ChooseTargetsAndUseSkills();
    }
}
