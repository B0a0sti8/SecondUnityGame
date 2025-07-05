using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyToken : DefaultToken
{ 
    // Dinge die im Tooltipp angezeigt werden
    public string cardName;
    public string description;
    public Sprite unitTypeSprite;
    public Sprite tokenSprite;
    
    public EnemyTokenSlot mySlot;
    public GameObject currentMainTarget;
    public List<EnemySkillPrefab> myActiveSkillList = new List<EnemySkillPrefab>();
    public List<EnemySkillPrefab> myPassiveSkillList = new List<EnemySkillPrefab>();

    GameObject myNoTargetMarker;

    public override void Start()
    {
        base.Start();

        currentLife = maxLife.GetValue();

        mySlot = transform.parent.GetComponent<EnemyTokenSlot>();
        for (int i = 0; i < transform.Find("EnemySkills").childCount; i++)
        {
            if (transform.Find("EnemySkills").GetChild(i).GetComponent<EnemySkillPrefab>().isPassive) myPassiveSkillList.Add(transform.Find("EnemySkills").GetChild(i).GetComponent<EnemySkillPrefab>());
            else myActiveSkillList.Add(transform.Find("EnemySkills").GetChild(i).GetComponent<EnemySkillPrefab>());
        }

        healthBar = transform.Find("Canvas").Find("Healthbar").Find("Health").gameObject;
        myNoTargetMarker = transform.Find("NoTargetMarker").gameObject;
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
            float dmgHeal = dmgHealVal.GetValue();              // Hole dir den Schaden
            BattleManager.instance.DealDamageToPlayer(gameObject, dmgHeal);
            //Debug.Log("Dealing damage to player");
        }
        else
        {
            int choice = Random.Range(0, potentialTargets.Count);
            currentMainTarget = potentialTargets[choice];
            //if (potentialTargets.Count > 0) Debug.Log("Found viable target!!");

            //Debug.Log("Habe so viele Skills: " + mySkillList.Count);
            if (myActiveSkillList.Count > 0)
            {
                int skillChoice = Random.Range(0, myActiveSkillList.Count);
                myActiveSkillList[skillChoice].UseSkill();
            }

        }
    }

    public void SearchTargetsAndShowWarning()
    {
        List<GameObject> potentialTargets = new List<GameObject>();
        for (int i = 0; i < mySlot.potentialTargetSlots.Count; i++)
        {
            if (mySlot.potentialTargetSlots[i].GetComponentInChildren<PlayerToken>() != null)
            {
                potentialTargets.Add(mySlot.potentialTargetSlots[i]);
            }
        }

        if (potentialTargets.Count == 0) myNoTargetMarker.SetActive(true);
        else myNoTargetMarker.SetActive(false);
    }

    public void HighlightTargetSlots()
    {

    }
}
