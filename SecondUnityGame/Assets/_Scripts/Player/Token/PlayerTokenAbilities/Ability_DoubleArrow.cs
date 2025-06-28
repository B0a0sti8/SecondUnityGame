using UnityEngine;

public class Ability_DoubleArrow : PlayerTokenAbilityPrefab
{
    protected override void Start()
    {
        myTargetType = TargetType.SingleTarget;
        base.Start();
        energyCost = 1;
        abilityCheckPoints = 0;
        range = 10;

        abilityCheckPointsMax = 2;

        skillDmgHealModifier = 0.7f;

        abilityName = "Double Arrow";
        abilityDescription = "Shoots two arrows to arbitrary enemies";
    }

    public override void ApplyAbilityEffect()
    {
        DealDamageHealing();

        base.ApplyAbilityEffect();
    }

    public override void DealDamageHealing()
    {
        base.DealDamageHealing();

        foreach (GameObject curTar in currentTargets)
        {
            if (curTar.GetComponentInChildren<DefaultToken>() != null)
            {
                BattleManager.instance.DealDamageOrHealing(curTar.GetComponentInChildren<DefaultToken>().gameObject, transform.parent.gameObject, finalDamage);
            }
        }
    }
}
