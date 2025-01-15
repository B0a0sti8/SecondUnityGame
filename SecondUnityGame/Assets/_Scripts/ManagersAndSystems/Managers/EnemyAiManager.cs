using System.Collections.Generic;
using UnityEngine;

public class EnemyAiManager : MonoBehaviour
{
    public static EnemyAiManager instance;

    public List<CardPrefabScriptable> allEnemyTokenPrefabs;
    public List<CardPrefabScriptable> activeEnemyTokens = new List<CardPrefabScriptable>();
    public Dictionary<int, enemyData> allEnemyData = new Dictionary<int, enemyData>();
    Dictionary<int, int[]> newEnemyPositions;
    public int gridXMax, gridYMax;
    public int[,] tileArray;
    int enemyID;
    float enemyUpdateElapsed = 0;

    public struct enemyData
    {
        public CardPrefabScriptable enemyObject;
        public int[] enemyPosition;
    }

    void Awake()
    {
        instance = this;
        enemyID = 0;
        newEnemyPositions = new Dictionary<int, int[]>();
    }

    private void Update()
    {
        if (enemyUpdateElapsed < 0.1)
        {
            enemyUpdateElapsed += Time.deltaTime;
            return;
        }
        enemyUpdateElapsed = 0;

        // Hier wird für jeden Gegner die Logik laufen gelassen:

        foreach (var item in allEnemyData)
        {
            int enemyIdent = item.Key;
            CardPrefabScriptable enemy = item.Value.enemyObject;
            int[] position = item.Value.enemyPosition;

            // Check if Allies are nearby
            List<int[]> allyPos = GridAndMovementManager.instance.EnemyWantsToSearchAllies(position[0], position[1], 5);
            if (allyPos.Count == 0)
            {
                // Wenn niemand gefunden wurde, laufen wir bisschen wahllos in der Gegend herum.
                EnemiesSearching(enemyIdent, position);
            }
        }

        UpdateEnemyPositionsInDict();
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
        List<int> potentialEnemies = new List<int>(allEnemyData.Keys);
        int enemyMoving = potentialEnemies[Random.Range(0, potentialEnemies.Count)];
        MoveEnemyToken(enemyMoving);
    }

    int GetNewEnemyID()
    {
        enemyID += 1;
        return enemyID - 1;
    }

    public bool SpawnRandomEnemyAtRandomPosition()
    {
        int[] position = new int[] { Random.Range(0, gridXMax - 1), Random.Range(0, gridYMax - 1) };
        if (!(tileArray[position[0], position[1]] == 400)) return false;
        if (GridAndMovementManager.instance.allTokenSlots[position[0], position[1]].GetComponent<TokenSlot>().hasToken) return false;

        CardPrefabScriptable myEnemy = allEnemyTokenPrefabs[Random.Range(0, allEnemyTokenPrefabs.Count)].Clone();
        int thisEnemyID = GetNewEnemyID();
        enemyData thisEnemyData = new enemyData();
        thisEnemyData.enemyObject = myEnemy; thisEnemyData.enemyPosition = position;

        activeEnemyTokens.Add(myEnemy);
        allEnemyData.Add(thisEnemyID, thisEnemyData);
        GridAndMovementManager.instance.allTokenSlots[position[0], position[1]].GetComponent<TokenSlot>().SetToken(myEnemy);

        Debug.Log("Enemy Spawned at: " + position[0] + " " + position[1]);
        return true;
    }

    public void MoveEnemyToken(int enemyId)
    {
        int[] start = allEnemyData[enemyId].enemyPosition;
        GridAndMovementManager.instance.AllyTokenWantsToMove(start[0], start[1]);
    }

    void EnemiesSearching(int enemyIdent, int[] startPos)
    {
        // Gehe Random in eine Richtung, mit größerer Wahrscheinlichkeit nach unten zu laufen.
        int[] stopPos = new int[] { startPos[0], startPos[1] };
        int chance = Random.Range(0, 100);
        if (chance < 10) stopPos[1] += 1; // nach oben
        else if (chance < 35) stopPos[0] += 1; // nach rechts
        else if (chance < 60) stopPos[0] -= 1; // nach links
        else stopPos[1] -= 1; // nach unten

        if (MovingEnemy(startPos, stopPos))
        {
            newEnemyPositions.Add(enemyIdent, stopPos);
        }
    }

    bool MovingEnemy(int[] start, int[] stop)
    {
        int xMax = GridAndMovementManager.instance.xMax;
        int yMax = GridAndMovementManager.instance.yMax;

        bool hasMoved = false;
        TokenSlot myStartSlot = null;
        TokenSlot myStopSlot = null;
        if (start[0] >= 0 && start[0] < xMax && start[1] >= 0 && start[1] < yMax)
        {
            myStartSlot = GridAndMovementManager.instance.allTokenSlots[start[0], start[1]].GetComponent<TokenSlot>();
        }

        if (stop[0] >= 0 && stop[0] < xMax && stop[1] >= 0 && stop[1] < yMax)
        {
            myStopSlot = GridAndMovementManager.instance.allTokenSlots[stop[0], stop[1]].GetComponent<TokenSlot>();
        }

        if ((myStopSlot != null && myStopSlot.hasToken == false) && (myStartSlot != null && myStartSlot.hasToken == true))
        {
            int energyCost = GridAndMovementManager.instance.Pathfinding_FindEnergyCostBetweenTokenSlots(myStartSlot.gameObject, myStopSlot.gameObject);
            int currentEnergy = myStartSlot.currentEnergy;
            myStopSlot.SetToken(myStartSlot.myCardToken, false, currentEnergy - energyCost);
            myStartSlot.GetComponent<TokenSlot>().RemoveToken();
            hasMoved = true;
        }
        return hasMoved;
    }

    void UpdateEnemyPositionsInDict()
    {
        foreach (var item in newEnemyPositions)
        {
            enemyData thisEnemyData = allEnemyData[item.Key];
            thisEnemyData.enemyPosition = item.Value;
            allEnemyData[item.Key] = thisEnemyData;
        }
        newEnemyPositions.Clear();
    }
}
