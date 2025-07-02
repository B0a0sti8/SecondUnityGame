using UnityEngine;

public class EnemySkillPrefab : MonoBehaviour
{
    public Sprite abilitySprite;
    public string abilityName;
    public string abilityDescription;

    protected EnemyToken myEnemyToken;
    public float skillDmgHealModifier;
    public float finalDamage;

    public bool isPassive = false;

    protected virtual void Start()
    {
        myEnemyToken = transform.parent.parent.GetComponent<EnemyToken>();
    }

    public virtual void UseSkill()
    {

    }

    public virtual void DealDamageHealing()
    {
        float dmgHeal = myEnemyToken.dmgHealVal.GetValue();              // Hole dir den Grundschaden
        finalDamage = Mathf.Clamp(0, dmgHeal * skillDmgHealModifier, Mathf.Infinity);
    }
}
