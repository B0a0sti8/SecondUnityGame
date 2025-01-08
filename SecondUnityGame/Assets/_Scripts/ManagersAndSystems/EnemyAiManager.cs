using System.Collections.Generic;
using UnityEngine;

public class EnemyAiManager : MonoBehaviour
{
    public static EnemyAiManager instance;

    public List<CardPrefabScriptable> allEnemyTokenPrefabs;
    public List<CardPrefabScriptable> activeEnemyTokens = new List<CardPrefabScriptable>();
    public Dictionary<CardPrefabScriptable, int[]> enemyPositions = new Dictionary<CardPrefabScriptable, int[]>();
    public int gridXMax, gridYMax;
    public int[,] tileArray;

    void Awake()
    {
        instance = this;
    }

    public void SpawnRandomEnemyDebug()
    {
        bool hasSpawned = false;

        for (int i = 0; i < 40; i++)
        {
            hasSpawned = SpawnRandomEnemyAtRandomPosition();
            if (hasSpawned) break;
        }
    }

    public void MoveRandomEnemyDebug()
    {
        CardPrefabScriptable enemyMoving = activeEnemyTokens[Random.Range(0, activeEnemyTokens.Count)];
        MoveEnemyToken(enemyMoving);
    }


    public bool SpawnRandomEnemyAtRandomPosition()
    {
        int[] position = new int[] { Random.Range(0, gridXMax), Random.Range(0, gridYMax) };
        if (!(tileArray[position[0], position[1]] == 400)) return false;
        if (GridMovementManager.instance.allTokenSlots[position[0], position[1]].GetComponent<TokenSlot>().hasToken) return false;

        CardPrefabScriptable myEnemy = allEnemyTokenPrefabs[Random.Range(0, allEnemyTokenPrefabs.Count)];
        GridMovementManager.instance.allTokenSlots[position[0], position[1]].GetComponent<TokenSlot>().SetToken(myEnemy);
        activeEnemyTokens.Add(myEnemy);
        enemyPositions.Add(myEnemy, position);
        Debug.Log("Enemy Spawned at: " + position[0] + " " + position[1]);
        return true;
    }


    public void MoveEnemyToken(CardPrefabScriptable enemy)
    {
        int[] start = enemyPositions[enemy];
        GridMovementManager.instance.TokenWantsToMove(start[0], start[1]);
    }
}
