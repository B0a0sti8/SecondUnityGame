using UnityEngine;

public class CardAbility_Heal : PlayerTokenAbilityPrefab
{
    protected override void Start()
    {
        myTargetType = TargetType.MultiTarget;
        base.Start();
        abilityCheckPoints = 0;
        range = 30;

        abilityCheckPointsMax = 3;

        skillEffectModifier = 0.2f;

        abilityName = "Heal";
        abilityDescription = "Heals a number of target units. ";
    }

    public override void ApplyAbilityEffect()
    {
        Debug.Log("Ability Effekt");
        DealDamageHealing();
        base.ApplyAbilityEffect();
    }

    public override void DealDamageHealing()
    {
        base.DealDamageHealing();
        Debug.Log("Ability Effek2t");
        foreach (GameObject curTar in currentTargets)
        {
            Debug.Log("Ability Effekt3");
            if (curTar.GetComponentInChildren<DefaultToken>() != null)
            {
                BattleManager.instance.DealDamageOrHealing(curTar.GetComponentInChildren<DefaultToken>().gameObject, PlayerObject.instance.gameObject, -finalDmgHeal);
                Debug.Log("Healing Target: " + -finalDmgHeal);
            }
        }
    }
}
