using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefaultToken : MonoBehaviour
{
    public string tokenName;
    public string tokenDescription;

    public int maxLife;
    public int currentLife;

    public float baseDmgHealVal;
    public List<float> dmgHealModifiersAdd;
    public List<float> dmgHealModifiersMult;

    public List<float> resistanceModifiersAdd;
    public List<float> resistanceModifiersMult;

    public GameObject healthBar;

    public List<Buff> myCurrentBuffs;
    public GameObject myBuffUI;

    public virtual void Start()
    {
        myBuffUI = transform.Find("Canvas").Find("Buffs").gameObject;
    }

    public virtual void TakeDamageOrHealing(int DamageAmount)
    {
        currentLife -= DamageAmount;
        UpdateHealthbar();

        //Debug.Log("currentlife = " + currentLife);

        if (currentLife <= 0) Die();
    }

    public virtual void UpdateHealthbar()
    {
        healthBar.GetComponent<Image>().fillAmount = (float)currentLife / (float)maxLife;
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
                Debug.Log("my BuffName = " + myCurrentBuffs[i].buffName);
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
        float dmgHeal = baseDmgHealVal;        // Hole dir den Grundschaden

        float myModAdd = 0f;
        foreach (float modAdd in dmgHealModifiersAdd) myModAdd += modAdd; // Addiere alle additiven Modifikatoren
        dmgHeal *= 1 + myModAdd;                                                            // Anwenden
        foreach (float modMult in dmgHealModifiersMult) dmgHeal *= 1 + modMult; // Alle multiplikativen Modifikatoren anwenden

        float finalDamage = Mathf.Clamp(0, dmgHeal, Mathf.Infinity);
        return (int)finalDamage;
    }
}
