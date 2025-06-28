using UnityEngine;

public class SwitchOffEventSystem : MonoBehaviour
{
    private void Awake()
    {
        if (GameObject.Find("MainSystems") != null) Destroy(gameObject);
    }
}
