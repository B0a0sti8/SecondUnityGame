using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MouseClickAndGrabManager : MonoBehaviour
{
    public static MouseClickAndGrabManager instance;
    Camera mainCam;

    [SerializeField] GameObject tokenPrefab;

    GameObject myGrabbedItem;
    private Vector3 originPosition;
    private Vector3 originRotation;
    private Transform originalParent;

    GameObject pendingCard;
    Vector3 pendingCardOriginPosition;
    Vector3 pendingCardOriginRotation;
    private bool isCardPending;

    bool isPlayingAbility = false;
    PlayerTokenAbilityPrefab currentAbility;

    [SerializeField] GameObject tokenSelectionMenue;

    private void Awake()
    {
        instance = this;
        myGrabbedItem = null;
        mainCam = Camera.main;
    }

    void Update()
    {
        // Wenn momentan eine Fähigkeit aktiv ist, können keine Karten gespielt werden oder ähnliches.
        if (isPlayingAbility)
        {
            // Bei Rechtslklick abbrechen
            if (Input.GetMouseButtonDown(1))
            {
                isPlayingAbility = false;
                currentAbility.CancelAbility();
                currentAbility = null;

                // Wenn die Fähigkeit durch eine Karte gespielt wird, wird die Karte am Ende der Fähigkeit auf den Ablagestapel gelegt.
                if (isCardPending)
                {
                    PlaceCardBackInHand();
                    //RemoveGrabbedObject();
                    isCardPending = false;
                }
            }
            // Bei Linksklick versuchen nächste Fähigkeiten-Stufe zu spielen (z.B. Ziel anvisieren)
            else if (Input.GetMouseButtonDown(0))
            {
                if (currentAbility.abilityCheckPoints < currentAbility.abilityCheckPointsMax) currentAbility.UseAbility();
                else
                {
                    currentAbility.ApplyAbilityEffect();
                    isPlayingAbility = false;
                    currentAbility = null;

                    // Wenn die Fähigkeit durch eine Karte gespielt wird, wird die Karte am Ende der Fähigkeit auf den Ablagestapel gelegt.
                    if (isCardPending)
                    {
                        HandlePlayedCard();
                        isCardPending = false;
                    }
                }
            }
            // Bei Mausrad wird für Flächenschaden der Indikator gedreht
            else if (Input.mouseScrollDelta.y > 0) if (currentAbility.myTargetType == PlayerTokenAbilityPrefab.TargetType.MultiTarget) currentAbility.RotateMultiTargetShape(90);
            else if (Input.mouseScrollDelta.y < 0) if (currentAbility.myTargetType == PlayerTokenAbilityPrefab.TargetType.MultiTarget) currentAbility.RotateMultiTargetShape(-90);
            return;
        }

        // KameraZoom
        if (Input.mouseScrollDelta.y > 0) mainCam.transform.parent.GetComponent<CameraMovement>().HandleCameraZoom(-3);
        if (Input.mouseScrollDelta.y < 0) mainCam.transform.parent.GetComponent<CameraMovement>().HandleCameraZoom(3);

        // Wenn das Token-Menü geöffnet ist, aber kein Button getroffen wird, wird das Menü geschlossen.
        // Wenn ein Button getroffen wird, wird versucht die Fähigkeit zu starten.
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) CheckCloseTokenSelectionMenue();

        // Wenn gerade keine Karte gespielt wird (allgemeiner Fall)
        if (!isCardPending)         
        {
            if (Input.GetMouseButtonDown(0) && myGrabbedItem == null) GrabCardOrToken();             // Wenn du nichts in der Hand hast und Linksklick kommt, versuche was zu greifen.
            else if (Input.GetMouseButtonDown(1) && myGrabbedItem == null) SelectToken();                 // Wenn du nichts in der Hand hast und Rechtsklick kommt, versuche Token zu nutzen.

            else if (Input.GetMouseButtonDown(0) && myGrabbedItem != null && myGrabbedItem.gameObject.tag == "Card") TryPlayCard();         // Wenn Karte in der Hand und Linksklick --> Versuche zu spielen 
            else if (Input.GetMouseButtonDown(0) && myGrabbedItem != null && myGrabbedItem.gameObject.tag != "Card") TryPlaceToken();       // Wenn Token in der Hand und Linksklick --> Versuche zu legen / tauschen 
            else if (Input.GetMouseButtonDown(1) && myGrabbedItem != null) RemoveGrabbedObject();         // Wenn du was in der Hand hast und Rechtsklick kommt, leg es wieder hin.
        }
        // Es wird gerade ein Karten-Effekt gespielt --> Es können keine Token-Menüs geöffnet oder Tokens verschoben werden usw.
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Check Card Pending");
                PlaceCardBackInHand();
            }
            else if (Input.GetMouseButtonDown(0) && myGrabbedItem != null && myGrabbedItem.gameObject.tag != "Card") // Die karte hat ein Token erschaffen und es wird Linksklick gedrückt
            {
                if (TryPlaceToken()) HandlePlayedCard();
            }

            // Hier müssen alle Karteneffekte abgehandelt werden, die keine Tokens erzeugen.
        }
        // Wenn ein item gegriffen ist, wird zwischen Karte und Token unterschieden. Und das Item entsprechend bewegt.
        if (myGrabbedItem != null)
        {
            if (myGrabbedItem.gameObject.tag == "Card") myGrabbedItem.transform.position = Input.mousePosition;
            else myGrabbedItem.transform.position = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void SelectToken()
    {
        Vector3 myWorldposition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D rayHit = Physics2D.Raycast((Vector2)myWorldposition, new Vector3(0, 0, 1));

        if (rayHit && (rayHit.transform.gameObject.tag == "PlayerUnitSlot" || rayHit.transform.gameObject.tag == "PlayerBuildingSlot"))
        {
            if (rayHit.transform.gameObject.GetComponentInChildren<PlayerToken>() != null)
            {
                GameObject myToken = rayHit.transform.gameObject.GetComponentInChildren<PlayerToken>().gameObject;
                tokenSelectionMenue.SetActive(true);
                tokenSelectionMenue.transform.position = rayHit.transform.position;
                for (int i = 0; i < myToken.transform.Find("Abilities").childCount; i++)
                {
                    if (i < 6)
                    {
                        tokenSelectionMenue.transform.GetChild(i).gameObject.SetActive(true);
                        tokenSelectionMenue.transform.GetChild(i).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = myToken.transform.Find("Abilities").GetChild(i).name;
                        tokenSelectionMenue.transform.GetChild(i).GetComponent<AbilitySelectionButton>().myAbilityObject = myToken.transform.Find("Abilities").GetChild(i).gameObject;
                    }
                }

                return;
            }
        }
    }

    void CheckCloseTokenSelectionMenue()
    {
        if (!tokenSelectionMenue.activeSelf) return; // Schaue ob Menü überhaupt aktiv ist.

        var pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        if (raycastResults.Count > 0 && raycastResults[0].gameObject.tag == "SelectionButton") // Button wurde geklickt
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentAbility = raycastResults[0].gameObject.GetComponent<AbilitySelectionButton>().myAbilityObject.GetComponent<PlayerTokenAbilityPrefab>();
                if (currentAbility.StartUsingAbility())
                {
                    isPlayingAbility = true;
                    tokenSelectionMenue.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < tokenSelectionMenue.transform.childCount; i++)
            {
                tokenSelectionMenue.transform.GetChild(i).gameObject.SetActive(false);
            }
            tokenSelectionMenue.SetActive(false);
        }
    }

    void GrabCardOrToken()
    {
        // Dieser Abschnitt schaut ob er eine Karte erwischt.
        var pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        if (raycastResults.Count > 0 && raycastResults[0].gameObject.tag == "Card")
        {
            myGrabbedItem = raycastResults[0].gameObject;
            originPosition = myGrabbedItem.transform.position;
            originRotation = myGrabbedItem.transform.eulerAngles;
            myGrabbedItem.transform.eulerAngles = new Vector3(0, 0, 0);

            return; // Wenn Karte gefunden, brauchst du gar nicht weiterschauen.
        }

        // Dieser Abschnitt schaut, ob er ein Token-Feld erwischt.
        Vector3 myWorldposition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D rayHit = Physics2D.Raycast((Vector2)myWorldposition, new Vector3(0, 0, 1));

        if (rayHit && rayHit.transform.gameObject.tag == "PlayerUnitSlot" && rayHit.transform.Find("PlayerToken") != null)
        {
            myGrabbedItem = rayHit.transform.Find("PlayerToken").gameObject;
            originPosition = myGrabbedItem.transform.localPosition;
            originRotation = myGrabbedItem.transform.localEulerAngles;
            originalParent = rayHit.transform;

            myGrabbedItem.transform.parent = null;

            return;
        }
    }

    void RemoveGrabbedObject()
    {
        if (myGrabbedItem.gameObject.tag == "Card")
        {
            myGrabbedItem.transform.position = originPosition;
            myGrabbedItem.transform.eulerAngles = originRotation;
        }
        else
        {
            myGrabbedItem.transform.parent = originalParent;
            myGrabbedItem.transform.localPosition = originPosition;
            myGrabbedItem.transform.localEulerAngles = originRotation;
        }

        myGrabbedItem = null;
    }

    void TryPlayCard()
    {
        MainCardScript myCard = myGrabbedItem.GetComponent<MainCardScript>();

        // Hier müssen noch Ressourcen und sontige Voraussetzungen geklärt werden.

        pendingCard = myCard.gameObject;
        pendingCard.transform.position = new Vector2(1800, 500);
        pendingCard.transform.SetParent(HandCardScript.instance.transform.parent);

        pendingCardOriginPosition = originPosition;
        pendingCardOriginRotation = originRotation;
        isCardPending = true;

        if (myCard.myCardToken.cardTypeString == "Unit" || myCard.myCardToken.cardTypeString == "Building") // Eine Karte die ein Token generiert wurde gespielt
        {
            originPosition = new Vector3(0, 0, 0);
            originRotation = new Vector3(0, 0, 0);

            GameObject newToken = Instantiate(tokenPrefab);
            newToken.name = "PlayerToken";
            newToken.GetComponent<PlayerToken>().SetToken((PlayerTokenScriptable)myCard.myCardToken);

            if (myCard.myCardToken.cardTypeString == "Building") newToken.transform.localScale *= 2;

            myGrabbedItem = newToken;
            myGrabbedItem.transform.position = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
        }

        else if (myCard.myCardToken.cardTypeString == "Ability")
        {
            string abilityName = ((AbilityCardScriptable)myCard.myCardToken).abilityName;
            currentAbility = ListOfAllAbilityCardAbilities.instance.transform.Find(abilityName).GetComponent<PlayerTokenAbilityPrefab>();
            currentAbility.isAbilityCard = true;

            if (currentAbility.StartUsingAbility())
            {
                myGrabbedItem = null;
                isPlayingAbility = true;
                tokenSelectionMenue.SetActive(false);
            }
        }
    }

    void HandlePlayedCard()
    {
        CardManager.instance.AddCardToDiscardPile(pendingCard.GetComponent<MainCardScript>().myCardToken);
        HandCardScript.instance.RemoveCard(pendingCard);
        pendingCard.GetComponent<MainCardScript>().DestroyCard();
        pendingCard = null;
    }

    bool TryPlaceToken()
    {
        // Dieser ist der Part für Einheiten
        // Hier checken ob ein Tokenslot getroffen wird. Wenn ja --> Schauen ob ein Token drinnen ist. Entsprechend Token / legen tauschen 
        Vector3 myWorldposition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D rayHit = Physics2D.Raycast((Vector2)myWorldposition, new Vector3(0, 0, 1));

        if (rayHit && rayHit.transform.gameObject.tag == "PlayerUnitSlot" && myGrabbedItem.GetComponent<PlayerToken>().myToken.cardTypeString == "Unit") // Wir haben einen Unit Tokenslot und eine Unit in der Hand
        {
            Transform targetTokSlot = rayHit.transform;

            if (targetTokSlot.Find("PlayerToken") == null) // Der TokenSlot ist leer
            {
                myGrabbedItem.transform.parent = targetTokSlot;
                myGrabbedItem.transform.localPosition = originPosition;
                myGrabbedItem.transform.localEulerAngles = originRotation;

                myGrabbedItem = null;
                isCardPending = false;

                return true;
            }

            else // Ein Fremder, nicht leerer slot --> Tausche den Hand-Token mit dem Token im slot.
            {
                if (!isCardPending)
                {
                    GameObject newToken = targetTokSlot.Find("PlayerToken").gameObject;
                    myGrabbedItem.transform.parent = targetTokSlot;
                    myGrabbedItem.transform.localPosition = originPosition;
                    myGrabbedItem.transform.localEulerAngles = originRotation;

                    myGrabbedItem = newToken;
                    myGrabbedItem.transform.parent = null;

                    return true;
                }
            }
        }

        // Dieser ist der Part für Gebäude
        else if (rayHit && rayHit.transform.gameObject.tag == "PlayerBuildingSlot" && myGrabbedItem.GetComponent<PlayerToken>().myToken.cardTypeString == "Building") // Wir haben einen Building Tokenslot und eine Building in der Hand
        {
            Transform targetTokSlot = rayHit.transform;

            if (targetTokSlot.Find("PlayerToken") == null) // Der TokenSlot ist leer
            {
                myGrabbedItem.transform.parent = targetTokSlot;
                myGrabbedItem.transform.localPosition = originPosition;
                myGrabbedItem.transform.localEulerAngles = originRotation;

                myGrabbedItem = null;
                isCardPending = false;

                return true;
            }
        }

        return false;
    }

    void PlaceCardBackInHand()
    {
        if (myGrabbedItem != null && myGrabbedItem.gameObject.tag != "Card")
        {
            Destroy(myGrabbedItem);
        }

        myGrabbedItem = pendingCard;
        pendingCard.transform.SetParent(HandCardScript.instance.transform);
        originPosition = pendingCardOriginPosition;
        originRotation = pendingCardOriginRotation;

        isCardPending = false;
    }
}
