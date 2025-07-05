using System.Collections.Generic;
using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager instance;

    List<string> listOfCompletedLevels;

    private void Awake()
    {
        instance = this;
        listOfCompletedLevels = new List<string>();
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
}
