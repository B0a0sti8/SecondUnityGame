using UnityEngine;

public class Ability_GenerateWood : PlayerTokenAbilityPrefab
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

        abilityName = "Generate Wood";
        abilityDescription = "Generates a small amount of wood each turn";
    }

    public override void ApplyPassiveTriggerEffect()
    {
        base.ApplyPassiveTriggerEffect();
        ResourceManager.instance.AddOrRemoveResources(2, 0, 0, 0, transform.parent.gameObject);
    }
}
