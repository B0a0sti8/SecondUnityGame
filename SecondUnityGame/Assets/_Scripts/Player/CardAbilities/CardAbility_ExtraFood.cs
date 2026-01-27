using System.Collections.Generic;
using UnityEngine;

public class CardAbility_ExtraFood : PlayerTokenAbilityPrefab
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

        abilityName = "Extra Food";
        abilityDescription = "Grants 1 food. ";
    }

    public override void ApplyAbilityEffect()
    {
        ResourceManager.instance.AddOrRemoveResources(0, 0, 1, 0);

        base.ApplyAbilityEffect();
    }
}
