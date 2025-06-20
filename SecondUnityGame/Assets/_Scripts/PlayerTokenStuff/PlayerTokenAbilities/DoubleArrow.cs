using UnityEngine;

public class DoubleArrow : PlayerTokenAbilityPrefab
{
    protected override void Start()
    {
        base.Start();
        energyCost = 1;
        abilityCheckPoints = 2;
        range = 10;

    }


}
