using System.Collections.Generic;
using UnityEngine;

public class CardAbility_ReduceCost: PlayerTokenAbilityPrefab
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

        abilityName = "Reduce Cost";
        abilityDescription = "Reduces the cost of target card to zero. ";
    }

    public override void ApplyAbilityEffect()
    {
        CardManager.instance.OpenCardSelectionMenue("Reduce Cost");
        base.ApplyAbilityEffect();
    }
}
