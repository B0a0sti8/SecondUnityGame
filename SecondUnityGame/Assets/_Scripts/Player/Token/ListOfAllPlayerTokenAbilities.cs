using UnityEngine;

public class ListOfAllPlayerTokenAbilities : MonoBehaviour
{
    public static ListOfAllPlayerTokenAbilities instance;

    private void Awake()
    {
        instance = this;
    }
}
