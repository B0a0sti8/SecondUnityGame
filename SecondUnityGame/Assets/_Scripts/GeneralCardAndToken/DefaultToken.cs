using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefaultToken : MonoBehaviour
{
    public string tokenName;
    public string tokenDescription;

    public Stat maxLife = new Stat();
    public float maxLifeBase;
    public float currentLife;

    public Stat maxEnergy = new Stat();
    public int maxEnergyBase;
    public int currentEnergy;

    public Stat dmgHealVal = new Stat();
    public int baseDmgHealVal;

    public Stat receiveDmgHealVal = new Stat();
    public int baseRecDmgHealVal = 1;

    //public List<float> dmgHealModifiersAdd;
    //public List<float> dmgHealModifiersMult;

    //public List<float> resistanceModifiersAdd;
    //public List<float> resistanceModifiersMult;

    public GameObject healthBar;

    public List<Buff> myCurrentBuffs;
    public GameObject myBuffUI;

    public virtual void Start()
    {
        myBuffUI = transform.Find("Canvas").Find("Buffs").gameObject;
        InitBaseStats();
    }

    void InitBaseStats()
    {
        if (this as EnemyToken != null)
        {
            maxLife.baseValue = maxLifeBase;
            maxEnergy.baseValue = maxEnergyBase;
            dmgHealVal.baseValue = baseDmgHealVal;
            if (baseRecDmgHealVal == 0) receiveDmgHealVal.baseValue = 1;
            else receiveDmgHealVal.baseValue = baseRecDmgHealVal;
        }
    }

    public virtual void TakeDamageOrHealing(float DamageAmount)
    {
        currentLife -= DamageAmount;
        UpdateHealthbar();
        if (currentLife <= 0) Die();
    }

    public virtual void UpdateHealthbar()
    {
        healthBar.GetComponent<Image>().fillAmount = (float)currentLife / (float)maxLife.GetValue();
    }

    public void UpdateBuffUI()
    {
        for (int i = 0; i < myBuffUI.transform.childCount; i++)
        {
            myBuffUI.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = myCurrentBuffs.Count - 1; i >= 0; i--)
        {
            if (i < 4)
            {
                myBuffUI.transform.GetChild(i).gameObject.SetActive(true);
                myBuffUI.transform.GetChild(i).Find("Image").GetComponent<Image>().sprite = myCurrentBuffs[i].buffSprite;
            }
        }
    }

    public void UpdateBuffs()
    {
        if (myCurrentBuffs.Count != 0) 
        {
            //Debug.Log("Updating Buffs: " + myCurrentBuffs.Count);
            for (int i = myCurrentBuffs.Count - 1; i >= 0; i--)
            {
                Debug.Log(i);
                myCurrentBuffs[i].TriggerBuffEffectInTurn();
            }
        }
        UpdateBuffUI();
    }

    public void Die()
    {
        StartCoroutine((RemoveToken(0.2f)));
    }

    IEnumerator RemoveToken(float time)
    {
        yield return new WaitForSeconds(time);
        if (TurnAndEnemyManager.instance.allPlayerSlotsWithTokens.Contains(gameObject)) TurnAndEnemyManager.instance.allPlayerSlotsWithTokens.Remove(gameObject);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (TurnAndEnemyManager.instance.allPlayerSlotsWithTokens.Contains(gameObject)) TurnAndEnemyManager.instance.allPlayerSlotsWithTokens.Remove(gameObject);
    }

    public int ReturnCurrentDamage()
    {
        float dmgHeal = dmgHealVal.GetValue();        // Hole dir den Grundschaden

        float finalDamage = Mathf.Clamp(0, dmgHeal, Mathf.Infinity);
        return (int)finalDamage;
    }
}
