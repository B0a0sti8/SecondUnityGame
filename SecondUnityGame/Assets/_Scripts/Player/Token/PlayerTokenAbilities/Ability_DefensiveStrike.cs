using UnityEngine;

public class Ability_DefensiveStrike : PlayerTokenAbilityPrefab
{
    float myBaseBuffStrengthMod;

    protected override void Start()
    {
        myTargetType = TargetType.SingleTarget;
        base.Start();
        energyCost = 1;
        abilityCheckPoints = 0;
        range = 10;

        abilityCheckPointsMax = 1;

        skillEffectModifier = 0.7f;

        abilityName = "Defensive Strike";
        abilityDescription = "Strikes one target and reduces its damage for 2 turns";

        myBaseBuffStrengthMod = -2f;
    }

    public override void ApplyAbilityEffect()
    {
        DealDamageHealing();

        base.ApplyAbilityEffect();
    }

    public override void DealDamageHealing()
    {
        base.DealDamageHealing();

        float myBuffStrengthMod = myBaseBuffStrengthMod;

        foreach (GameObject curTar in currentTargets)
        {
            if (curTar.GetComponentInChildren<DefaultToken>() != null)
            {
                BattleManager.instance.DealDamageOrHealing(curTar.GetComponentInChildren<DefaultToken>().gameObject, transform.parent.gameObject, finalDamage);
                BattleManager.instance.ApplyBuffToTarget(curTar.GetComponentInChildren<DefaultToken>().gameObject, transform.parent.gameObject, myBuff, abilityIcon, 3, myBuffStrengthMod);
            }
        }
    }
}
