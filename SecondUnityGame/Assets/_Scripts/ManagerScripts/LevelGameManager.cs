using TMPro;
using UnityEngine;

public class LevelGameManager : MonoBehaviour
{
    [SerializeField] GameObject levelFinishedObject;

    public static LevelGameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void FinishLevelSuccess()
    {
        levelFinishedObject.transform.Find("Frame").Find("SuccessOrDefeat").GetComponent<TextMeshProUGUI>().text = "Success";
        levelFinishedObject.transform.Find("Frame").Find("Info").GetComponent<TextMeshProUGUI>().text = "We achieved all our goals. Let's return to base camp.";
        ShowLevelFinishedScreen();
    }

    public void FinishLevelDefeat()
    {
        levelFinishedObject.transform.Find("Frame").Find("SuccessOrDefeat").GetComponent<TextMeshProUGUI>().text = "Defeat";
        levelFinishedObject.transform.Find("Frame").Find("Info").GetComponent<TextMeshProUGUI>().text = "The enemies were to strong and our leader is a loser. Let's return to base camp for now.";
        ShowLevelFinishedScreen();
    }


    void ShowLevelFinishedScreen()
    {
        levelFinishedObject.SetActive(true);
    }
}
