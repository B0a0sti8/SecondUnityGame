using System.Collections.Generic;
using UnityEngine;

public class CardAbility_CloneCard : PlayerTokenAbilityPrefab
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

        abilityName = "Clone Card";
        abilityDescription = "Dublicates a card in your hand. ";
    }

    public override void ApplyAbilityEffect()
    {
        CardManager.instance.OpenCardSelectionMenue("Duplicate");
        base.ApplyAbilityEffect();
    }
}
