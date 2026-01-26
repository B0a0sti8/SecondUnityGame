using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadManager : MonoBehaviour
{
    public enum MyScene
    {
        TestingGrounds,
        WorldMap,
        Region1_Stage1,
    }

    private void Start()
    {
        SceneManager.sceneLoaded += InitRefs;
        InitRefs(SceneManager.GetActiveScene(), LoadSceneMode.Additive);
    }

    public void InitRefs(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name != "WorldMap")
        {
            Button myFinishBut = MainCanvasSingleton.instance.transform.Find("LevelFinishedScreen").Find("Frame").Find("FinishButton").GetComponent<Button>();
            myFinishBut.onClick.AddListener(() => LoadWorldMapScene());
        }
        else
        {
            Transform myRegs = GameObject.Find("MainCanvas").transform.Find("WorldMap").Find("Regions");
            myRegs.Find("Region1").GetComponent<Button>().onClick.AddListener(() => LoadRegion1_Stage1());
        }

    }

    public void LoadNewScene(MyScene myScene)
    {
        if (SceneManager.GetActiveScene().name == "WorldMap")
        {
            TalentTreeManager.instance.talentPointCount = GameObject.Find("MainCanvas").transform.Find("TalentTree").GetComponent<TalentTree>().talentPointCount;
            TurnAndEnemyManager.instance.isLevelFinished = false;
        }
        else
        {
            CardManager.instance.CloseSceneDeInitRefs();
            TurnAndEnemyManager.instance.AtTheEndOfLevel();
        }

        SceneManager.LoadScene(myScene.ToString());
    }

    public void LoadTestingGroundsScene()
    {
        LoadNewScene(MyScene.TestingGrounds);
    }

    public void LoadWorldMapScene()
    {
        LoadNewScene(MyScene.WorldMap);
    }
    public void LoadRegion1_Stage1()
    {
        LoadNewScene(MyScene.Region1_Stage1);
    }
}
