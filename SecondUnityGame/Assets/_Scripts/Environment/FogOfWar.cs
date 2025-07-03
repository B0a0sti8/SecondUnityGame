using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    Vector2 rightUp, rightDown, leftUp, leftDown;
    List<Vector2> fogPositions = new List<Vector2>();

    [SerializeField] GameObject fogPrefab;

    void Start()
    {
        rightUp = transform.Find("Corners").Find("UpperRight").position;
        rightDown = transform.Find("Corners").Find("LowerRight").position;
        leftUp = transform.Find("Corners").Find("UpperLeft").position;
        leftDown = transform.Find("Corners").Find("LowerLeft").position;

        for (int i = (int)leftDown.y + 20; i < (int)leftUp.y; i += 20)
        {
            fogPositions.Add(new Vector2(leftDown.x, i));
            fogPositions.Add(new Vector2(rightDown.x, i));
        }

        for (int i = (int)leftDown.x + 20; i < (int)rightDown.x; i += 20)
        {
            fogPositions.Add(new Vector2(i, leftDown.y));
            fogPositions.Add(new Vector2(i, leftUp.y));
        }

        fogPositions.Add(rightUp);
        fogPositions.Add(rightDown);
        fogPositions.Add(leftUp);
        fogPositions.Add(leftDown);

        foreach (Vector2 pos in fogPositions)
        {
            GameObject myFog = Instantiate(fogPrefab, transform.Find("FogElements"));
            myFog.transform.position = pos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
