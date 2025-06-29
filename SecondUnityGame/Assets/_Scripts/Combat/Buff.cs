using UnityEngine;

public class Buff : MonoBehaviour
{
    public Sprite buffSprite;
    public string buffName;
    public string buffDescription;

    DefaultToken myToken;
    public int remainingDuration;
    public bool hasTurnEffect;

    public virtual void StartBuffEffect(DefaultToken newTok, int turnDuration, Sprite newSprite, string newName)
    {
        myToken = newTok;
        remainingDuration = turnDuration;
        buffSprite = newSprite;
        myToken.myCurrentBuffs.Add(this);
        Debug.Log(myToken.myCurrentBuffs[0].buffName);
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
