using UnityEngine;

public class Ability_GenerateFood : PlayerTokenAbilityPrefab
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

        abilityName = "Generate Food";
        abilityDescription = "Generates a small amount of food each turn";
    }

    public override void ApplyPassiveTriggerEffect()
    {
        base.ApplyPassiveTriggerEffect();
        ResourceManager.instance.AddOrRemoveResources(0, 0, 2, 0, transform.parent.gameObject);
    }
}
