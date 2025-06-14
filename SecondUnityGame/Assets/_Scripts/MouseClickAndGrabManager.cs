using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClickAndGrabManager : MonoBehaviour
{
    public static MouseClickAndGrabManager instance;
    Camera mainCam;

    [SerializeField] GameObject tokenPrefab;

    public GameObject myGrabbedItem;
    private Vector3 originPosition;
    private Vector3 originRotation;
    private Transform originalParent;

    private void Awake()
    {
        instance = this;
        myGrabbedItem = null;
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && myGrabbedItem == null) GrabCardOrToken();             // Wenn du nichts in der Hand hast und Linksklick kommt, versuche was zu greifen.
        else if (Input.GetMouseButtonUp(1) && myGrabbedItem != null) RemoveGrabbedObject();         // Wenn du was in der Hand hast und Rechtsklick kommt, leg es wieder hin.
        else if (Input.GetMouseButtonUp(0) && myGrabbedItem != null && myGrabbedItem.gameObject.tag == "Card") TryPlayCard();         // Wenn Karte in der Hand und Linksklick --> Versuche zu spielen 
        else if (Input.GetMouseButtonUp(0) && myGrabbedItem != null && myGrabbedItem.gameObject.tag != "Card") TryPlaceToken();       // Wenn Token in der Hand und Linksklick --> Versuche zu legen / tauschen 

        if (myGrabbedItem != null)
        {
            // Wenn ein item gegriffen ist, wird zwischen Karte und Token unterschieden. Und das Item entsprechend bewegt.
            if (myGrabbedItem.gameObject.tag == "Card") myGrabbedItem.transform.position = Input.mousePosition;
            else myGrabbedItem.transform.position = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition); //- (Vector2)parentPosition
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

        if (rayHit && rayHit.transform.gameObject.tag == "PlayerSlot")
        {
            myGrabbedItem = rayHit.transform.Find("PlayerToken").gameObject;//
            originPosition = myGrabbedItem.transform.localPosition;
            originRotation = myGrabbedItem.transform.localEulerAngles;
            originalParent = rayHit.transform;

            myGrabbedItem.transform.parent = null;

            return;
        }
    }

    public void RemoveGrabbedObject()
    {
        //Destroy(myGrabbedItem.gameObject);
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

        if (myCard.createsPlayerToken) // Eine Karte die ein Token generiert wurde gespielt
        {
            originPosition = new Vector3(0, 0, 0);
            originRotation = new Vector3(0, 0, 0);

            GameObject newToken = Instantiate(tokenPrefab);
            newToken.name = "PlayerToken";
            newToken.GetComponent<PlayerToken>().SetToken(myCard.myPlayerTokenScriptable);

            Destroy(myCard.gameObject);
            myGrabbedItem = newToken;

            myGrabbedItem.transform.position = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);

            // Bug: Wenn Karte ein Token beschwört und man dann Rechtsklickt, wird das Token irgendwo abgelegt.

        }

        // Todo: Karte in den Ablagestapel, Ressourcencheck
    }

    void TryPlaceToken()
    {
        // Hier checken ob ein Tokenslot getroffen wird. Wenn ja --> Schauen ob ein Token drinnen ist. Entsprechend Token / legen tauschen 
        Vector3 myWorldposition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D rayHit = Physics2D.Raycast((Vector2)myWorldposition, new Vector3(0, 0, 1));

        Debug.Log("1");

        if (rayHit && rayHit.transform.gameObject.tag == "PlayerSlot") // Wir haben einen Tokenslot
        {

            Debug.Log("2");
            Transform targetTokSlot = rayHit.transform;

            if (targetTokSlot.Find("PlayerToken") == null) // Der TokenSlot ist leer
            {
                myGrabbedItem.transform.parent = targetTokSlot;
                myGrabbedItem.transform.localPosition = originPosition;
                myGrabbedItem.transform.localEulerAngles = originRotation;

                myGrabbedItem = null;
            }

            else // Ein Fremder, nicht leerer slot --> Tausche den Hand-Token mit dem Token im slot.
            {
                GameObject newToken = targetTokSlot.Find("PlayerToken").gameObject;
                myGrabbedItem.transform.parent = targetTokSlot;
                myGrabbedItem.transform.localPosition = originPosition;
                myGrabbedItem.transform.localEulerAngles = originRotation;

                myGrabbedItem = newToken;
                myGrabbedItem.transform.parent = null;
            }
        }
    }

    //TokenSlot ConvertMousePositionToTokenSlot()
    //{
    //    Vector3 myWorldposition = mainCam.ScreenToWorldPoint(Input.mousePosition);
    //    TokenSlot mytokSlot = null;

    //    RaycastHit2D rayHit = Physics2D.Raycast((Vector2)myWorldposition, new Vector3(0, 0, 1));
    //    if (rayHit)
    //    {
    //        mytokSlot = rayHit.transform.GetComponent<TokenSlot>();
    //    }
    //    //Debug.Log(mytokSlot);
    //    return mytokSlot;
    //}
}
