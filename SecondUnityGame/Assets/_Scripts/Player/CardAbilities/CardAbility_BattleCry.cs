using System.Collections.Generic;
using UnityEngine;

public class CardAbility_BattleCry : PlayerTokenAbilityPrefab
{
    float myBaseBuffStrengthMod;

    protected override void Start()
    {
        myTargetType = TargetType.NoTargetNeeded;
        base.Start();
        abilityCheckPoints = 0;
        range = 30;

        abilityCheckPointsMax = 0;

        skillDmgHealModifier = 0.0f;
        myBaseBuffStrengthMod = 1.5f;

        abilityName = "Battle Cry";
        abilityDescription = "Increases the damage of all units by 10 % for 2 turns. Gives 1 Energy to each unit.";
    }

    public override void ApplyAbilityEffect()
    {
        Debug.Log("Triggering Card Ability!");

        float myBuffStrengthMod = myBaseBuffStrengthMod;

        List<PlayerToken> allPlayerT = new List<PlayerToken>();
        TurnAndEnemyManager.instance.allPlayerSlotsWithTokens.ForEach(x => allPlayerT.Add(x.GetComponent<PlayerToken>()));
        foreach (PlayerToken pTok in allPlayerT)
        {
            pTok.ManageEnergy(-1);
            BattleManager.instance.ApplyBuffToTarget(pTok.gameObject, PlayerObject.instance.gameObject, myBuff, abilityIcon, 2, myBuffStrengthMod);
        }

        base.ApplyAbilityEffect();
    }
}
