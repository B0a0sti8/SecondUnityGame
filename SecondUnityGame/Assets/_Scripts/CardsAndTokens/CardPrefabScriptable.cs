using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Objects/Cards")]
public class CardPrefabScriptable : ScriptableObject
{
    public string cardName;
    public string description;
    public string unitTypeString;

    public int woodCost;
    public int stoneCost;
    public int manaCost;

    public Sprite cardSprite;
    public Sprite tokenSprite;
    public Sprite unitTypeSprite;

    public int maxEnergy;
    public int maxLife;
    public int attackValue;

    public CardPrefabScriptable Clone()
    {
        return (CardPrefabScriptable)this.MemberwiseClone();
    }
}