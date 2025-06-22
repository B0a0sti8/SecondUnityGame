using UnityEngine;

public class Ability_RemoveBuilding : PlayerTokenAbilityPrefab
{
    protected override void Start()
    {
        base.Start();
        abilityCheckPoints = 0;
        range = 0;
        myTargetType = TargetType.NoTargetNeeded;
        abilityCheckPointsMax = 0;

        abilityName = "Remove Building";
        abilityDescription = "Does what it says.";
    }

    public override void ApplyAbilityEffect()
    {
        Destroy(transform.parent.parent.gameObject);

        base.ApplyAbilityEffect();
    }
}
