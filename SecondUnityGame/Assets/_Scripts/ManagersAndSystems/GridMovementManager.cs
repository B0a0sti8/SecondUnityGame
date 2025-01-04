using System.Collections.Generic;
using UnityEngine;

public class GridMovementManager : MonoBehaviour
{
    public static GridMovementManager instance;

    public int[,] mapMovementCostArray;
    public GameObject[,] allTokenSlots;
    public GameObject[,] allMovementIndicators;
    public List<GameObject> activeMovementIndicators;
    public int xMax, yMax;

    List<int[]> tilesToCheck = new List<int[]>();

    private void Awake()
    {
        instance = this;
    }

    public void TokenWantsToMove(int xValue, int yValue)
    {
        // An die Funktion IterativeMovementCheck wird je eine Liste gegeben, die die Positionen der Felder beinhaltet, die getestet werden sollen und zusätzlich die Energy die übrig ist.

        int energyRem = allTokenSlots[xValue, yValue].GetComponent<TokenSlot>().myCardToken.currentEnergy;
        MouseClickAndGrabManager.instance.movingTokenOrigin = allTokenSlots[xValue, yValue];

        tilesToCheck.Clear();
        PrepareTiles(new int[] { xValue, yValue }, energyRem);

        List<int[]> startingList = new List<int[]>();
        startingList.Add(new int[] { xValue, yValue, energyRem });

        IterativeMovementCheck(startingList, 0);
    }

    void PrepareTiles(int[] kek, int energyRem)
    {
        for (int xVal = 0; xVal < xMax; xVal++)
        {
            for (int yVal = 0; yVal < yMax; yVal++)
            {
                if (Mathf.Abs(kek[0] - xVal) + Mathf.Abs(kek[1] - yVal) < energyRem)
                {
                    //allMovementIndicators[xVal, yVal].SetActive(true);
                    tilesToCheck.Add(new int[] { xVal, yVal });
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
            if (TestTileInList(tileToTest1)) newTilesToCheck.Add(tileToTest1);

            int[] tileToTest2 = new int[3];
            tileToTest2[0] = tile[0] - 1; tileToTest2[1] = tile[1]; tileToTest2[2] = newEnergyRemain;
            if (TestTileInList(tileToTest2)) newTilesToCheck.Add(tileToTest2);

            int[] tileToTest3 = new int[3];
            tileToTest3[0] = tile[0]; tileToTest3[1] = tile[1] + 1; tileToTest3[2] = newEnergyRemain;
            if (TestTileInList(tileToTest3)) newTilesToCheck.Add(tileToTest3);

            int[] tileToTest4 = new int[3];
            tileToTest4[0] = tile[0]; tileToTest4[1] = tile[1] - 1; tileToTest4[2] = newEnergyRemain;
            if (TestTileInList(tileToTest4)) newTilesToCheck.Add(tileToTest4);
        }

        if (newTilesToCheck.Count > 0)
        {
            IterativeMovementCheck(newTilesToCheck, counter);
        }
    }

    bool TestTileInList(int[] tileToTest)
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
}