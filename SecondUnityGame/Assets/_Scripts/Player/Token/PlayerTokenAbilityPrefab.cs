using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerTokenAbilityPrefab : MonoBehaviour
{
    Camera mainCam;
    GameObject rangeIndicator;
    GameObject abilityPreviewObject;

    PlayerToken myToken;
    PlayerObject myPlayer;

    public bool isAbilityCard;

    [SerializeField] public Buff myBuff;

    public int energyCost;
    public int range;
    public int abilityCheckPoints;
    public int abilityCheckPointsMax;

    public float skillEffectModifier;
    protected float finalDamage;

    public bool isPassiveTriggerAbility = false;

    public enum TargetType
    {
        SingleTarget,
        MultiTarget,
        NoTargetNeeded
    }

    public TargetType myTargetType;

    public List<GameObject> potentialTargets;
    public List<GameObject> currentTargets;
    [SerializeField] Sprite multTargetShapeSprite;

    [SerializeField] GameObject multiTargetShape;
    public GameObject activeMultiTargetShape;
    List<GameObject> placedMultiTargetShapes = new List<GameObject>();

    // UI Stuff
    [SerializeField] public string abilityName;
    [SerializeField] public string abilityDescription;
    [SerializeField] public Sprite abilityIcon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        if (GameObject.Find("Level") == null) return;

        rangeIndicator = GameObject.Find("Level").transform.Find("CombatVisuals").Find("RangeIndicator").gameObject;

        abilityPreviewObject = MainCanvasSingleton.instance.transform.Find("PreviewSlot").Find("TokenAbilityPreview").gameObject;
        potentialTargets = new List<GameObject>();
        mainCam = Camera.main;

        myPlayer = GameObject.Find("Systems").transform.Find("CameraStuff").Find("Target").GetComponent<PlayerObject>();
        myToken = transform.parent.parent.GetComponent<PlayerToken>();
    }

    private void Update()
    {
        if (GameObject.Find("Level") == null) return;
        // Falls die Zielauswahl für Flächenschaden aktiv ist, folgt sie der Maus.
        if (activeMultiTargetShape != null) SnapMultiTargetShapeIntoGrid(activeMultiTargetShape);
    }

    public bool StartUsingAbility()
    {
        // Checke Ressourcen usw. Wenn verfügbar, return true; Wenn nicht, return false
        if (!isAbilityCard)
        {
            if (myToken.currentEnergy < energyCost)
            {
                Debug.Log("Nicht genügend Energy");
                return false;
            }
        }

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

        if (myTargetType == TargetType.MultiTarget)
        {
            for (int i = 0; i < multiTargetShape.transform.childCount; i++)
            {
                multiTargetShape.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = multTargetShapeSprite;
            }

            activeMultiTargetShape = Instantiate(multiTargetShape);
            activeMultiTargetShape.transform.position = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
        }

        return true;
    }

    // Eine Fähigkeit kann unter Umständen mehrere Ziele wählen (z.B. 3 Pfeile, die auf verschiedene Gegner verteilt werden können.)
    // Zudem, kann jede dieser Zielauswahlen Single-Target sein, oder eine Fläche mit einer bestimmten Form. (Quadrat, horizontale oder diagonale Linie, etc.)
    // Wenn alle Ziele gewählt sind, wird die Fähigkeit mit Linksklick ausgeführt.
    public void UseAbility()
    {
        // Für single-Target Fähigkeiten
        if (myTargetType == TargetType.SingleTarget)
        {
            Vector3 myWorldposition = mainCam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D rayHit = Physics2D.Raycast((Vector2)myWorldposition, new Vector3(0, 0, 1), 10, (1 << LayerMask.NameToLayer("EnemySlots")) | (1 << LayerMask.NameToLayer("PlayerUnitSlots")));

            if (rayHit && (rayHit.transform.gameObject.tag == "PlayerUnitSlot" || rayHit.transform.gameObject.tag == "EnemySlot") )
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

        // Für Multi-Target Fähigkeiten
        else if (myTargetType == TargetType.MultiTarget)
        {
            // Schau, ob Maus beim Platzieren in der range ist.
            Vector2 posit = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D rayHit = Physics2D.Raycast(posit, new Vector3(0, 0, 1), (1 << LayerMask.NameToLayer("RangeIndicator")));
            if (rayHit)
            {
                List<GameObject> myTargets = GetTargetsFromMultiTargetShape();
                currentTargets.AddRange(myTargets);

                GameObject newPlacedShape = Instantiate(activeMultiTargetShape);
                placedMultiTargetShapes.Add(newPlacedShape);

                abilityCheckPoints += 1;
                abilityPreviewObject.transform.Find("TargetCount").GetComponent<TextMeshProUGUI>().text = "Target count: " + abilityCheckPoints + " / " + abilityCheckPointsMax.ToString();
                if (abilityCheckPoints == abilityCheckPointsMax) RemoveMultShape();
            }
        }

        // Für Fähigkeiten ohne Target
        else if (myTargetType == TargetType.NoTargetNeeded)
        {
            abilityCheckPoints += 1;
            abilityPreviewObject.transform.Find("TargetCount").GetComponent<TextMeshProUGUI>().text = "Target count: " + abilityCheckPoints + " / " + abilityCheckPointsMax.ToString();
        }
    }

    public void RotateMultiTargetShape(float angle)
    {
        if (activeMultiTargetShape == null) return;
        activeMultiTargetShape.transform.eulerAngles += new Vector3(0, 0, angle);
        for (int i = 0; i < activeMultiTargetShape.transform.childCount; i++)
        {
            activeMultiTargetShape.transform.GetChild(i).eulerAngles += new Vector3(0, 0, -angle);
        }
    }

    private void RemoveMultShape()
    {
        Destroy(activeMultiTargetShape);
        activeMultiTargetShape = null;
    }

    public virtual void ApplyAbilityEffect()
    {
        AbilityCleanUp();
        if (!isAbilityCard) myToken.ManageEnergy(energyCost);
    }

    public virtual void ApplyPassiveTriggerEffect()
    {
        
    }

    public virtual void DealDamageHealing()
    {
        if (isAbilityCard)
        {
            float dmgHeal = myPlayer.dmgHealVal.GetValue();        // Hole dir den Grundschaden
            finalDamage = Mathf.Clamp(0, dmgHeal * skillEffectModifier, Mathf.Infinity);
        }
        else
        {
            float dmgHeal = myToken.dmgHealVal.GetValue();        // Hole dir den Grundschaden
            finalDamage = Mathf.Clamp(0, dmgHeal * skillEffectModifier, Mathf.Infinity);
        }
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

        if (myTargetType == TargetType.MultiTarget)
        {
            foreach (GameObject placedShape in placedMultiTargetShapes) Destroy(placedShape);
            placedMultiTargetShapes.Clear();
            if (activeMultiTargetShape != null) RemoveMultShape();
        }
    }

    // Eine Helferfunktion, die alle Ziele innerhalb eines Kreises findet.
    List<GameObject> GetTargetsInCircleHelper(Vector3 circleCenter, float circleRadius)
    {
        List<GameObject> listOfMatches = new List<GameObject>();

        Collider2D[] hit = Physics2D.OverlapCircleAll(circleCenter, circleRadius, (1 << LayerMask.NameToLayer("EnemySlots")) | (1 << LayerMask.NameToLayer("PlayerUnitSlots")));
        foreach (Collider2D coll in hit)
        {

            // Man muss sich überlegen ob man auch Slots markiert, die leer sind. Z.B. für Flächenschaden --> ja?
            if (coll.gameObject.GetComponentInChildren<DefaultToken>() != null)
            {
                //listOfMatches.Add(coll.gameObject);
            }
            listOfMatches.Add(coll.gameObject);
        }

        return listOfMatches;
    }

    List<GameObject> GetTargetsFromMultiTargetShape()
    {
        List<GameObject> listOfMatches = new List<GameObject>();

        for (int i = 0; i < activeMultiTargetShape.transform.childCount; i++)
        {
            Transform shapePart = activeMultiTargetShape.transform.GetChild(i);
            RaycastHit2D rayHit = Physics2D.Raycast((Vector2)shapePart.position, new Vector3(0, 0, 1), 10, (1 << LayerMask.NameToLayer("EnemySlots")) | (1 << LayerMask.NameToLayer("PlayerUnitSlots")));

            if (rayHit && (rayHit.transform.gameObject.tag == "PlayerUnitSlot" || rayHit.transform.gameObject.tag == "EnemySlot"))
            {
                //Debug.Log("Found something at: " + (Vector2)shapePart.position);
                if (rayHit.transform.GetComponentInChildren<DefaultToken>() != null) { }
                listOfMatches.Add(rayHit.transform.gameObject);
            }
        }
        return listOfMatches;
    }
    
    void SnapMultiTargetShapeIntoGrid(GameObject multiShape)
    {
        // Formposition an Gitter orientieren
        Vector2 currentMousePos = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
        float currentGridX = currentMousePos.x - (currentMousePos.x % 10) + 5 * Mathf.Sign(currentMousePos.x); 
        float currentGridY = currentMousePos.y - (currentMousePos.y % 10) + 5 * Mathf.Sign(currentMousePos.y);

        Vector2 currentGridPos = new Vector2(currentGridX, currentGridY);
        multiShape.transform.position = currentGridPos;
    }
}
