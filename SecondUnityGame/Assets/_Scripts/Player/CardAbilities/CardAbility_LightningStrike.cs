using UnityEngine;

public class CardAbility_LightningStrike : PlayerTokenAbilityPrefab
{
    protected override void Start()
    {
        myTargetType = TargetType.MultiTarget;
        base.Start();
        abilityCheckPoints = 0;
        range = 30;

        abilityCheckPointsMax = 3;

        skillDamageBaseModifier = 0.7f;

        abilityName = "Lightning Strike";
        abilityDescription = "Chooses Targets and deals damage";
    }

    public override void ApplyAbilityEffect()
    {
        foreach (GameObject curTar in currentTargets)
        {
            if (curTar.GetComponentInChildren<DefaultToken>() != null)
            {
                float sourceDamage = skillDamageBaseModifier * 80;
                BattleManager.instance.DealDamage(curTar.GetComponentInChildren<DefaultToken>().gameObject, PlayerObject.instance.gameObject, sourceDamage);
            }
        }

        Debug.Log("Triggering Card Ability!");

        base.ApplyAbilityEffect();
    }
}
