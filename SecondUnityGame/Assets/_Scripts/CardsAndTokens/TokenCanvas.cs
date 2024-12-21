using System.Collections.Generic;
using UnityEngine;

public class TokenCanvas : MonoBehaviour
{
    Dictionary<int, GameObject> myTokenSlots;
    [SerializeField] GameObject tokenSlotPrefab;

    public void InstantiateTokenSlots(int levelWidth, int levelHeight)
    {
        myTokenSlots = new Dictionary<int, GameObject>();

        for (int i = 0; i < levelWidth; i++)
        {
            for (int k = 0; k < levelHeight; k++)
            {
                GameObject myTok = Instantiate(tokenSlotPrefab, transform);
                myTokenSlots.Add(i + levelWidth * k, myTok);
            }
        }
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InstantiateTokenSlots(14, 20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
