using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void DealDamage(GameObject target, GameObject source, int damageAmount)
    {
        Debug.Log(source + " deals damage to " + target);

        // Check if target is Playertoken
        if (target.GetComponentInChildren<PlayerToken>() != null)
        {
            Debug.Log("Treffaaa");
            target.GetComponentInChildren<PlayerToken>().TakeDamage(damageAmount);
        }
        // Check if target is EnemyToken
        else if (target.GetComponentInChildren<EnemyToken>() != null)
        {
            target.GetComponentInChildren<EnemyToken>().TakeDamage(damageAmount);
        }
    }

    public void DealHealing()
    {

    }
}
