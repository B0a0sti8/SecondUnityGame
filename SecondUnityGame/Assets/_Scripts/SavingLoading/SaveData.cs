using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public SkillTreeData MySkillTreeData { get; set; }
    public CardData MyCardData { get; set; }
    public CompletedLevelData MyCompletedLevelData { get; set; }

    public SaveData()
    {
        MyCardData = new CardData();
        MyCompletedLevelData = new CompletedLevelData();
    }
}

[Serializable]
public class SkillTreeData
{
    public int permanentKnowledgeAmount;

    public SkillTreeData()
    {

    }
}

[Serializable]
public class CardData
{
    public Dictionary<string, int> ownedCards { get; set; }
    public Dictionary<string, int> deckCards { get; set; }

    public CardData()
    {
        ownedCards = new Dictionary<string, int>();
        deckCards = new Dictionary<string, int>();
    }
}

[Serializable]
public class CompletedLevelData
{
    public List<string> completedLevels { get; set; }

    public CompletedLevelData()
    {
        completedLevels = new List<string>();
    }
}