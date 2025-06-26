using UnityEngine;

public class EnemySkill_Slash : EnemySkillPrefab
{
    protected override void Start()
    {
        base.Start();
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
        BattleManager.instance.DealDamage(myEnemyToken.currentMainTarget, myEnemyToken.gameObject, finalDamage);
        BattleManager.instance.ShowSkillEffect(skillSprite, myEnemyToken.currentMainTarget.transform.position, 1, 0.1f);
    }
}
