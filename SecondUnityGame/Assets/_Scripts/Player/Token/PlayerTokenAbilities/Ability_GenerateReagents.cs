using UnityEngine;

public class Ability_GenerateReagents : PlayerTokenAbilityPrefab
{
    protected override void Start()
    {
        isPassiveTriggerAbility = true;
        myTargetType = TargetType.NoTargetNeeded;
        base.Start();
        energyCost = 0;
        abilityCheckPoints = 0;
        range = 0;

        abilityCheckPointsMax = 0;

        skillEffectModifier = 0f;

        abilityName = "Generate Reagents";
        abilityDescription = "Generates a small amount of reagents each turn";
    }

    public override void ApplyPassiveTriggerEffect()
    {
        base.ApplyPassiveTriggerEffect();
        RessourceManager.instance.AddOrRemoveResources(0, 0, 0, 2, transform.parent.gameObject);
    }
}
