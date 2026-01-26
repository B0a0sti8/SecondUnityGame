using UnityEngine;

public class Talent8_UnitBaseDamage : Talent
{
    protected override void Start()
    {
        talentName = "Unit Base Damage";
        talentDescription = "Increases the base damage and healing of all your units by 1 per level. ";
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
