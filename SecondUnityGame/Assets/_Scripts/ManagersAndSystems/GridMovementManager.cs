using UnityEngine;

public class GridMovementManager : MonoBehaviour
{
    public static GridMovementManager instance;

    public int[,] mapMovementCostArray;
    public GameObject[,] allTokenSlots;

    private void Awake()
    {
        instance = this;
    }

    public void TokenWantsToMove(int xValue, int yValue)
    {
        allTokenSlots[xValue + 1, yValue].transform.Find("MovementMarker").gameObject.SetActive(true);
        allTokenSlots[xValue - 1, yValue].transform.Find("MovementMarker").gameObject.SetActive(true);
        allTokenSlots[xValue, yValue + 1].transform.Find("MovementMarker").gameObject.SetActive(true);
        allTokenSlots[xValue, yValue - 1].transform.Find("MovementMarker").gameObject.SetActive(true);
    }

}
