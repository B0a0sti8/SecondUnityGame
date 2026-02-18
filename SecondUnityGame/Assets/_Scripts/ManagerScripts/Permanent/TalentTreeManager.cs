using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TalentTreeManager : MonoBehaviour
{
    public Transform skillTreeObject;
    public bool isInWorldScene = false;
    public static TalentTreeManager instance;

    public Dictionary<string, int> talentPointCount = new Dictionary<string, int>();

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
            MainCanvasSingleton.instance.transform.Find("TalentTree").Find("ExitButton").GetComponent<Button>().onClick.AddListener(() => OpenCloseSkillTree());
            skillTreeObject.gameObject.SetActive(false);
        }
        else isInWorldScene = false;
    }

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

    // Ab hier: Talent-Funktionen. Können je nach Talent sehr unterschiedlich ausfallen. Bennenung nach Talentnamen im Tree

    public void Talent4_StartingResources()
    {
        if (!talentPointCount.ContainsKey("Starting Resources")) return;

        int talPoi = talentPointCount["Starting Resources"];
        if (talPoi == 0) return;

        ResourceManager.instance.AddOrRemoveResources(talPoi, talPoi, 0, 0, null);

        if (talPoi == 3) ResourceManager.instance.AddOrRemoveResources(0, 0, 1, 1, null);
    }

    public void Talent7_BasicRessourceGeneration(object sender, EventArgs e)
    {
        int talPoi = talentPointCount["Basic Resource Generation"];

        for (int i = 0; i < talPoi; i++)
        {
            int res = UnityEngine.Random.Range(0, 4);

            if (res == 0) ResourceManager.instance.AddOrRemoveResources(1, 0, 0, 0, null);
            if (res == 1) ResourceManager.instance.AddOrRemoveResources(0, 1, 0, 0, null);
            if (res == 2) ResourceManager.instance.AddOrRemoveResources(0, 0, 1, 0, null);
            if (res == 3) ResourceManager.instance.AddOrRemoveResources(0, 0, 0, 1, null);
        }
        Debug.Log("Trigger");
    }

    public float Talent8_UnitBaseDamage()
    {
        if (!talentPointCount.ContainsKey("Unit Base Damage")) return 0f;

        int talPoi = talentPointCount["Unit Base Damage"];
        float damageHealMod = talPoi * 1f;
        return damageHealMod;
    }
}