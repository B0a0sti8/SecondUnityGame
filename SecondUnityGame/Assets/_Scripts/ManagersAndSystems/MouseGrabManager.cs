using UnityEngine;

public class MouseGrabManager : MonoBehaviour
{
    public GameObject myGrabbedItem;

    public static MouseGrabManager instance;
    Camera mainCam;

    private void Awake()
    {
        instance = this;
        myGrabbedItem = null;
        mainCam = Camera.main;
    }


    private void Update()
    {
        DraggingCard();
    }

    void DraggingCard()
    {
        if (myGrabbedItem == null) return;          // wird geblockt, wenn kein item in der Hand
        if (!Input.GetMouseButtonUp(0)) return;     // wird geblockt, wenn die Maus-Taste nicht released wird

        Vector3 myWorldposition = mainCam.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D rayHit = Physics2D.Raycast((Vector2)myWorldposition, new Vector3(0, 0, 1));
        if (rayHit)
        {
            if (!rayHit.transform.GetComponent<TokenSlot>().hasToken)
            {
                rayHit.transform.GetComponent<TokenSlot>().SetToken(myGrabbedItem);
            }
        }
    }

    public void RemoveGrabbedItem()
    {
        Destroy(myGrabbedItem.gameObject);
        myGrabbedItem = null;
    }

}
