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

        skillDmgHealModifier = 0.7f;

        abilityName = "Lightning Strike";
        abilityDescription = "Chooses Targets and deals damage";
    }

    public override void ApplyAbilityEffect()
    {
        DealDamageHealing();
        Debug.Log("Triggering Card Ability!");

        base.ApplyAbilityEffect();
    }

    public override void DealDamageHealing()
    {
        base.DealDamageHealing();

        foreach (GameObject curTar in currentTargets)
        {
            if (curTar.GetComponentInChildren<DefaultToken>() != null)
            {
                BattleManager.instance.DealDamageOrHealing(curTar.GetComponentInChildren<DefaultToken>().gameObject, PlayerObject.instance.gameObject, finalDamage);
            }
        }
    }
}
