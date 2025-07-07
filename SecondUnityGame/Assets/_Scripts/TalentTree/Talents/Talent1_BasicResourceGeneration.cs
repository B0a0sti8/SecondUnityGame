using System;
using UnityEngine;

public class Talent1_BasicResourceGeneration : Talent
{
    protected override void Start()
    {
        talentName = "Basic Resource Generation";
        talentDescription = "Gain one wood and one stone each turn. ";
    }

    public override void ActivateTalentEffect()
    {
        base.ActivateTalentEffect();
        TurnAndEnemyManager.instance.OnPlayerTurnStart += GenerateWoodAndStone;
    }

    private void GenerateWoodAndStone(object sender, EventArgs e)
    {
        RessourceManager.instance.AddOrRemoveResources(1, 1, 0, 0, null);
        Debug.Log("Trigger");
    }
}
