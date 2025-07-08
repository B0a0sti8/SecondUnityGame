using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TalentTreeManager : MonoBehaviour
{
    public Transform skillTreeObject;
    public bool isInWorldScene = false;
    public static TalentTreeManager instance;

    //public bool talent1_BasicResourceGenerationActive;

    private void Awake()
    {
        instance = this;
    }

    public void InitStuff(Scene scene, LoadSceneMode sMode)
    {
        if (SceneManager.GetActiveScene().name == "WorldMap")
        {
            isInWorldScene = true;
            skillTreeObject = MainCanvasSingleton.instance.transform.Find("TalentTree");
            //MainCanvasSingleton.instance.transform.Find("TaskBar").Find("SkillTree").Find("SkillTreeButton").GetComponent<Button>().onClick.AddListener(() => OpenCloseSkillTree());
            MainCanvasSingleton.instance.transform.Find("TalentTree").Find("AdditionalContent").Find("ExitButton").GetComponent<Button>().onClick.AddListener(() => OpenCloseSkillTree());
            skillTreeObject.gameObject.SetActive(false);
        }
        else
        {
            isInWorldScene = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitStuff(SceneManager.GetActiveScene(), LoadSceneMode.Additive);
        SceneManager.sceneLoaded += InitStuff;
    }

    public void OpenCloseSkillTree()
    {
        if (skillTreeObject.gameObject.activeSelf) skillTreeObject.gameObject.SetActive(false);
        else skillTreeObject.gameObject.SetActive(true);
    }

    public void GenerateWoodAndStone(object sender, EventArgs e)
    {
        RessourceManager.instance.AddOrRemoveResources(1, 1, 0, 0, null);
        Debug.Log("Trigger");
    }

}
