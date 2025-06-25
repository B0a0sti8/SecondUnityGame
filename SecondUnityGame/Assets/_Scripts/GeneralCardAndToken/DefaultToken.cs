using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DefaultToken : MonoBehaviour
{
    public int maxLife;
    public int currentLife;

    public int attackValue;

    public GameObject healthBar;

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
}
