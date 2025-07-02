using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemManager : MonoBehaviour
{
    public static SystemManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += EsKannNurEinenGeben;
    }

    public void EsKannNurEinenGeben(Scene scene, LoadSceneMode mode)
    {
        if (GameObject.Find("Systems") != null) GameObject.Find("Systems").SetActive(false);
        Debug.Log("Level Loaded");
    }
}
