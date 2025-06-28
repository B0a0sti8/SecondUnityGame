using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public enum MyScene
    {
        TestingGrounds,

    }

    public void LoadNewScene(MyScene myScene)
    {
        SceneManager.LoadScene(myScene.ToString());
    }

    public void LoadTestingGroundsScene()
    {
        LoadNewScene(MyScene.TestingGrounds);
    }
}
