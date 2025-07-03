using UnityEngine;

public class CardAbility_GatherKnowledge : PlayerTokenAbilityPrefab
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

        abilityName = "Gather Knowledge";
        abilityDescription = "Gathers lost knowledge (1) of the Continent. Gathering enough, will win the stage. Knowledge is used to improve cards and abilities. ";
    }

    public override void ApplyAbilityEffect()
    {
        RessourceManager.instance.AddKnowledge(1);
        base.ApplyAbilityEffect();
    }
}
