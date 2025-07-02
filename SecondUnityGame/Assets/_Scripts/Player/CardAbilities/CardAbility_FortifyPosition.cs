using UnityEngine;

public class CardAbility_FortifyPosition : PlayerTokenAbilityPrefab
{
    protected override void Start()
    {
        myTargetType = TargetType.MultiTarget;
        base.Start();
        abilityCheckPoints = 0;
        range = 30;

        abilityCheckPointsMax = 1;

        skillEffectModifier = 2f;

        abilityName = "Fortify Position";
        abilityDescription = "Fortifies the target position. Units located at there, take significantly less damage. ";
    }

    public override void ApplyAbilityEffect()
    {
        Debug.Log("Triggering Card Ability!");

        float buffStrength = PlayerObject.instance.buffEffectMod.GetValue() * skillEffectModifier;

        foreach (GameObject curTar in currentTargets)
        {
            curTar.GetComponent<DefaultTokenSlot>().SetAreaModification(myBuff, myBuff.buffSprite, buffStrength);
        }

        base.ApplyAbilityEffect();
    }

    public override void DealDamageHealing()
    {
        base.DealDamageHealing();


    }
}
