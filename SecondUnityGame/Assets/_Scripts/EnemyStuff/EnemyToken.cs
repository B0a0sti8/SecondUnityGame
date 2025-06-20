using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyToken : MonoBehaviour
{ 
    // Dinge die im Tooltipp angezeigt werden
    public string cardName;
    public string description;
    public Sprite unitTypeSprite;

    public Sprite tokenSprite;

    // Eigenschafte und Werte für Kampf
    public int maxEnergy;
    public int maxLife;
    public int currentLife;
    public int attackValue;
    public int attackRange;

    EnemyTokenSlot mySlot;
    public GameObject currentMainTarget;
    List<EnemySkillPrefab> mySkillList = new List<EnemySkillPrefab>();

    public GameObject healthBar;

    private void Start()
    {
        mySlot = transform.parent.GetComponent<EnemyTokenSlot>();
        mySlot.hasToken = true;
        for (int i = 0; i < transform.Find("EnemySkills").childCount; i++)
        {
            mySkillList.Add(transform.Find("EnemySkills").GetChild(i).GetComponent<EnemySkillPrefab>());
        }

        healthBar = transform.Find("Canvas").Find("Healthbar").Find("Health").gameObject;
    }

    public void EvaluateBuffsAndDebuffs()
    {
        //Debug.Log("Evaluating Debuffs");
    }

    public void ChooseTargetsAndUseSkills()
    {
        //Debug.Log("Choosing Target");
        List<GameObject> potentialTargets = new List<GameObject>();

        for (int i = 0; i < mySlot.potentialTargetSlots.Count; i++)
        {
            if (mySlot.potentialTargetSlots[i].GetComponentInChildren<PlayerToken>() != null)
            {
                potentialTargets.Add(mySlot.potentialTargetSlots[i]);
            }
        }

        if (potentialTargets.Count == 0)
        {
            // Wenn er keine Ziele findet, soll er den Spieler angreifen.
        }
        else
        {
            int choice = Random.Range(0, potentialTargets.Count);
            currentMainTarget = potentialTargets[choice];
            //if (potentialTargets.Count > 0) Debug.Log("Found viable target!!");

            //Debug.Log("Habe so viele Skills: " + mySkillList.Count);
            if (mySkillList.Count > 0)
            {
                int skillChoice = Random.Range(0, mySkillList.Count);
                mySkillList[skillChoice].UseSkill();
            }

        }
    }

    public void TakeDamageOrHealing(int DamageAmount)
    {
        currentLife -= DamageAmount;
        UpdateHealthbar();

        if (currentLife <= 0) Die();
    }

    public void UpdateHealthbar()
    {
        healthBar.GetComponent<Image>().fillAmount = (float)currentLife / (float)maxLife;
    }


    public void Die()
    {
        StartCoroutine((RemoveEnemy(0.2f)));
    }

    IEnumerator RemoveEnemy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
