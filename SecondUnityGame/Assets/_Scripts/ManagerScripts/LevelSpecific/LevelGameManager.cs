using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGameManager : MonoBehaviour
{
    [SerializeField] GameObject levelFinishedObject;

    public int neededKnowledgeAmount; // Defines the amount of knowledge that is needed to win the level / Gained when finishing the level
    public bool isBossLevel; // Bosslevels cann not be won by collecting knowledge

    public static LevelGameManager instance;
    public List<GameObject> allEnemiesInLevel = new List<GameObject>();
    public int enemiesPerTurn = 2;

    private void Awake()
    {
        instance = this;
    }

    public void FinishLevelSuccess()
    {
        levelFinishedObject.transform.Find("Frame").Find("SuccessOrDefeat").GetComponent<TextMeshProUGUI>().text = "Success";
        levelFinishedObject.transform.Find("Frame").Find("Info").GetComponent<TextMeshProUGUI>().text = "We achieved all our goals. Let's return to base camp.";
        ShowLevelFinishedScreen();

        RessourceManager.instance.ManagePermanentKnowledge(neededKnowledgeAmount);
        GameProgressManager.instance.AddCompletedLevel(SceneManager.GetActiveScene().name);

        HandleFinishedLevel();
    }

    public void FinishLevelDefeat()
    {
        levelFinishedObject.transform.Find("Frame").Find("SuccessOrDefeat").GetComponent<TextMeshProUGUI>().text = "Defeat";
        levelFinishedObject.transform.Find("Frame").Find("Info").GetComponent<TextMeshProUGUI>().text = "The enemies were to strong and our leader is a loser. Let's return to base camp for now.";
        ShowLevelFinishedScreen();
        HandleFinishedLevel();
    }

    void ShowLevelFinishedScreen()
    {
        levelFinishedObject.SetActive(true);
    }

    public void HandleFinishedLevel()
    {
        TurnAndEnemyManager.instance.isLevelFinished = true;
    }
}
