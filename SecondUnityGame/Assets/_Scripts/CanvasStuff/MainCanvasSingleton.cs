using UnityEngine;

public class MainCanvasSingleton : MonoBehaviour
{
    public static MainCanvasSingleton instance;

    private void Awake()
    {
        instance = this;
    }
}
