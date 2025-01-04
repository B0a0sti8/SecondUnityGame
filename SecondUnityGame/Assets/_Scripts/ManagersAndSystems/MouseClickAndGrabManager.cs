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

        Vector3 myWorldposition = mainCam.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D rayHit = Physics2D.Raycast((Vector2)myWorldposition, new Vector3(0, 0, 1));
        if (rayHit)
        {
            if (rayHit.transform.GetComponent<TokenSlot>().hasToken == false)
            {
                rayHit.transform.GetComponent<TokenSlot>().SetToken(myGrabbedItem.GetComponent<MainCardScript>().myCardScriptable, true);
            }
        }
        isDraggingCard = false;
    }

    void MovingToken()
    {
        if (!Input.GetMouseButtonUp(0)) return;

        Vector3 myWorldposition = mainCam.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D rayHit = Physics2D.Raycast((Vector2)myWorldposition, new Vector3(0, 0, 1));
        if (rayHit)
        {
            if (rayHit.transform.GetComponent<TokenSlot>().hasToken == false)
            {
                rayHit.transform.GetComponent<TokenSlot>().SetToken(movingTokenOrigin.GetComponent<TokenSlot>().myCardToken, false);
                movingTokenOrigin.GetComponent<TokenSlot>().RemoveToken();
                isMovingToken = false;
                GridMovementManager.instance.DisableMovementMarkers();
            }
        }
    }

    public void RemoveGrabbedItem()
    {
        Destroy(myGrabbedItem.gameObject);
        myGrabbedItem = null;
    }

    public void TokenClicked(GameObject tokenSlotClicked)
    {
        GridMovementManager.instance.TokenWantsToMove((int)Mathf.Round(tokenSlotClicked.transform.position.x), (int)Mathf.Round(tokenSlotClicked.transform.position.y));
        isMovingToken = true;
    }
}
