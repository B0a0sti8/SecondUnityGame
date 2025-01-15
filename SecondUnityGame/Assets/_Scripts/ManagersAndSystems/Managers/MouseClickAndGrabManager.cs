using UnityEngine;

public class MouseClickAndGrabManager : MonoBehaviour
{
    public GameObject myGrabbedItem;

    public bool isMovingToken = false;
    public GameObject movingTokenOrigin;

    public bool isDraggingCard;

    public static MouseClickAndGrabManager instance;
    Camera mainCam;

    private void Awake()
    {
        instance = this;
        myGrabbedItem = null;
        mainCam = Camera.main;
    }

    void Update()
    {
        if (isDraggingCard) DraggingCard();
        if (isMovingToken) MovingToken();
    }

    void DraggingCard()
    {
        if (myGrabbedItem == null) return;          // wird geblockt, wenn kein item in der Hand
        if (!Input.GetMouseButtonUp(0)) return;     // wird geblockt, wenn die Maus-Taste nicht released wird

        TokenSlot myTokSlot = ConvertMousePositionToTokenSlot();

        if (myTokSlot != null && myTokSlot.hasToken == false)
        {
            CardManager.instance.AddCardToDiscardPile(myGrabbedItem.GetComponent<MainCardScript>().myCardScriptable);
            myTokSlot.SetToken(myGrabbedItem.GetComponent<MainCardScript>().myCardScriptable, true);
        }
        isDraggingCard = false;
    }

    void MovingToken()
    {
        if (!Input.GetMouseButtonUp(0)) return;

        TokenSlot myTokSlot = ConvertMousePositionToTokenSlot();
        if (myTokSlot != null && myTokSlot.hasToken == false)
        {
            if (myTokSlot.transform.Find("MovementMarker").gameObject.activeSelf)
            {
                int energyCost = GridAndMovementManager.instance.Pathfinding_FindEnergyCostBetweenTokenSlots(movingTokenOrigin, myTokSlot.gameObject);
                int currentEnergy = movingTokenOrigin.GetComponent<TokenSlot>().currentEnergy;
                myTokSlot.SetToken(movingTokenOrigin.GetComponent<TokenSlot>().myCardToken, false, currentEnergy - energyCost);
                //movingTokenOrigin.GetComponent<TokenSlot>().ModifyTokenEnergy(-1 * energyCost, TokenSlot.EnergyModificationSource.Moving);
                movingTokenOrigin.GetComponent<TokenSlot>().RemoveToken();
            }
            GridAndMovementManager.instance.DisableMovementMarkers();
            isMovingToken = false;
        }
    }

    public void RemoveGrabbedItem()
    {
        Destroy(myGrabbedItem.gameObject);
        myGrabbedItem = null;
    }

    public void TokenClicked(GameObject tokenSlotClicked)
    {
        GridAndMovementManager.instance.AllyTokenWantsToMove((int)Mathf.Round(tokenSlotClicked.transform.position.x), (int)Mathf.Round(tokenSlotClicked.transform.position.y));
        isMovingToken = true;
    }

    TokenSlot ConvertMousePositionToTokenSlot()
    {
        Vector3 myWorldposition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        TokenSlot mytokSlot = null;

        RaycastHit2D rayHit = Physics2D.Raycast((Vector2)myWorldposition, new Vector3(0, 0, 1));
        if (rayHit)
        {
            mytokSlot = rayHit.transform.GetComponent<TokenSlot>();
        }
        //Debug.Log(mytokSlot);
        return mytokSlot;
    }
}
