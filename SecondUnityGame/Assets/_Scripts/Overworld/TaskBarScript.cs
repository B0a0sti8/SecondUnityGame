using UnityEngine;
using UnityEngine.UI;

public class TaskBarScript : MonoBehaviour
{
    Transform mainCan;

    private void Start()
    {
        mainCan = MainCanvasSingleton.instance.transform;

        transform.Find("DeckBuilding").Find("DeckBuilderButton").GetComponent<Button>().onClick.AddListener(() => OpenCloseDeckBuilder());
        transform.Find("TalentTree").Find("TalentTreeButton").GetComponent<Button>().onClick.AddListener(() => OpenCloseSkillTree());
    }


    public void OpenCloseSkillTree()
    {
        if (mainCan.Find("TalentTree").gameObject.activeSelf) TalentTreeManager.instance.OpenCloseSkillTree();
        else
        {
            CloseAllWindows();
            TalentTreeManager.instance.OpenCloseSkillTree();
        }
    }

    public void OpenCloseDeckBuilder()
    {
        if (mainCan.Find("DeckBuilding").gameObject.activeSelf) DeckbuildingManager.instance.OpenClose();
        else
        {
            CloseAllWindows();
            DeckbuildingManager.instance.OpenClose();
        }
    }

    public void CloseAllWindows()
    {
        if (mainCan.Find("DeckBuilding").gameObject.activeSelf) DeckbuildingManager.instance.OpenClose();
        if (mainCan.Find("TalentTree").gameObject.activeSelf) TalentTreeManager.instance.OpenCloseSkillTree();
    }
}
