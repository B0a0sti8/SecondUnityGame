using UnityEngine;

public class EnemySkill_Slash : EnemySkillPrefab
{
    int baseDamage = 3;

    protected override void Start()
    {
        base.Start();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        //Debug.Log(myEnemyToken);
        Debug.Log("Using Slash on: " + myEnemyToken.currentMainTarget);
        BattleManager.instance.DealDamage(myEnemyToken.currentMainTarget, myEnemyToken.gameObject, baseDamage);
        BattleManager.instance.ShowSkillEffect(skillSprite, myEnemyToken.currentMainTarget.transform.position, 1, 0.1f);
    }
}
