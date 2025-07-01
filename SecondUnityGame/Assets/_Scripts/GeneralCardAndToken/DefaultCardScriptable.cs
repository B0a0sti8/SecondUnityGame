using System;
using UnityEngine;

//[CreateAssetMenu(fileName = "DefaultCardScriptable", menuName = "Scriptable Objects/DefaultCardScriptable")]
public class DefaultCardScriptable : ScriptableObject
{
    public string cardName;
    public string description;
    public string cardTypeString; // "Unit", "Building", oder "Ability"
    public int maxCardAmount;
    public int cardRank;

    public int woodCost;
    public int stoneCost;
    public int foodCost;
    public int reagentCost;

    public int manaCost;

    public Sprite cardSprite;
    public Sprite cardTypeSprite;
}
