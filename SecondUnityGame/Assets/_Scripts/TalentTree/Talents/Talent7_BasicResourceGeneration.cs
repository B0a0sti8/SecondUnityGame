using System;
using UnityEngine;

public class Talent7_BasicResourceGeneration : Talent
{
    protected override void Start()
    {
        talentName = "Basic Resource Generation";
        talentDescription = "Gain an amount (1 per level) of randomly distributed ressources each turn. ";

        maxCount = 5;
    }

    public override void ActivateTalentEffect()
    {
        base.ActivateTalentEffect();
        if (currentCount == 1)
        {
            TurnAndEnemyManager.instance.OnPlayerTurnStart += TalentTreeManager.instance.Talent7_BasicRessourceGeneration;
            Debug.Log("Adding listener");
        }
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffect();
        if (currentCount == 0)
        {
            TurnAndEnemyManager.instance.OnPlayerTurnStart -= TalentTreeManager.instance.Talent7_BasicRessourceGeneration;
            Debug.Log("Removing listener");
        }
    }
}