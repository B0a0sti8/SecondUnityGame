using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [SerializeField] GameObject myMainSystems;
    public bool hasStarted = false;

    private void Awake()
    {

    }

    public void InitStuff(Scene scene, LoadSceneMode mode)
    {
        List<GameObject> myStarters = new List<GameObject>(GameObject.FindGameObjectsWithTag("GameStarter"));
        foreach (GameObject gameO in myStarters)
        {
            if (gameO.GetComponent<GameStarter>().hasStarted == false)
            {
                gameO.GetComponent<GameStarter>().hasStarted = true;
                Destroy(gameO);
            }
        }
    }

    private void Start()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            Instantiate(myMainSystems);
        }
        SceneManager.sceneLoaded += InitStuff;
    }
}
