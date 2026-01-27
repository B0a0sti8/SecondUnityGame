using System.Collections.Generic;
using UnityEngine;

public class CardAbility_ExtraStone : PlayerTokenAbilityPrefab
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

        abilityName = "Extra Stone";
        abilityDescription = "Grants 1 stone. ";
    }

    public override void ApplyAbilityEffect()
    {
        ResourceManager.instance.AddOrRemoveResources(0, 1, 0, 0);

        base.ApplyAbilityEffect();
    }
}
