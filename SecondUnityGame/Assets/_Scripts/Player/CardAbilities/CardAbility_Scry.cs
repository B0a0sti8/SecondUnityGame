using System.Collections.Generic;
using UnityEngine;

public class CardAbility_Scry : PlayerTokenAbilityPrefab
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

        abilityName = "Scry";
        abilityDescription = "Scry two cards. ";
    }

    public override void ApplyAbilityEffect()
    {
        CardManager.instance.ScryCards(2);
        base.ApplyAbilityEffect();
    }
}
