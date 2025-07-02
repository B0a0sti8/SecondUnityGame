using UnityEngine;

public class Ability_CanonShot : PlayerTokenAbilityPrefab
{
    protected override void Start()
    {
        base.Start();
        energyCost = 2;
        abilityCheckPoints = 0;
        range = 12;
        myTargetType = TargetType.MultiTarget;
        abilityCheckPointsMax = 1;

        skillEffectModifier = 0.7f;

        abilityName = "Canon shot";
        abilityDescription = "Shoots his canon to attack a whole area.";
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
