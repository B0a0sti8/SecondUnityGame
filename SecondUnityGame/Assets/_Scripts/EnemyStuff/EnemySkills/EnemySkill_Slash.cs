using UnityEngine;

public class EnemySkill_Slash : EnemySkillPrefab
{
    protected override void Start()
    {
        base.Start();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        //Debug.Log(myEnemyToken);
        Debug.Log("Using Slash on: " + myEnemyToken.currentMainTarget);
        BattleManager.instance.DealDamage(myEnemyToken.currentMainTarget, myEnemyToken.gameObject);
    }
}
