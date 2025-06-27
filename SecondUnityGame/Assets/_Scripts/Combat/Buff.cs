using UnityEngine;

public class Buff
{
    DefaultToken myToken;
    public float remainingDuration;
    public bool hasTurnEffect;

    public virtual void StartBuffEffect(DefaultToken newTok, float duration)
    {
        myToken = newTok;
        myToken.myCurrentBuffs.Add(this);
        remainingDuration = duration;
    }

    public virtual void EndBuffEffect()
    {
        
    }

    public virtual void TriggerBuffEffectInTurn()
    {
        remainingDuration -= 1;
    }

    public Buff Clone()
    {
        return (Buff)this.MemberwiseClone();
    }
}
