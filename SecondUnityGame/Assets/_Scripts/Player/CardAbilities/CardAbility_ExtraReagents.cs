using System.Collections.Generic;
using UnityEngine;

public class CardAbility_ExtraReagents : PlayerTokenAbilityPrefab
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

        abilityName = "Extra Reagents";
        abilityDescription = "Grants 1 reagents. ";
    }

    public override void ApplyAbilityEffect()
    {
        ResourceManager.instance.AddOrRemoveResources(0, 0, 0, 1);

        base.ApplyAbilityEffect();
    }
}
