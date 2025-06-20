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

    private void Start()
    {
        for (int i = 0; i < levelObject.transform.Find("EnemySlots").childCount; i++)
        {
            allEnemySlots.Add(levelObject.transform.Find("EnemySlots").GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayerTurn)
        {
            // Bei Rundenbeginn, warte etwas
            if (enemyTurnTimer_Debug > 0) enemyTurnTimer_Debug -= Time.deltaTime;

            else
            {
                //Debug.Log("Starting Enemy Turn");
                enemyTurnTimer_Debug = 1f;
                for (int i = 0; i < allEnemySlots.Count; i++)
                {
                    if (!allEnemySlots[i].GetComponent<EnemyTokenSlot>().hasToken) continue;

                    //Debug.Log("Playing Enemy: " + allEnemySlots[i]);
                    PlayEnemyTurn(allEnemySlots[i]);
                }
                EndEnemyTurn();
            }
        }
    }

    public void EndPlayerTurn()
    {
        isPlayerTurn = false;
        turnIndicatorText.text = "Enemy Turn";
        Debug.Log("Ended Player Turn");
    }

    public void EndEnemyTurn()
    {
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
