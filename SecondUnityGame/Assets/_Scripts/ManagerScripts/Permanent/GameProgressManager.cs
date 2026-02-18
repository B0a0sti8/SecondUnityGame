using System.Collections.Generic;
using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager instance;

    List<string> listOfCompletedLevels;
    List<LoreStoryNoteScriptable> listOfCollectedLoreElements;

    private void Awake()
    {
        instance = this;
        listOfCompletedLevels = new List<string>();
        listOfCollectedLoreElements = new List<LoreStoryNoteScriptable>();
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
        return listOfCollectedLoreElements;
    }
}
