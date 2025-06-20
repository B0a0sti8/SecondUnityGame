using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerTokenAbilityPrefab : MonoBehaviour
{
    Camera mainCam;
    GameObject rangeIndicator;
    GameObject abilityPreviewObject;

    public int energyCost;
    public int range;
    public int abilityCheckPoints;
    public int abilityCheckPointsMax;
    public float skillDamageBaseModifier;

    public List<GameObject> potentialTargets;
    public List<GameObject> currentTargets;
    public bool isSingleTarget;

    // UI Stuff
    [SerializeField] protected string abilityName;
    [SerializeField] protected string abilityDescription;
    [SerializeField] protected Sprite abilityIcon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        rangeIndicator = GameObject.Find("Level").transform.Find("CombatVisuals").Find("RangeIndicator").gameObject;

        abilityPreviewObject = MainCanvasSingleton.instance.transform.Find("PreviewSlot").Find("TokenAbilityPreview").gameObject;
        potentialTargets = new List<GameObject>();
        mainCam = Camera.main;
    }

    public bool StartUsingAbility()
    {
        // Checke Ressourcen usw. Wenn verfügbar, return true;


        potentialTargets.Clear();

        rangeIndicator.SetActive(true);
        rangeIndicator.transform.position = transform.parent.parent.position;
        rangeIndicator.transform.localScale = new Vector3(10, 10, 10) * range;

        potentialTargets = GetTargetsInCircleHelper(transform.parent.parent.position, range * 10);
        for (int i = 0; i < potentialTargets.Count; i++)
        {
            Color myColor = potentialTargets[i].transform.Find("Background").GetComponent<SpriteRenderer>().color;
            myColor.a = 0.6f;
            potentialTargets[i].transform.Find("Background").GetComponent<SpriteRenderer>().color = myColor;
        }

        abilityPreviewObject.SetActive(true);
        abilityPreviewObject.transform.Find("AbilityName").GetComponent<TextMeshProUGUI>().text = abilityName;
        abilityPreviewObject.transform.Find("AbilityDescription").GetComponent<TextMeshProUGUI>().text = abilityDescription;
        abilityPreviewObject.transform.Find("AbilityIcon").GetComponent<Image>().sprite = abilityIcon;
        abilityPreviewObject.transform.Find("TargetCount").GetComponent<TextMeshProUGUI>().text = "Target count: 0 / " + abilityCheckPointsMax.ToString();

        return true;
    }

    public void UseAbility()
    {
        if (isSingleTarget)
        {
            Vector3 myWorldposition = mainCam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D rayHit = Physics2D.Raycast((Vector2)myWorldposition, new Vector3(0, 0, 1));

            if (rayHit && (rayHit.transform.gameObject.tag == "PlayerSlot" || rayHit.transform.gameObject.tag == "EnemySlot") )
            {
                if (potentialTargets.Contains(rayHit.transform.gameObject)) 
                {
                    rayHit.transform.gameObject.GetComponent<DefaultTokenSlot>().IncreaseMark();
                    currentTargets.Add(rayHit.transform.gameObject);

                    abilityCheckPoints += 1;

                    abilityPreviewObject.transform.Find("TargetCount").GetComponent<TextMeshProUGUI>().text = "Target count: " + abilityCheckPoints + " / " + abilityCheckPointsMax.ToString();
                }
                else
                {
                    Debug.Log("Dieses Ziel kann NICHT angegriffen werden. ");
                }
            }
        }
    }

    public virtual void ApplyAbilityEffect()
    {

        AbilityCleanUp();
    }

    public void CancelAbility()
    {
        AbilityCleanUp();
    }

    public void AbilityCleanUp()
    {
        rangeIndicator.SetActive(false);

        for (int i = 0; i < potentialTargets.Count; i++)
        {
            Color myColor = potentialTargets[i].transform.Find("Background").GetComponent<SpriteRenderer>().color;
            myColor.a = 0.4f;

            potentialTargets[i].transform.Find("Background").GetComponent<SpriteRenderer>().color = myColor;
            potentialTargets[i].GetComponent<DefaultTokenSlot>().ClearMark();

        }

        abilityCheckPoints = 0;

        abilityPreviewObject.SetActive(false);

        currentTargets.Clear();
        potentialTargets.Clear();
    }

    // Eine Helferfunktion, die alle Ziele innerhalb eines Kreises findet.
    public List<GameObject> GetTargetsInCircleHelper(Vector3 circleCenter, float circleRadius)
    {
        List<GameObject> listOfMatches = new List<GameObject>();

        Collider2D[] hit = Physics2D.OverlapCircleAll(circleCenter, circleRadius, (1 << LayerMask.NameToLayer("EnemySlots")) | (1 << LayerMask.NameToLayer("PlayerSlots")));
        foreach (Collider2D coll in hit)
        {

            // Man muss sich überlegen ob man auch Slots markiert, die leer sind. Z.B. für Flächenschaden
            if (coll.gameObject.GetComponentInChildren<DefaultToken>() != null)
            {
                listOfMatches.Add(coll.gameObject);
            }
        }

        return listOfMatches;
    }
}
