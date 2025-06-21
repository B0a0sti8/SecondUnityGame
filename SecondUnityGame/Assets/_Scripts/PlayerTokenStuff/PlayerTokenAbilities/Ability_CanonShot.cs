using UnityEngine;

public class Ability_CanonShot : PlayerTokenAbilityPrefab
{
    protected override void Start()
    {
        base.Start();
        energyCost = 2;
        abilityCheckPoints = 0;
        range = 12;
        isSingleTarget = false;
        abilityCheckPointsMax = 1;

        skillDamageBaseModifier = 0.7f;

        abilityName = "Canon shot";
        abilityDescription = "Shoots his canon to attack a whole area.";
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
