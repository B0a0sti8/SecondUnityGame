using System.Collections.Generic;
using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager instance;

    List<string> listOfCompletedLevels = new List<string>();
    [SerializeField] List<LoreStoryNoteScriptable> listOfLoreElementsForSetup = new List<LoreStoryNoteScriptable>();
    Dictionary<LoreStoryNoteScriptable, bool> listOfAllLoreElements = new Dictionary<LoreStoryNoteScriptable, bool>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitLoreElements();
        listOfAllLoreElements[listOfLoreElementsForSetup[0]] = true;
        listOfAllLoreElements[listOfLoreElementsForSetup[2]] = true;
    }

    public void LoadListOfCompletedLevels(List<string> newList)
    {
        listOfCompletedLevels = newList;
    }

    public void AddCompletedLevel(string levelName)
    {
        listOfCompletedLevels.Add(levelName);
    }

    public List<string> GetListOfCompletedLevels()
    {
        return listOfCompletedLevels;
    }

    public List<LoreStoryNoteScriptable> GetListOfCollectedLore()
    {
        List<LoreStoryNoteScriptable> listOfCollectedLoreElements = new List<LoreStoryNoteScriptable>();
        listOfCollectedLoreElements.Clear();

        foreach (KeyValuePair<LoreStoryNoteScriptable, bool> keyVal in listOfAllLoreElements)
        {
            if (keyVal.Value == true) listOfCollectedLoreElements.Add(keyVal.Key);
        }
        return listOfCollectedLoreElements;
    }

    void InitLoreElements()
    {
        foreach (LoreStoryNoteScriptable loreScr in listOfLoreElementsForSetup)
        {
            listOfAllLoreElements.Add(loreScr, false);
        }
    }

    public void AddCollectedLoreElement(LoreStoryNoteScriptable newElem)
    {
        if (listOfAllLoreElements.ContainsKey(newElem)) listOfAllLoreElements[newElem] = true;        
    }

    public bool CheckIfLoreElementIsCollectred(LoreStoryNoteScriptable checkElem)
    {
        if (listOfAllLoreElements[checkElem]) return true;
        else return false;
    }
}
