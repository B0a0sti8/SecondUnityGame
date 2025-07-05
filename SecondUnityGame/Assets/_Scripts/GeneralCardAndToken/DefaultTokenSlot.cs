using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class DefaultTokenSlot : MonoBehaviour
{
    public GameObject myBackground;
    public GameObject myIsMarkedObject;
    public int markedCount;
    public bool isBuildingSlot;

    Transform tokenPreviewWindow;
    bool isPreviewUpdated;

    public Sprite areaModifiedSprite;
    public Buff areaModification;
    public float areaModStrength;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        myBackground = transform.Find("Background").gameObject;
        Color myColor = myBackground.GetComponent<SpriteRenderer>().color;
        myColor.a = 0.4f;
        myBackground.GetComponent<SpriteRenderer>().color = myColor;

        myIsMarkedObject = transform.Find("MarkedIndicator").gameObject;

        tokenPreviewWindow = MainCanvasSingleton.instance.transform.Find("PreviewSlot").Find("TokenPreview");
    }

    public virtual void IncreaseMark()
    {
        markedCount += 1;
        myIsMarkedObject.SetActive(true);
        if (markedCount > 1)
        {
            myIsMarkedObject.transform.Find("Canvas").Find("Text").GetComponent<TextMeshProUGUI>().text = markedCount.ToString();
        }
    }

    public virtual void ClearMark()
    {
        markedCount = 0;
        myIsMarkedObject.transform.Find("Canvas").Find("Text").GetComponent<TextMeshProUGUI>().text = "";
        myIsMarkedObject.SetActive(false);
    }

    public void ShowTokenPreview(bool setActive)
    {
        tokenPreviewWindow.gameObject.SetActive(setActive);
    }

    public void OnMouseOver()
    {
        if (GetComponentInChildren<DefaultToken>() == null) return;
        if (isPreviewUpdated) return;
        isPreviewUpdated = true;

        DefaultToken myT = GetComponentInChildren<DefaultToken>();
        tokenPreviewWindow.Find("TokenName").GetComponent<TextMeshProUGUI>().text = myT.tokenName;
        tokenPreviewWindow.Find("TokenDescription").GetComponent<TextMeshProUGUI>().text = myT.tokenDescription;
        tokenPreviewWindow.Find("TokenIcon").GetComponent<Image>().sprite = myT.transform.Find("Picture").GetComponent<SpriteRenderer>().sprite;

        if (isBuildingSlot)
        {
            tokenPreviewWindow.Find("HealthText").GetComponent<TextMeshProUGUI>().text = " Health: - ";
            tokenPreviewWindow.Find("DamageText").GetComponent<TextMeshProUGUI>().text = "Damage: - ";
            tokenPreviewWindow.Find("EnergyText").GetComponent<TextMeshProUGUI>().text = "Energy: - ";
        }
        else if (this as PlayerUnitSlot != null)
        {
            tokenPreviewWindow.Find("HealthText").GetComponent<TextMeshProUGUI>().text = " Health: " + myT.currentLife.ToString() + " / " + myT.maxLife.GetValue().ToString();
            tokenPreviewWindow.Find("DamageText").GetComponent<TextMeshProUGUI>().text = "Damage: " + myT.ReturnCurrentDamage().ToString() + " ";
            tokenPreviewWindow.Find("EnergyText").GetComponent<TextMeshProUGUI>().text = "Energy: " + (myT as PlayerToken).currentEnergy.ToString() + " / " + (myT as PlayerToken).maxEnergy.GetValue().ToString();
        }
        else // Enemy Token
        {
            tokenPreviewWindow.Find("HealthText").GetComponent<TextMeshProUGUI>().text = " Health: " + myT.currentLife.ToString() + " / " + myT.maxLife.GetValue().ToString();
            tokenPreviewWindow.Find("DamageText").GetComponent<TextMeshProUGUI>().text = "Damage: " + myT.ReturnCurrentDamage().ToString() + " ";
            tokenPreviewWindow.Find("EnergyText").GetComponent<TextMeshProUGUI>().text = "Energy: - ";
        }

        // Set Active abilities
        for (int i = 0; i < tokenPreviewWindow.Find("Abilities Active").childCount; i++)
        {
            tokenPreviewWindow.Find("Abilities Active").GetChild(i).gameObject.SetActive(false);
        }

        if (isBuildingSlot || this as PlayerUnitSlot != null) // Ein Playerslot
        {
            List<Transform> tokenAbilities = myT.GetComponent<PlayerToken>().myAbilities;
            for (int i = 0; i < tokenAbilities.Count; i++)
            {
                if (i < 3)
                {
                    tokenPreviewWindow.Find("Abilities Active").GetChild(i).gameObject.SetActive(true);
                    tokenPreviewWindow.Find("Abilities Active").GetChild(i).Find("AbilitySprite").GetComponent<Image>().sprite = tokenAbilities[i].GetComponent<PlayerTokenAbilityPrefab>().abilityIcon;
                    tokenPreviewWindow.Find("Abilities Active").GetChild(i).Find("AbilityName").GetComponent<TextMeshProUGUI>().text = tokenAbilities[i].GetComponent<PlayerTokenAbilityPrefab>().abilityName;
                    tokenPreviewWindow.Find("Abilities Active").GetChild(i).Find("AbilityDescription").GetComponent<TextMeshProUGUI>().text = tokenAbilities[i].GetComponent<PlayerTokenAbilityPrefab>().abilityDescription;
                }
            }
        }
        else // Ein EnemyToken
        {
            List<EnemySkillPrefab> tokenAbilities = myT.GetComponent<EnemyToken>().myActiveSkillList;
            for (int i = 0; i < tokenAbilities.Count; i++)
            {
                if (i < 3)
                {
                    tokenPreviewWindow.Find("Abilities Active").GetChild(i).gameObject.SetActive(true);
                    tokenPreviewWindow.Find("Abilities Active").GetChild(i).Find("AbilitySprite").GetComponent<Image>().sprite = tokenAbilities[i].abilitySprite;
                    tokenPreviewWindow.Find("Abilities Active").GetChild(i).Find("AbilityName").GetComponent<TextMeshProUGUI>().text = tokenAbilities[i].abilityName;
                    tokenPreviewWindow.Find("Abilities Active").GetChild(i).Find("AbilityDescription").GetComponent<TextMeshProUGUI>().text = tokenAbilities[i].abilityDescription;
                }
            }
        }


        // Passive skills
        for (int i = 0; i < tokenPreviewWindow.Find("Abilities Passive").childCount; i++)
        {
            tokenPreviewWindow.Find("Abilities Passive").GetChild(i).gameObject.SetActive(false);
        }

        if (isBuildingSlot || this as PlayerUnitSlot != null) // Ein Playerslot
        {
            List<Transform> tokenAbilities = myT.GetComponent<PlayerToken>().myPassiveTriggerAbilities;
            for (int i = 0; i < tokenAbilities.Count; i++)
            {
                if (i < 3)
                {
                    tokenPreviewWindow.Find("Abilities Passive").GetChild(i).gameObject.SetActive(true);
                    tokenPreviewWindow.Find("Abilities Passive").GetChild(i).Find("AbilitySprite").GetComponent<Image>().sprite = tokenAbilities[i].GetComponent<PlayerTokenAbilityPrefab>().abilityIcon;
                    tokenPreviewWindow.Find("Abilities Passive").GetChild(i).Find("AbilityName").GetComponent<TextMeshProUGUI>().text = tokenAbilities[i].GetComponent<PlayerTokenAbilityPrefab>().abilityName;
                    tokenPreviewWindow.Find("Abilities Passive").GetChild(i).Find("AbilityDescription").GetComponent<TextMeshProUGUI>().text = tokenAbilities[i].GetComponent<PlayerTokenAbilityPrefab>().abilityDescription;
                }
            }
        }
        else // Ein EnemyToken
        {
            List<EnemySkillPrefab> tokenAbilities = myT.GetComponent<EnemyToken>().myPassiveSkillList;
            for (int i = 0; i < tokenAbilities.Count; i++)
            {
                if (i < 3)
                {
                    tokenPreviewWindow.Find("Abilities Passive").GetChild(i).gameObject.SetActive(true);
                    tokenPreviewWindow.Find("Abilities Passive").GetChild(i).Find("AbilitySprite").GetComponent<Image>().sprite = tokenAbilities[i].abilitySprite;
                    tokenPreviewWindow.Find("Abilities Passive").GetChild(i).Find("AbilityName").GetComponent<TextMeshProUGUI>().text = tokenAbilities[i].abilityName;
                    tokenPreviewWindow.Find("Abilities Passive").GetChild(i).Find("AbilityDescription").GetComponent<TextMeshProUGUI>().text = tokenAbilities[i].abilityDescription;
                }
            }
        }

        // Buffs
        for (int i = 0; i < tokenPreviewWindow.Find("Buffs").childCount; i++)
        {
            tokenPreviewWindow.Find("Buffs").GetChild(i).gameObject.SetActive(false);
        }

        List<Buff> allMyBuffs = myT.GetComponent<DefaultToken>().myCurrentBuffs;
        for (int i = 0; i < allMyBuffs.Count; i++)
        {
            if (i < 9)
            {
                tokenPreviewWindow.Find("Buffs").GetChild(i).gameObject.SetActive(true);
                tokenPreviewWindow.Find("Buffs").GetChild(i).Find("BuffSprite").GetComponent<Image>().sprite = allMyBuffs[i].buffSprite;
                tokenPreviewWindow.Find("Buffs").GetChild(i).Find("BuffName").GetComponent<TextMeshProUGUI>().text = allMyBuffs[i].buffName;
                tokenPreviewWindow.Find("Buffs").GetChild(i).Find("BuffDescription").GetComponent<TextMeshProUGUI>().text = allMyBuffs[i].buffDescription;
                tokenPreviewWindow.Find("Buffs").GetChild(i).Find("RemainingDuration").GetComponent<TextMeshProUGUI>().text = "Remaining Duration: " + allMyBuffs[i].remainingDuration.ToString() + " ";
            }
        }

        ShowTokenPreview(true);

        if (transform.GetComponentInChildren<PlayerToken>() != null)
        {
            transform.GetComponentInChildren<PlayerToken>().transform.Find("Canvas").Find("EnergyBar").gameObject.SetActive(true);
        }

        if (this as EnemyTokenSlot != null)
        {
            EnemyTokenSlot enemSlot = (EnemyTokenSlot)this;
            foreach (GameObject potTar in enemSlot.potentialTargetSlots)
            {
                potTar.GetComponent<PlayerUnitSlot>().HighlightBackground(true);
            }
        }
    }

    public void OnMouseExit()
    {
        ShowTokenPreview(false);
        isPreviewUpdated = false;

        if (transform.GetComponentInChildren<PlayerToken>() != null)
        {
            transform.GetComponentInChildren<PlayerToken>().transform.Find("Canvas").Find("EnergyBar").gameObject.SetActive(false);
        }

        if (this as EnemyTokenSlot != null)
        {
            EnemyTokenSlot enemSlot = (EnemyTokenSlot)this;
            foreach (GameObject potTar in enemSlot.potentialTargetSlots)
            {
                potTar.GetComponent<PlayerUnitSlot>().HighlightBackground(false);
            }
        }
    }

    public void OnTokenRemove()
    {
        if (areaModification != null)
        {
            if (GetComponentInChildren<DefaultToken>() != null)
            {
                BattleManager.instance.RemoveBuffFromTarget(GetComponentInChildren<DefaultToken>().gameObject, areaModification);
            }
        }
    }

    public virtual void OnTokenSet()
    {
        if (areaModification != null)
        {
            if (GetComponentInChildren<DefaultToken>() != null)
            {
                BattleManager.instance.ApplyBuffToTarget(GetComponentInChildren<DefaultToken>().gameObject, gameObject, areaModification, areaModifiedSprite, 100, areaModStrength);
            }
        }
    }

    public void SetAreaModification(Buff areaMod, Sprite areaModSpr, float areaModStr)
    {

        areaModification = areaMod;
        areaModification.buffName = "Area " + areaModification.buffName;
        areaModifiedSprite = areaModSpr;
        areaModStrength = areaModStr;

        transform.Find("AreaMod").GetComponent<SpriteRenderer>().sprite = areaModifiedSprite;
        transform.Find("AreaMod").gameObject.SetActive(true);

        OnTokenSet();
    }

    public void RemoveAreaModification()
    {
        OnTokenRemove();

        areaModification = null;
        areaModifiedSprite = null;
        areaModStrength = 0;

        transform.Find("AreaMod").gameObject.SetActive(false);
    }
}
