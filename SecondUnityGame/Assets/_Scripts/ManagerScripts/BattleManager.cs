using System.Collections;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    [SerializeField] GameObject skillEffectSprite;
    GameObject combatVisualObject;
    [SerializeField] GameObject damageIndicatorObject;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        combatVisualObject = skillEffectSprite.transform.parent.gameObject;
    }

    public void DealDamage(GameObject target, GameObject source, float damageAmount)
    {
        //Debug.Log(source + " deals damage to " + target);

        // Check if target is Playertoken
        if (target.GetComponentInChildren<DefaultToken>() != null)
        {
            target.GetComponentInChildren<DefaultToken>().TakeDamageOrHealing((int)damageAmount);
        }

        ShowDamageHealingIndicator((int)damageAmount, false, true, target.transform.position);
    }

    public void DealHealing()
    {

    }

    public void DealDamageToPlayer(GameObject source, float damageAmount)
    {
        RessourceManager.instance.TakeDamageOrHealing_Player((int)damageAmount);
    }

    public void ShowSkillEffect(Sprite skillSprite, Vector2 position, float scalingFactor, float duration) 
    {
        skillEffectSprite.SetActive(true);
        skillEffectSprite.GetComponent<SpriteRenderer>().sprite = skillSprite;
        skillEffectSprite.transform.position = position;
        skillEffectSprite.transform.localScale = new Vector3(10,10,1) * scalingFactor;
        StartCoroutine(SetSkillEffectSpriteInactive(duration));
    }

    public void ShowDamageHealingIndicator(int amount, bool isCrit, bool isDamage, Vector3 position)
    {
        GameObject myDamagePopUp = GameObject.Instantiate(damageIndicatorObject, combatVisualObject.transform.Find("Canvas"));
        Vector3 randomPosMod = new Vector3(Random.Range(0f, 3f), Random.Range(0f, 3f), 0);
        myDamagePopUp.transform.position = position + randomPosMod;
        myDamagePopUp.GetComponent<TextMeshProUGUI>().text = amount.ToString();
        if (isCrit) myDamagePopUp.GetComponent<TextMeshProUGUI>().text += "!";

        if (isDamage) myDamagePopUp.GetComponent<FadeOverTime>().myTextColor = Color.red;
        else myDamagePopUp.GetComponent<FadeOverTime>().myTextColor = Color.green;

    }

    IEnumerator SetSkillEffectSpriteInactive(float time)
    {
        yield return new WaitForSeconds(time);
        skillEffectSprite.SetActive(false);
    }
}
