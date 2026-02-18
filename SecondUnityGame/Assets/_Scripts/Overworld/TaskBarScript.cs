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
        transform.Find("LoreStoryAndNotes").Find("LoreStoryAndNotesButton").GetComponent<Button>().onClick.AddListener(() => OpenCloseNotebook());
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
        if (mainCan.Find("DeckBuilding").gameObject.activeSelf) mainCan.Find("DeckBuilding").GetComponent<DeckbuildingManager>().OpenClose();
        else
        {
            CloseAllWindows();
            mainCan.Find("DeckBuilding").GetComponent<DeckbuildingManager>().OpenClose();
        }
    }

    public void OpenCloseNotebook()
    {
        if (mainCan.Find("LoreStoryAndNotes").gameObject.activeSelf) mainCan.Find("LoreStoryAndNotes").GetComponent<LoreStoryAndNotes_Script>().OpenClose();
        else
        {
            CloseAllWindows();
            mainCan.Find("LoreStoryAndNotes").GetComponent<LoreStoryAndNotes_Script>().OpenClose();
        }
    }

    public void CloseAllWindows()
    {
        if (mainCan.Find("DeckBuilding").gameObject.activeSelf) mainCan.Find("DeckBuilding").GetComponent<DeckbuildingManager>().OpenClose();
        if (mainCan.Find("TalentTree").gameObject.activeSelf) TalentTreeManager.instance.OpenCloseSkillTree();
        if (mainCan.Find("LoreStoryAndNotes").gameObject.activeSelf) mainCan.Find("LoreStoryAndNotes").GetComponent<LoreStoryAndNotes_Script>().OpenClose();
    }
}
