using UnityEngine;

public class ModifyDamageMult : Buff
{
    float myBaseEffekt;
    float myFinalEffekt;

    public override void StartBuffEffect(DefaultToken newTok, int turnDuration, Sprite newSprite, string newName, float buffStrengthMod)
    {
        if (buffStrengthMod > 0)
        {
            buffName = "Increased Damage";
            buffDescription = "Increases damage significantly";
        }
        else
        {
            buffName = "Reduced Damage";
            buffDescription = "Reduces damage significantly";
        }

        base.StartBuffEffect(newTok, turnDuration, newSprite, newName, buffStrengthMod);

        myBaseEffekt = 0.1f;
        myFinalEffekt = myBaseEffekt * buffStrengthMod;

        myToken.dmgHealVal.AddModifierMultiply(myFinalEffekt);
    }

    public override void EndBuffEffect()
    {
        myToken.dmgHealVal.RemoveModifierMultiply(myFinalEffekt);
        base.EndBuffEffect();
    }
}
