using UnityEngine;

public class Talent4_StartingResources : Talent
{
    protected override void Start()
    {
        talentName = "Starting Resources";
        talentDescription = "Grants 1 wood and 1 stone per level at the start of an expedition. At highest level also grants 1 food and reagents";
    }

    public override void ActivateTalentEffect()
    {
        base.ActivateTalentEffect();
        //TurnAndEnemyManager.instance.OnPlayerTurnStart += GenerateWoodAndStone;
        TurnAndEnemyManager.instance.OnPlayerTurnStart += TalentTreeManager.instance.Talent7_BasicRessourceGeneration;
        Debug.Log("Adding listener");
    }

    public override void RemoveActiveTalentEffect()
    {
        base.RemoveActiveTalentEffect();
        //TurnAndEnemyManager.instance.OnPlayerTurnStart += GenerateWoodAndStone;
        TurnAndEnemyManager.instance.OnPlayerTurnStart -= TalentTreeManager.instance.Talent7_BasicRessourceGeneration;
        Debug.Log("Removing listener");
    }
}
