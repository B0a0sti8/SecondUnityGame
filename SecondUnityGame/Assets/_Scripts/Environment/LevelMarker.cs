using UnityEngine;

public class LevelMarker : MonoBehaviour
{
    public static LevelMarker instance;

    private void Awake()
    {
        instance = this;
    }
}
