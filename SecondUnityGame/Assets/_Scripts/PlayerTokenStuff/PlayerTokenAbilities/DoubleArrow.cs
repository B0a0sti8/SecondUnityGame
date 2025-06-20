using UnityEngine;

public class DoubleArrow : PlayerTokenAbilityPrefab
{
    protected override void Start()
    {
        base.Start();
        energyCost = 1;
        abilityCheckPoints = 0;
        range = 10;
        isSingleTarget = true;
        abilityCheckPointsMax = 2;

        skillDamageBaseModifier = 0.7f;

        abilityName = "Double Arrow";
        abilityDescription = "Shoots two arrows to arbitrary enemies";
    }

    public override void ApplyAbilityEffect()
    {
        foreach (GameObject curTar in currentTargets)
        {
            if (curTar.GetComponentInChildren<DefaultToken>() != null)
            {
                float sourceDamage = skillDamageBaseModifier * transform.parent.parent.GetComponent<PlayerToken>().attackValue;
                BattleManager.instance.DealDamage(curTar.GetComponentInChildren<DefaultToken>().gameObject, transform.parent.gameObject, sourceDamage);
            }
        }

        base.ApplyAbilityEffect();
    }
}
