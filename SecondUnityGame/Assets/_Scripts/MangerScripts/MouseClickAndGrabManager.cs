using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MouseClickAndGrabManager : MonoBehaviour
{
    public static MouseClickAndGrabManager instance;
    Camera mainCam;

    [SerializeField] GameObject tokenPrefab;

    public GameObject myGrabbedItem;
    private Vector3 originPosition;
    private Vector3 originRotation;
    private Transform originalParent;

    public GameObject pendingCard;
    Vector3 pendingCardOriginPosition;
    Vector3 pendingCardOriginRotation;
    private bool isCardPending;

    public bool isPlayingTokenAbility = false;
    public PlayerTokenAbilityPrefab currentTokenAbility;

    [SerializeField] GameObject tokenSelectionMenue; 

    private void Awake()
    {
        instance = this;
        myGrabbedItem = null;
        mainCam = Camera.main;
    }

    void Update()
    {
        // Wenn kein Button im Auswahlmenü getroffen wird, und dieses offen ist, wird es geschlossen. 
        // Wenn ein Button getroffen wird, wird versucht die Fähigkeit zu starten
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) CheckCloseTokenSelectionMenue(); 


        // Wenn momentan eine Fähigkeit aktiv ist, können keine Karten gespielt werden oder ähnliches.
        // Mit Rechtsklick wird die Fähigkeit abgebrochen. Mit Linksklick wird versucht einen Checkpoint zu erreichen (z.B. ein zusätzliches Ziel zu wählen)
        if (isPlayingTokenAbility)
        {
            if (Input.GetMouseButtonDown(1))
            {
                isPlayingTokenAbility = false;
                currentTokenAbility.CancelAbility();
                currentTokenAbility = null;
            }

            else if (Input.GetMouseButtonDown(0))
            {
                if (currentTokenAbility.abilityCheckPoints < currentTokenAbility.abilityCheckPointsMax) currentTokenAbility.UseAbility();
                else currentTokenAbility.ApplyAbilityEffect();
            }

            return;
        }

        if (!isCardPending)         // Es wird gerade kein Karten-Effekt gespielt
        {
            if (Input.GetMouseButtonDown(0) && myGrabbedItem == null) GrabCardOrToken();             // Wenn du nichts in der Hand hast und Linksklick kommt, versuche was zu greifen.
            else if (Input.GetMouseButtonDown(1) && myGrabbedItem == null) SelectToken();                 // Wenn du nichts in der Hand hast und Rechtsklick kommt, versuche Token zu nutzen.

            else if (Input.GetMouseButtonDown(0) && myGrabbedItem != null && myGrabbedItem.gameObject.tag == "Card") TryPlayCard();         // Wenn Karte in der Hand und Linksklick --> Versuche zu spielen 
            else if (Input.GetMouseButtonDown(0) && myGrabbedItem != null && myGrabbedItem.gameObject.tag != "Card") TryPlaceToken();       // Wenn Token in der Hand und Linksklick --> Versuche zu legen / tauschen 
            else if (Input.GetMouseButtonDown(1) && myGrabbedItem != null) RemoveGrabbedObject();         // Wenn du was in der Hand hast und Rechtsklick kommt, leg es wieder hin.
        }
        else             // Es wird gerade ein Karten-Effekt gespielt
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

        if (myGrabbedItem != null)
        {
            // Wenn ein item gegriffen ist, wird zwischen Karte und Token unterschieden. Und das Item entsprechend bewegt.
            if (myGrabbedItem.gameObject.tag == "Card") myGrabbedItem.transform.position = Input.mousePosition;
            else myGrabbedItem.transform.position = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition); //- (Vector2)parentPosition
        }
    }

    //bool TryFinishPlayingTokenAbility()
    //{
    //    //currentTokenAbility
    //    if (true) return true;
    //    else return false;
    //}

    void SelectToken()
    {
        Vector3 myWorldposition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D rayHit = Physics2D.Raycast((Vector2)myWorldposition, new Vector3(0, 0, 1));

        if (rayHit && rayHit.transform.gameObject.tag == "PlayerSlot")
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
                currentTokenAbility = raycastResults[0].gameObject.GetComponent<AbilitySelectionButton>().myAbilityObject.GetComponent<PlayerTokenAbilityPrefab>();
                if (currentTokenAbility.StartUsingAbility())
                {
                    isPlayingTokenAbility = true;

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

        if (rayHit && rayHit.transform.gameObject.tag == "PlayerSlot" && rayHit.transform.Find("PlayerToken") != null)
        {

            myGrabbedItem = rayHit.transform.Find("PlayerToken").gameObject;//
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

        pendingCardOriginPosition = originPosition;
        pendingCardOriginRotation = originRotation;
        isCardPending = true;

        if (myCard.createsPlayerToken) // Eine Karte die ein Token generiert wurde gespielt
        {
            originPosition = new Vector3(0, 0, 0);
            originRotation = new Vector3(0, 0, 0);

            GameObject newToken = Instantiate(tokenPrefab);
            newToken.name = "PlayerToken";
            newToken.GetComponent<PlayerToken>().SetToken(myCard.myPlayerTokenScriptable);

            myGrabbedItem = newToken;
            myGrabbedItem.transform.position = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void HandlePlayedCard()
    {
        CardManager.instance.AddCardToDiscardPile(pendingCard.GetComponent<MainCardScript>().myPlayerTokenScriptable);
        HandCardScript.instance.RemoveCard(pendingCard);
        Destroy(pendingCard);
        pendingCard = null;
    }

    bool TryPlaceToken()
    {
        // Hier checken ob ein Tokenslot getroffen wird. Wenn ja --> Schauen ob ein Token drinnen ist. Entsprechend Token / legen tauschen 
        Vector3 myWorldposition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D rayHit = Physics2D.Raycast((Vector2)myWorldposition, new Vector3(0, 0, 1));

        if (rayHit && rayHit.transform.gameObject.tag == "PlayerSlot") // Wir haben einen Tokenslot
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

        return false;
    }

    void PlaceCardBackInHand()
    {
        if (myGrabbedItem != null && myGrabbedItem.gameObject.tag != "Card")
        {
            Destroy(myGrabbedItem);
            myGrabbedItem = pendingCard;
            originPosition = pendingCardOriginPosition;
            originRotation = pendingCardOriginRotation;

            isCardPending = false;
        }
    }
}
