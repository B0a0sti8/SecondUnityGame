using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultToken : MonoBehaviour
{
    public int maxLife;
    public int currentLife;

    public float baseDmgHealVal;
    public List<float> dmgHealModifiersAdd;
    public List<float> dmgHealModifiersMult;

    public GameObject healthBar;

    public List<Buff> myCurrentBuffs;

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
}
