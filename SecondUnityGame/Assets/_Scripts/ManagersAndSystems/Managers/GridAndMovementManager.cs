using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAndMovementManager : MonoBehaviour
{
    public static GridAndMovementManager instance;

    public int[,] ressourceMap;

    public int[,] mapMovementCostArray;
    public GameObject[,] allTokenSlots;
    public GameObject[,] allMovementIndicators;
    public List<GameObject> activeMovementIndicators;
    public int xMax, yMax;

    List<GameObject> pathMarkers = new List<GameObject>();
    List<int[]> tilesToCheck = new List<int[]>();
    List<int[]> tilesToCheck_Pathfind = new List<int[]>();
    List<int[]> tilesToCheck_Pathfind_enemies = new List<int[]>();

    public int[,] enemyAndAllyMap; // Allies = 1, Enemies = 2

    private void Awake()
    {
        instance = this;
    }

    public void SetRessourceTypes()
    {
        for (int xPos = 0; xPos < xMax; xPos++)
        {
            for (int yPos = 0; yPos < yMax; yPos++)
            {
                int tile = ressourceMap[xPos, yPos];
                RessourceManager.ressourceType resType = RessourceManager.ressourceType.none;

                if (tile >= 101 && tile < 109) resType = RessourceManager.ressourceType.wood;               // baum
                else if (tile >= 201 && tile < 210) resType = RessourceManager.ressourceType.stone;         // stein
                else if (tile >= 301 && tile < 310) resType = RessourceManager.ressourceType.food;          // Kraut
                else if (tile == 401 || tile == 402) resType = RessourceManager.ressourceType.reagents;     // Kristall
                else if (tile >= 403 && tile < 406) resType = RessourceManager.ressourceType.knowledge;     // Runenstein
                else if (tile >= 406 && tile < 409) resType = RessourceManager.ressourceType.coin;     // Schatz
                else resType = RessourceManager.ressourceType.none;

                allTokenSlots[xPos, yPos].GetComponent<TokenSlot>().SetRessource(resType);
            }
        }
    }

    public List<int[]> EnemyWantsToSearchAllies(int xValue, int yValue, int distance)
    {
        List<int[]> allyPos = new List<int[]>();
        for (int xVal = xValue - distance; xVal < xValue + distance + 1; xVal++)
        {
            for (int yVal = yValue - distance; yVal < yValue + distance + 1; yVal++)
            {
                if (xVal >=0 && xVal < xMax && yVal >= 0 && yVal < yMax)
                {
                    //Debug.Log(xVal + " / " + yVal);
                    if (Mathf.Abs(xValue - xVal) + Mathf.Abs(yValue - yVal) < distance)
                    {
                        allMovementIndicators[xVal, yVal].SetActive(true);
                        if (enemyAndAllyMap[xVal, yVal] == 1) // Ally gefunden!
                        {

                            allyPos.Add(new int[] { xVal, yVal});
                        }
                    }
                }
            }
        }
        return allyPos;
    }

    public void AllyTokenWantsToMove(int xValue, int yValue)
    {
        // An die Funktion IterativeMovementCheck wird je eine Liste gegeben, die die Positionen der Felder beinhaltet, die getestet werden sollen und zusätzlich die Energy die übrig ist.
        DisableMovementMarkers();
        int energyRem = allTokenSlots[xValue, yValue].GetComponent<TokenSlot>().currentEnergy;
        MouseClickAndGrabManager.instance.movingTokenOrigin = allTokenSlots[xValue, yValue];

        tilesToCheck.Clear();
        tilesToCheck_Pathfind.Clear();
        PrepareTilesForMovement(new int[] { xValue, yValue }, energyRem + mapMovementCostArray[xValue, yValue]);

        List<int[]> startingList = new List<int[]>();
        startingList.Add(new int[] { xValue, yValue, energyRem + mapMovementCostArray[xValue, yValue]});

        IterativeMovementCheck(startingList, 0);
    }

    void PrepareTilesForMovement(int[] kek, int energyRem)
    {
        for (int xVal = 0; xVal < xMax; xVal++)
        {
            for (int yVal = 0; yVal < yMax; yVal++)
            {
                if (Mathf.Abs(kek[0] - xVal) + Mathf.Abs(kek[1] - yVal) < energyRem)
                {
                    tilesToCheck.Add(new int[] { xVal, yVal });
                    tilesToCheck_Pathfind.Add(new int[] { xVal, yVal });
                }
            }
        }
    }

    void IterativeMovementCheck(List<int[]> myTileList, int counter = 0)
    {
        if (counter >= 100) return;
        counter += 1;

        List<int[]> newTilesToCheck = new List<int[]>();

        foreach (int[] tile in myTileList)
        {        
            int newEnergyRemain = tile[2] - mapMovementCostArray[tile[0], tile[1]];

            int[] tileToTest1 = new int[3];
            tileToTest1[0] = tile[0] + 1; tileToTest1[1] = tile[1]; tileToTest1[2] = newEnergyRemain;
            if (TestTileInListForMovement(tileToTest1)) newTilesToCheck.Add(tileToTest1);

            int[] tileToTest2 = new int[3];
            tileToTest2[0] = tile[0] - 1; tileToTest2[1] = tile[1]; tileToTest2[2] = newEnergyRemain;
            if (TestTileInListForMovement(tileToTest2)) newTilesToCheck.Add(tileToTest2);

            int[] tileToTest3 = new int[3];
            tileToTest3[0] = tile[0]; tileToTest3[1] = tile[1] + 1; tileToTest3[2] = newEnergyRemain;
            if (TestTileInListForMovement(tileToTest3)) newTilesToCheck.Add(tileToTest3);

            int[] tileToTest4 = new int[3];
            tileToTest4[0] = tile[0]; tileToTest4[1] = tile[1] - 1; tileToTest4[2] = newEnergyRemain;
            if (TestTileInListForMovement(tileToTest4)) newTilesToCheck.Add(tileToTest4);
        }

        if (newTilesToCheck.Count > 0)
        {
            IterativeMovementCheck(newTilesToCheck, counter);
        }
    }

    bool TestTileInListForMovement(int[] tileToTest)
    {
        List<int[]> removeStuff = new List<int[]>();
        bool isInList = false;
        foreach (int[] element in tilesToCheck)
        {
            if (element[0] == tileToTest[0] && element[1] == tileToTest[1])
            {
                isInList = true;
                removeStuff.Add(element);
            }
        }

        if ((tileToTest[0] >= 0 && tileToTest[0] < xMax && tileToTest[1] >= 0 && tileToTest[1] < yMax && mapMovementCostArray[tileToTest[0], tileToTest[1]] <= tileToTest[2]) && isInList)
        {
            allMovementIndicators[tileToTest[0], tileToTest[1]].SetActive(true);
            activeMovementIndicators.Add(allMovementIndicators[tileToTest[0], tileToTest[1]]);
            tilesToCheck.Remove(removeStuff[0]);
            removeStuff.Clear();
            return true;
        }
        else return false;
    }

    public void DisableMovementMarkers()
    {
        foreach (GameObject mark in activeMovementIndicators)
        {
            mark.SetActive(false);
        }
        activeMovementIndicators.Clear();
    }

    public int[] Pathfinding_FindEnergyCostBetweenTokenSlots_LimitedStep(GameObject origin, GameObject destination, int maxStep)
    {
        foreach (GameObject pm in pathMarkers)
        {
            pm.SetActive(false);
        }
        pathMarkers.Clear();
        tilesToCheck_Pathfind_enemies.Clear();

        List<int[]> openList = new List<int[]>();
        int energyCost = 0;
        int[] originPos = new int[] { (int)Mathf.Round(origin.transform.position.x), (int)Mathf.Round(origin.transform.position.y), 0 };
        int[] destinationPos = new int[] { (int)Mathf.Round(destination.transform.position.x), (int)Mathf.Round(destination.transform.position.y) };
        int[] lowC = originPos;

        for (int xVal = 0; xVal < xMax; xVal++)
        {
            for (int yVal = 0; yVal < yMax; yVal++)
            {
                if (Mathf.Abs(originPos[0] - xVal) + Mathf.Abs(originPos[1] - yVal) < maxStep + 1)
                {
                    tilesToCheck_Pathfind_enemies.Add(new int[] { xVal, yVal });
                }
            }
        }

        Debug.Log("Before first run: FieldPos = " + lowC[0] + " / " + lowC[1]);

        int removeOrigin = 1000;
        for (int i = 0; i < tilesToCheck_Pathfind_enemies.Count; i++)
        {
            int[] tile = tilesToCheck_Pathfind_enemies[i];
            if (originPos[0] == tile[0] && originPos[1] == tile[1])
            {
                removeOrigin = i;
            }
        }
        if (removeOrigin != 1000) tilesToCheck_Pathfind_enemies.RemoveAt(removeOrigin);

        // for debugging
        GameObject pathMarker = allTokenSlots[originPos[0], originPos[1]].transform.Find("PathfindingMarker").gameObject;
        pathMarkers.Add(pathMarker);
        pathMarker.SetActive(true);

        bool foundDestination = false;
        int counter = 0;

        while (!foundDestination && counter < maxStep)
        {
            counter += 1;

            openList = Pathfinding_GetNeighbourTiles_limitedStep(lowC);
            lowC = Pathfinding_GiveLowestCostTile(openList, destinationPos, out bool isDestination);
            foundDestination = isDestination;
            if (foundDestination)
            {
                energyCost = lowC[2];
            }

            Debug.Log("Run Nr. " + counter + "FieldPos = " + lowC[0] + " / " + lowC[1]);
        }
        tilesToCheck_Pathfind.Clear();
        StartCoroutine(Pathfinding_ClearUpMarks(0.5f));
        return lowC;
    }

    public int Pathfinding_FindEnergyCostBetweenTokenSlots(GameObject origin, GameObject destination)
    {
        foreach (GameObject pm in pathMarkers)
        {
            pm.SetActive(false);
        }
        pathMarkers.Clear();

        List<int[]> openList = new List<int[]>();
        int energyCost = 0;
        int[] originPos = new int[] { (int)Mathf.Round(origin.transform.position.x), (int)Mathf.Round(origin.transform.position.y), 0};
        int[] destinationPos = new int[] { (int)Mathf.Round(destination.transform.position.x), (int)Mathf.Round(destination.transform.position.y)};
        int[] lowC = originPos;

        int removeOrigin = 1000;
        for (int i = 0; i < tilesToCheck_Pathfind.Count; i++)
        {
            int[] tile = tilesToCheck_Pathfind[i];
            if (originPos[0] == tile[0] && originPos[1] == tile[1])
            {
                removeOrigin = i;
            }
        }
        if (removeOrigin != 1000) tilesToCheck_Pathfind.RemoveAt(removeOrigin);

        // for debugging
        GameObject pathMarker = allTokenSlots[originPos[0], originPos[1]].transform.Find("PathfindingMarker").gameObject;
        pathMarkers.Add(pathMarker);
        pathMarker.SetActive(true);

        bool foundDestination = false;
        int counter = 0;

        while (!foundDestination && counter < 20)
        {
            counter += 1;

            openList = Pathfinding_GetNeighbourTiles(lowC);
            lowC = Pathfinding_GiveLowestCostTile(openList, destinationPos, out bool isDestination);
            foundDestination = isDestination;
            if (foundDestination)
            {
                energyCost = lowC[2];
            }
        }
        tilesToCheck_Pathfind.Clear();
        StartCoroutine(Pathfinding_ClearUpMarks(0.5f));
        return energyCost;
    }

    int[] Pathfinding_GiveLowestCostTile(List<int[]> potTiles, int[] destPos, out bool isDestination)
    {
        isDestination = false;
        int[] lowestCostTile = new int[] { 0, 0, 1000 };
        float lowestHCost = 100;

        foreach (int[] tile in potTiles)
        {
            float hCost = Mathf.Sqrt(Mathf.Pow(destPos[0] - tile[0], 2) + Mathf.Pow(destPos[1] - tile[1], 2)); // h cost: optimistische Abschätzung des Abstandes zwischen Tile und Destination
            if (hCost == 0)
            {
                // Found Destination
                isDestination = true;
            }
            int gCost = tile[2] + mapMovementCostArray[tile[0], tile[1]]; // Bisheriger, tatsächlicher Laufweg. In diesem Fall: Bisherige Energy kosten.
            float fCost = gCost + hCost;

            if (fCost < lowestCostTile[2] + lowestHCost)
            {
                tile[2] = gCost;
                lowestCostTile = tile;
                lowestHCost = hCost;
            }
            else if (hCost < lowestHCost) // gCost < lowestCostTile[2]
            {
                tile[2] = gCost;
                lowestCostTile = tile;
                lowestHCost = hCost;
            }
        }
        GameObject pathMarker = allTokenSlots[lowestCostTile[0], lowestCostTile[1]].transform.Find("PathfindingMarker").gameObject;
        pathMarker.SetActive(true);
        pathMarkers.Add(pathMarker);

        return lowestCostTile;
    }

    List<int[]> Pathfinding_GetNeighbourTiles(int[] tilePos)
    {
        List<int> removeList = new List<int>();
        List<int[]> neighbours = new List<int[]>();

        for (int i = 0; i < tilesToCheck_Pathfind.Count; i++)
        {
            int[] tile = tilesToCheck_Pathfind[i];

            if (tile[0] == tilePos[0] + 1 && tile[1] == tilePos[1])
            {
                neighbours.Add(new int[] { tilePos[0] + 1, tilePos[1], tilePos[2] });
                removeList.Add(i);
            }

            if (tile[0] == tilePos[0] - 1 && tile[1] == tilePos[1])
            {
                neighbours.Add(new int[] { tilePos[0] - 1, tilePos[1], tilePos[2] });
                removeList.Add(i);
            }

            if (tile[0] == tilePos[0] && tile[1] == tilePos[1] + 1)
            {
                neighbours.Add(new int[] { tilePos[0], tilePos[1] + 1, tilePos[2] });
                removeList.Add(i);
            }

            if (tile[0] == tilePos[0] && tile[1] == tilePos[1] - 1)
            {
                neighbours.Add(new int[] { tilePos[0], tilePos[1] - 1, tilePos[2] });
                removeList.Add(i);
            }
        }

        for (int i = removeList.Count - 1; i >= 0; i--)
        {
            tilesToCheck_Pathfind.RemoveAt(removeList[i]);
        }
        removeList.Clear();
        return neighbours;
    }

    List<int[]> Pathfinding_GetNeighbourTiles_limitedStep(int[] tilePos)
    {
        List<int> removeList = new List<int>();
        List<int[]> neighbours = new List<int[]>();

        for (int i = 0; i < tilesToCheck_Pathfind_enemies.Count; i++)
        {
            int[] tile = tilesToCheck_Pathfind_enemies[i];

            if (tile[0] == tilePos[0] + 1 && tile[1] == tilePos[1])
            {
                neighbours.Add(new int[] { tilePos[0] + 1, tilePos[1], tilePos[2] });
                removeList.Add(i);
            }

            if (tile[0] == tilePos[0] - 1 && tile[1] == tilePos[1])
            {
                neighbours.Add(new int[] { tilePos[0] - 1, tilePos[1], tilePos[2] });
                removeList.Add(i);
            }

            if (tile[0] == tilePos[0] && tile[1] == tilePos[1] + 1)
            {
                neighbours.Add(new int[] { tilePos[0], tilePos[1] + 1, tilePos[2] });
                removeList.Add(i);
            }

            if (tile[0] == tilePos[0] && tile[1] == tilePos[1] - 1)
            {
                neighbours.Add(new int[] { tilePos[0], tilePos[1] - 1, tilePos[2] });
                removeList.Add(i);
            }
        }

        for (int i = removeList.Count - 1; i >= 0; i--)
        {
            tilesToCheck_Pathfind_enemies.RemoveAt(removeList[i]);
        }
        removeList.Clear();
        return neighbours;
    }

    public IEnumerator Pathfinding_ClearUpMarks(float time)
    {
        yield return new WaitForSeconds(time);

        foreach (GameObject pm in pathMarkers)
        {
            pm.SetActive(false);
        }
        pathMarkers.Clear();
    }
}