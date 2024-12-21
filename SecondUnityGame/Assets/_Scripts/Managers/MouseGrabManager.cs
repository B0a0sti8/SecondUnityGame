using UnityEngine;

public class MouseGrabManager : MonoBehaviour
{
    public static MouseGrabManager instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject myGrabbedItem;



}
