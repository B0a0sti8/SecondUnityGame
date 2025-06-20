using UnityEngine;

public class EnemySkill_Slash : EnemySkillPrefab
{
    int skillDamageBaseModifier = 2;

    protected override void Start()
    {
        base.Start();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        //Debug.Log(myEnemyToken);
        //Debug.Log("Using Slash on: " + myEnemyToken.currentMainTarget);

        float sourceDamage = myEnemyToken.attackValue * skillDamageBaseModifier;

        BattleManager.instance.DealDamage(myEnemyToken.currentMainTarget, myEnemyToken.gameObject, sourceDamage);
        BattleManager.instance.ShowSkillEffect(skillSprite, myEnemyToken.currentMainTarget.transform.position, 1, 0.1f);
    }
}
