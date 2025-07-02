using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    public static PlayerObject instance;
    public Stat dmgHealVal = new Stat();
    public Stat buffEffectMod = new Stat();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        dmgHealVal.baseValue = 50;
        buffEffectMod.baseValue = 1;
    }

    public float GetDamageHealValue()
    {
        float finalDamage = dmgHealVal.GetValue();
        return finalDamage;
    }

    public float GetBuffEffektModifier()
    {
        float buffEffectM = buffEffectMod.GetValue();
        return buffEffectM;
    }
}
