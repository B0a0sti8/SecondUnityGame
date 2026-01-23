using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalentTree : MonoBehaviour
{
    [SerializeField] int remainingTPoints = 500, maxTPoints = 500;

    [SerializeField] List<Talent> talents, unlockedByDefault;

    Dictionary<string, int> talentPointCount = new Dictionary<string, int>();

    [SerializeField] TextMeshProUGUI talentPointText;
    Transform talentTreeContent;

    void Start()
    {
        talentTreeContent = transform.Find("Mask").Find("TalentTreeContent").Find("Talents");
        talentPointText = transform.Find("AdditionalContent").Find("TalentInspector").Find("ForeGround").Find("PointCounterText").GetComponent<TextMeshProUGUI>();
        transform.Find("AdditionalContent").Find("TalentInspector").Find("ForeGround").Find("ResetTalents").GetComponent<Button>().onClick.AddListener(() => ResetTalents());

        FetchAllTalents();
        ResetTalents();
    }

    public void TryUseTalent(Talent talent)
    {
        if (remainingTPoints > talent.pointCost && talent.TryAllocateTalent())
        {
            remainingTPoints -= talent.pointCost;
            talentPointCount[talent.talentName] += 1;
            CheckUnlockTalent();
        }
        UpdateTalentPointText();
    }

    void CheckUnlockTalent()
    {
        foreach (Talent tal in talents)
        {
            // Unlock all talents, where the predecessors have been skilled at least once.
            bool shouldUnlock = true;
            foreach (Talent predTal in tal.myPredecessorTalents)
            {
                if (predTal.currentCount == 0) shouldUnlock = false;
            }
            if (shouldUnlock) tal.Unlock();

            // Mark all connections whose talent has been skilled at least once.
            foreach (GameObject con in tal.myDescendantConnections)
            {
                Image sprite = con.transform.Find("Fill").GetComponent<Image>();
                Color myCol = sprite.color;

                if (tal.currentCount > 0) myCol.a = 1;
                else myCol.a = 70f / 255f;
                sprite.color = myCol;
            }
        }
    }

    public void UpdateTalentPointText()
    {
        talentPointText.text = remainingTPoints.ToString() + " / " + maxTPoints.ToString();
    }

    void FetchAllTalents()
    {
        talents.Clear();
        talents = talentTreeContent.GetComponentsInChildren<Talent>().ToList();
        talents.RemoveAll(item => item == null);
    }

    public void ResetTalents()
    {
        remainingTPoints = maxTPoints;

        GetUnlockedByDefaultTalents();

        foreach (Talent talent in talents)
        {
            bool hasToDoEverything = true;
            if (talent != null)
            {
                if (talent.currentCount == 0) hasToDoEverything = false;

                talent.InitRefs();
                talent.Lock();
                if (hasToDoEverything) talent.RemoveActiveTalentEffect();
                talent.currentCount = 0;
                if (hasToDoEverything) talent.RemoveActiveTalentEffectAfterPointCountReduced();
                talent.UpdatePointCounterAndBackground();
            }
        }

        foreach (Talent talent in unlockedByDefault) talent.Unlock();

        talentPointCount.Clear();
        foreach (Talent talent in unlockedByDefault) talentPointCount.Add(talent.name, 0);

        UpdateTalentPointText();
    }

    void GetUnlockedByDefaultTalents()
    {
        unlockedByDefault.Clear();
        foreach (Talent tal in talents)
        {
            if (tal.isUnlockedByDefault) unlockedByDefault.Add(tal);
        }
    }

    public Dictionary<string,int> GetSkillPointsInTalents()
    {
        talentPointCount.Clear();
        foreach (Talent ta in talents) talentPointCount.Add(ta.talentName, ta.currentCount);
        return talentPointCount;
    }

    public List<Talent> GetTalentsForSaving()
    {
        FetchAllTalents();
        return talents;
    }

    public void AutoSkillWhenLoading(List<int> loadedTalentCount)
    {
        Debug.Log(loadedTalentCount.Count);
        Debug.Log(talents.Count);

        bool isWindowActive = transform.Find("TalentTreeWindow").gameObject.activeSelf;
        if (!isWindowActive) transform.Find("TalentTreeWindow").gameObject.SetActive(true);

        for (int i = 0; i < talents.Count-1; i++)
        {
            int myCurCount = loadedTalentCount[i];
            if (myCurCount == 0) continue;

            for (int k = 0; k < myCurCount; k++)
            {
                TryUseTalent(talents[i]);
            }
        }

        transform.Find("TalentTreeWindow").gameObject.SetActive(isWindowActive);
    }
}