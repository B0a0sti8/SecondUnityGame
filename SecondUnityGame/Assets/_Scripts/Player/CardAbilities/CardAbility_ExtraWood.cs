using System.Collections.Generic;
using UnityEngine;

public class CardAbility_ExtraWood : PlayerTokenAbilityPrefab
{
    float myBaseBuffStrengthMod;

    protected override void Start()
    {
        myTargetType = TargetType.NoTargetNeeded;
        base.Start();
        abilityCheckPoints = 0;
        range = 30;

        abilityCheckPointsMax = 0;

        skillEffectModifier = 0.0f;

        abilityName = "Extra Wood";
        abilityDescription = "Grants 1 wood. ";
    }

    public override void ApplyAbilityEffect()
    {
        ResourceManager.instance.AddOrRemoveResources(1, 0, 0, 0);

        base.ApplyAbilityEffect();
    }
}
