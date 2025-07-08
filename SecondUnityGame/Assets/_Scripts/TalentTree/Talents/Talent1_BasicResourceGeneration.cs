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
        //TurnAndEnemyManager.instance.OnPlayerTurnStart += GenerateWoodAndStone;
        TurnAndEnemyManager.instance.OnPlayerTurnStart += TalentTreeManager.instance.GenerateWoodAndStone;
        Debug.Log("Adding listener");
    }

    public override void RemoveActiveTalentEffect()
    {
        base.RemoveActiveTalentEffect();
        //TurnAndEnemyManager.instance.OnPlayerTurnStart += GenerateWoodAndStone;
        TurnAndEnemyManager.instance.OnPlayerTurnStart -= TalentTreeManager.instance.GenerateWoodAndStone;
        Debug.Log("Removing listener");
    }
}