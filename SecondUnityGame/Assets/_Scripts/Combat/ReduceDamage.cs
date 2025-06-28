using UnityEngine;

public class ReduceDamage : Buff
{
    public override void StartBuffEffect(DefaultToken newTok, int turnDuration, Sprite newSprite, string newName)
    {
        buffName = "Reduced Damage";
        buffDescription = "Reduces damage significantly";
        base.StartBuffEffect(newTok, turnDuration, newSprite, newName);
    }
}
