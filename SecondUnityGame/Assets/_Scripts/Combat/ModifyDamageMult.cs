using UnityEngine;

public class ModifyDamageMult : Buff
{
    float myBaseEffekt;
    float myFinalEffekt;

    public override void StartBuffEffect(DefaultToken newTok, int turnDuration, Sprite newSprite, string newName, float buffStrengthMod)
    {
        buffName = "Reduced Damage";
        buffDescription = "Reduces damage significantly";

        base.StartBuffEffect(newTok, turnDuration, newSprite, newName, buffStrengthMod);

        myBaseEffekt = 0.1f;
        myFinalEffekt = myBaseEffekt * buffStrengthMod;

        myToken.dmgHealModifiersMult.Add(myFinalEffekt);
    }

    public override void EndBuffEffect()
    {
        myToken.dmgHealModifiersMult.Remove(myFinalEffekt);
        base.EndBuffEffect();
    }
}
