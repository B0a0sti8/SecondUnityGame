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
        float dmgHeal = myEnemyToken.baseDmgHealVal;              // Hole dir den Grundschaden
        float myModAdd = 0f;
        foreach (float modAdd in myEnemyToken.dmgHealModifiersAdd) myModAdd += modAdd; // Addiere alle additiven Modifikatoren
        dmgHeal *= 1 + myModAdd;                                                            // Anwenden
        foreach (float modMult in myEnemyToken.dmgHealModifiersMult) dmgHeal *= 1 + modMult; // Alle multiplikativen Modifikatoren anwenden

        finalDamage = Mathf.Clamp(0, dmgHeal * skillDmgHealModifier, Mathf.Infinity);
    }
}
