using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadManager : MonoBehaviour
{
    Transform levelOptions;

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
            //myFinishBut.onClick.AddListener(() => LoadWorldMapScene());
            myFinishBut.onClick.AddListener(() => LoadNewScene(MyScene.WorldMap));
        }
        else
        {
            levelOptions = MainCanvasSingleton.instance.transform.Find("WorldMap").Find("LevelOptionMenue");

            Transform myRegs = GameObject.Find("MainCanvas").transform.Find("WorldMap").Find("Regions");
            //myRegs.Find("Region1").GetComponent<Button>().onClick.AddListener(() => LoadRegion1_Stage1());
            //myRegs.Find("Region1").GetComponent<Button>().onClick.AddListener(() => LoadNewScene(MyScene.Region1_Stage1));
            myRegs.Find("Region1").GetComponent<Button>().onClick.RemoveAllListeners();
            myRegs.Find("Region1").GetComponent<Button>().onClick.AddListener(() => PrepareLevelMenue(MyScene.Region1_Stage1, myRegs.Find("Region1").position));
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

    private void PrepareLevelMenue(MyScene myPrepScene, Vector3 posi)
    {
        levelOptions.gameObject.SetActive(true);
        levelOptions.GetComponent<LevelOptionMenueScript>().UpdateUI(myPrepScene);
        levelOptions.position = posi;
        levelOptions.Find("StartButton").GetComponent<Button>().onClick.AddListener(() => LoadNewScene(myPrepScene));
    }

    //public void LoadTestingGroundsScene()
    //{
    //    LoadNewScene(MyScene.TestingGrounds);
    //}

    //public void LoadWorldMapScene()
    //{
    //    LoadNewScene(MyScene.WorldMap);
    //}
    //public void LoadRegion1_Stage1()
    //{
    //    LoadNewScene(MyScene.Region1_Stage1);
    //}
}
