using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    GameObject skillEffectSprite;
    GameObject combatVisualObject;
    [SerializeField] GameObject damageIndicatorObject;
    //Transform listOfAllBuffs;

    private void Awake()
    {
        instance = this;
        SceneManager.sceneLoaded += InitRefs;
        if (SceneManager.GetActiveScene().name == "WorldMap") gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }

    public void InitRefs(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "WorldMap") gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }

        private void OnDestroy()
    {
        SceneManager.sceneLoaded -= InitRefs;
    }

    private void Start()
    {
        skillEffectSprite = GameObject.Find("Level").transform.Find("CombatVisuals").Find("SkillEffektSprite").gameObject;
        combatVisualObject = skillEffectSprite.transform.parent.gameObject;

        //if (GameObject.Find("MainSystems") == null) listOfAllBuffs = GameObject.Find("Systems").transform.Find("ListOfAllBuffs");
        //else listOfAllBuffs = GameObject.Find("MainSystems").transform.Find("ListOfAllBuffs");
    }

    // Hier kommt Schaden und Heilung

    public void DealDamageOrHealing(GameObject target, GameObject source, float damageAmount)
    {
        if (target.GetComponentInChildren<DefaultToken>() != null)
        {
            if (damageAmount > 0) // nimmt wirklich schaden und keine Heilung
            {
                Debug.Log("Dealing Damage: " + damageAmount + " Damage reduction: " + target.GetComponentInChildren<DefaultToken>().receiveDmgHealVal.GetValue());
                damageAmount *= target.GetComponentInChildren<DefaultToken>().receiveDmgHealVal.GetValue();

            }

            target.GetComponentInChildren<DefaultToken>().TakeDamageOrHealing(damageAmount);
        }

        ShowDamageHealingIndicator((int)damageAmount, false, target.transform.position);
    }

    public void DealDamageToPlayer(GameObject source, float damageAmount)
    {
        RessourceManager.instance.TakeDamageOrHealing_Player((int)damageAmount);
    }

    // Hier kommen die Buffs

    public void ApplyBuffToTarget(GameObject target, GameObject source, Buff myBuff, Sprite buffSprite, int buffDuration, float buffStrengthMod)
    {
        if (target.GetComponent<DefaultToken>() == null) return;

        Buff newBuff = myBuff.Clone();
        newBuff.StartBuffEffect(target.GetComponent<DefaultToken>(), buffDuration, buffSprite, myBuff.buffName, buffStrengthMod);
        target.GetComponent<DefaultToken>().UpdateBuffUI();
    }

    public void RemoveBuffFromTarget(GameObject target, Buff myBuff)
    {
        if (target.GetComponent<DefaultToken>() == null) return;

        Buff newBuff = myBuff.Clone();
        List<Buff> currentBuffs = target.GetComponent<DefaultToken>().myCurrentBuffs;
        for (int i = currentBuffs.Count - 1; i >= 0; i--)
        {
            if (currentBuffs[i].name == newBuff.name) currentBuffs[i].EndBuffEffect();

        }
        target.GetComponent<DefaultToken>().UpdateBuffUI();
    }

    // Ab hier kommt hauptsächlich UI stuff

    public void ShowSkillEffect(Sprite skillSprite, Vector2 position, float scalingFactor, float duration) 
    {
        skillEffectSprite.SetActive(true);
        skillEffectSprite.GetComponent<SpriteRenderer>().sprite = skillSprite;
        skillEffectSprite.transform.position = position;
        skillEffectSprite.transform.localScale = new Vector3(10,10,1) * scalingFactor;
        StartCoroutine(SetSkillEffectSpriteInactive(duration));
    }

    public void ShowDamageHealingIndicator(int amount, bool isCrit, Vector3 position)
    {
        GameObject myDamagePopUp = GameObject.Instantiate(damageIndicatorObject, combatVisualObject.transform.Find("Canvas"));
        Vector3 randomPosMod = new Vector3(Random.Range(0f, 3f), Random.Range(0f, 3f), 0);
        myDamagePopUp.transform.position = position + randomPosMod;
        myDamagePopUp.GetComponent<TextMeshProUGUI>().text = Mathf.Abs(amount).ToString();
        if (isCrit) myDamagePopUp.GetComponent<TextMeshProUGUI>().text += "!";

        if (amount >= 0) myDamagePopUp.GetComponent<FadeOverTime>().myTextColor = Color.red;
        else myDamagePopUp.GetComponent<FadeOverTime>().myTextColor = Color.green;
    }

    IEnumerator SetSkillEffectSpriteInactive(float time)
    {
        yield return new WaitForSeconds(time);
        skillEffectSprite.SetActive(false);
    }
}
