using System.Collections.Generic;
using UnityEngine;

public class CardAbility_Discard : PlayerTokenAbilityPrefab
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

        abilityName = "Discard";
        abilityDescription = "Draw two cards, then discard one card. ";
    }

    public override void ApplyAbilityEffect()
    {
        CardManager.instance.OpenCardSelectionMenue("Discard");
        base.ApplyAbilityEffect();
    }
}
