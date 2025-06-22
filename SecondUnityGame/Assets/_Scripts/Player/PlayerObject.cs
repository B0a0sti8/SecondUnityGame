using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    public static PlayerObject instance;

    private void Awake()
    {
        instance = this;
    }
}
