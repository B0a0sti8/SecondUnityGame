using UnityEngine;

public class CardAbility_Blank : PlayerTokenAbilityPrefab
{
    protected override void Start()
    {
        myTargetType = TargetType.NoTargetNeeded;
        base.Start();
        abilityCheckPoints = 0;
        range = 30;

        abilityCheckPointsMax = 0;

        skillEffectModifier = 0.7f;

        abilityName = "Blank";
        abilityDescription = "Does Nothing";
    }

    public override void ApplyAbilityEffect()
    {
        base.ApplyAbilityEffect();
    }
}
