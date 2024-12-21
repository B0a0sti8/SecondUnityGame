using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    Dictionary<int, GameObject> usedCards;

    Dictionary<int, GameObject> cardDeck1;
    Dictionary<int, GameObject> cardDeck2;
    Dictionary<int, GameObject> cardDeck3;

    Dictionary<int, GameObject> handCards;

    public static CardManager instance;

    private void Awake()
    {
        instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
