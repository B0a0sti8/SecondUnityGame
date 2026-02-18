using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelOptionMenueScript : MonoBehaviour
{
    Button startButton, exitButton;
    TextMeshProUGUI regionNameText, leveltypeText, dangerLevelText, badCardAmountText;

    private void Start()
    {
        InitReferences();
    }

    private void InitReferences()
    {
        exitButton = transform.Find("ExitButton").GetComponent<Button>();
        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(() => CloseLevelMenue());

        regionNameText = transform.Find("RegionTitle").GetComponent<TextMeshProUGUI>();
        leveltypeText = transform.Find("LevelInfos").Find("LevelType").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        dangerLevelText = transform.Find("LevelInfos").Find("Dangerlevel").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        badCardAmountText = transform.Find("LevelInfos").Find("BadCards").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    public void CloseLevelMenue()
    {
        gameObject.SetActive(false);
        startButton.onClick.RemoveAllListeners();
    }

    public void UpdateUI(SceneLoadManager.MyScene sceneName)
    {
        InitReferences();
        switch (sceneName)
        {
            case SceneLoadManager.MyScene.TestingGrounds:
                break;
            case SceneLoadManager.MyScene.WorldMap:
                break;
            case SceneLoadManager.MyScene.Region1_Stage1:
                regionNameText.text = "Region 1, Stage 1";
                leveltypeText.text = "Search";
                dangerLevelText.text = "1";
                badCardAmountText.text = "5";
                break;
            default:
                break;
        }
    }
}
