using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Objects/PlayerToken")]
public class PlayerTokenScriptable : DefaultCardScriptable
{
    public Sprite tokenSprite;

    public int maxEnergy;
    public int maxLife;
    public float baseDmgHealValue;
    public float baseRecDmgHealValue;
    public int attackRange;

    public List<string> tokenAbilities = new List<string>();
    public List<string> tokenPassiveTriggerAbilities = new List<string>();

    public PlayerTokenScriptable Clone()
    {
        return (PlayerTokenScriptable)this.MemberwiseClone();
    }
}