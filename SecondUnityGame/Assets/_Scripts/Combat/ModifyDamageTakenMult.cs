using UnityEngine;

public class ModifyDamageTakenMult : Buff
{
    float myBaseEffekt;
    float myFinalEffekt;

    public override void StartBuffEffect(DefaultToken newTok, int turnDuration, Sprite newSprite, string newName, float buffStrengthMod)
    {
        if (buffStrengthMod > 0)
        {
            buffName = "Reduced Damage Taken";
            buffDescription = "Reduces damage taken significantly";
        }
        else
        {
            buffName = "Increased Damage Taken";
            buffDescription = "Increases damage taken significantly";
        }

        base.StartBuffEffect(newTok, turnDuration, newSprite, newName, buffStrengthMod);

        myBaseEffekt = -0.1f;
        myFinalEffekt = myBaseEffekt * buffStrengthMod;

        myToken.receiveDmgHealVal.AddModifierMultiply(myFinalEffekt);
        Debug.Log("Adding damage reduction: " + myFinalEffekt);
    }

    public override void EndBuffEffect()
    {
        myToken.receiveDmgHealVal.RemoveModifierMultiply(myFinalEffekt);
        base.EndBuffEffect();
    }
}
