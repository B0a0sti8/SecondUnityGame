using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void DealDamage(GameObject target, GameObject source)
    {
        Debug.Log(source + " deals damage to " + target);
    }

    public void DealHealing()
    {

    }
}
