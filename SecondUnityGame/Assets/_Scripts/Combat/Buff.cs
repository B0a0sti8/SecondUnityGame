using UnityEngine;

public class Buff : MonoBehaviour
{
    public Sprite buffSprite;
    public string buffName;
    public string buffDescription;

    public DefaultToken myToken;
    public int remainingDuration;
    public bool hasTurnEffect;

    public float myBuffStrengthMod;

    public virtual void StartBuffEffect(DefaultToken newTok, int turnDuration, Sprite newSprite, string newName, float buffStrengthMod)
    {
        myBuffStrengthMod = buffStrengthMod;
        myToken = newTok;
        remainingDuration = turnDuration;
        buffSprite = newSprite;
        myToken.myCurrentBuffs.Add(this);
        //buffName = newName;
    }

    public virtual void EndBuffEffect()
    {
        myToken.myCurrentBuffs.Remove(this);
    }

    public virtual void TriggerBuffEffectInTurn()
    {
        remainingDuration -= 1;
        if (remainingDuration <= 0) EndBuffEffect();
       //Debug.Log("Triggering Buff Effect. Remaining duration:" + remainingDuration);
    }

    public Buff Clone()
    {
        return (Buff)this.MemberwiseClone();
    }
}
