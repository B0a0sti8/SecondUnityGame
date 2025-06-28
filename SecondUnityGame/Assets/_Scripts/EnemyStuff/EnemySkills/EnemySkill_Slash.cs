using UnityEngine;

public class EnemySkill_Slash : EnemySkillPrefab
{
    protected override void Start()
    {
        base.Start();
        abilityName = "Slash";
        abilityDescription = "Hits a single target for low damage";
        skillDmgHealModifier = 2;
    }

    public override void UseSkill()
    {
        base.UseSkill();
        DealDamageHealing();
    }

    public override void DealDamageHealing()
    {
        base.DealDamageHealing(); // hier findet die berechung des schadens statt.
        BattleManager.instance.DealDamageOrHealing(myEnemyToken.currentMainTarget, myEnemyToken.gameObject, finalDamage);
        BattleManager.instance.ShowSkillEffect(abilitySprite, myEnemyToken.currentMainTarget.transform.position, 1, 0.1f);
    }
}
