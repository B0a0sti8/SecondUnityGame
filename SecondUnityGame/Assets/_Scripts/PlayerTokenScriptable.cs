using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Objects/Cards")]
public class PlayerTokenScriptable : ScriptableObject
{
    public string cardName;
    public string description;
    public string unitTypeString; // "Military Unit", "Building", "Building Unit" oder "Special"

    public int woodCost;
    public int stoneCost;
    public int manaCost;

    public Sprite cardSprite;
    public Sprite tokenSprite;
    public Sprite unitTypeSprite;

    public int maxEnergy;
    public int maxLife;
    public int attackValue;
    public int attackRange;

    public PlayerTokenScriptable Clone()
    {
        return (PlayerTokenScriptable)this.MemberwiseClone();
    }
}