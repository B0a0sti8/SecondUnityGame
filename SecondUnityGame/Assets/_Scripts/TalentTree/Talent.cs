using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Talent : MonoBehaviour
{
    public string talentName;
    public string talentDescription = "";
    public Sprite talentIcon;

    public int maxCount;
    public int pointCost = 1;
    public int currentCount = 0;

    public List<Talent> myPredecessorTalents = new List<Talent>();
    public List<GameObject> myDescendantConnections = new List<GameObject>();
    public bool isUnlockedByDefault = false;

    private TextMeshProUGUI talentPointTextOwn;
    TalentTree myTalentTree;
    Button button;
    Image talentImageObj, backgroundSprite;

    protected virtual void Start()
    {
        InitRefs();
    }

    void OnTalentButtonClick()
    {
        myTalentTree.TryUseTalent(this);
    }

    public virtual void InitRefs()
    {
        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnTalentButtonClick);
        talentImageObj = transform.Find("TalentImage").GetComponent<Image>();
        //talentImageObj.sprite = talentIcon;
        backgroundSprite = transform.Find("Background").GetComponent<Image>();
        talentPointTextOwn = transform.Find("Counter").Find("Text").GetComponent<TextMeshProUGUI>();
        myTalentTree = transform.parent.parent.parent.parent.GetComponent<TalentTree>();
    }

    public bool TryAllocateTalent()
    {
        bool isAlloc = false;
        if (currentCount < maxCount)
        {
            currentCount++;
            Debug.Log("Activating Talent");
            ActivateTalentEffect();
            isAlloc = true;
        }
        UpdatePointCounterAndBackground();
        return isAlloc;
    }

    public void Lock()
    {
        button.interactable = false;
        Color newColor = talentImageObj.color;
        newColor.a = 0.3f;
        talentImageObj.color = newColor;
    }

    public void Unlock()
    {
        button.interactable = true;
        Color newColor = talentImageObj.color;
        newColor.a = 2;
        talentImageObj.color = newColor;
    }

    public void UpdatePointCounterAndBackground()
    {
        float alphaStep = (255 - 70) * 1 / Mathf.Clamp(maxCount, 1, 5);
        float newAlpha = (70 + alphaStep * currentCount) / 255;

        Color myNewColor = backgroundSprite.color;
        myNewColor.a = newAlpha;
        backgroundSprite.color = myNewColor;

        talentPointTextOwn.text = currentCount.ToString() + " / " + maxCount.ToString();
    }

    public virtual void ActivateTalentEffect()
    {
        //GetComponent<MasterEventTriggerTalent>().GetTalentInfo();
    }

    public virtual void RemoveActiveTalentEffect()
    {
        //GetComponent<MasterEventTriggerTalent>().GetTalentInfo();
    }

    public virtual void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        //GetComponent<MasterEventTriggerTalent>().GetTalentInfo();
    }
}
