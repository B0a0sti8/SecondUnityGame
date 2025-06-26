using UnityEngine;

public class EnemySkillPrefab : MonoBehaviour
{
    [SerializeField] protected Sprite skillSprite;
    protected EnemyToken myEnemyToken;
    public float skillDmgHealModifier;
    public float finalDamage;

    protected virtual void Start()
    {
        myEnemyToken = transform.parent.parent.GetComponent<EnemyToken>();
    }

    public virtual void UseSkill()
    {

    }

    public virtual void DealDamageHealing()
    {
        float dmgHeal = myEnemyToken.baseDmgHealVal;              // Hole dir den Grundschaden
        float myModAdd = 0f;
        foreach (float modAdd in myEnemyToken.dmgHealModifiersAdd) myModAdd += modAdd; // Addiere alle additiven Modifikatoren
        dmgHeal *= 1 + myModAdd;                                                            // Anwenden
        foreach (float modMult in myEnemyToken.dmgHealModifiersMult) dmgHeal *= 1 + modMult; // Alle multiplikativen Modifikatoren anwenden

        finalDamage = Mathf.Clamp(0, dmgHeal * skillDmgHealModifier, Mathf.Infinity);
    }
}
