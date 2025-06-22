using UnityEngine;

//[CreateAssetMenu(fileName = "DefaultCardScriptable", menuName = "Scriptable Objects/DefaultCardScriptable")]
public class DefaultCardScriptable : ScriptableObject
{
    public string cardName;
    public string description;
    public string cardTypeString; // "Unit", "Building", oder "Ability"

    public int woodCost;
    public int stoneCost;
    public int manaCost;

    public Sprite cardSprite;
    public Sprite cardTypeSprite;
}
