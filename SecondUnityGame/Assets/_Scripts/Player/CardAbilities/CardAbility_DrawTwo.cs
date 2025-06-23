using UnityEngine;

public class CardAbility_DrawTwo : PlayerTokenAbilityPrefab
{
    protected override void Start()
    {
        myTargetType = TargetType.NoTargetNeeded;
        base.Start();
        abilityCheckPoints = 0;
        range = 30;

        abilityCheckPointsMax = 0;

        abilityName = "Draw Two";
        abilityDescription = "Draws two cards from deck";
    }

    public override void ApplyAbilityEffect()
    {
        CardManager.instance.DrawNextCardFromDeck();
        CardManager.instance.DrawNextCardFromDeck();

        base.ApplyAbilityEffect();
    }
}
