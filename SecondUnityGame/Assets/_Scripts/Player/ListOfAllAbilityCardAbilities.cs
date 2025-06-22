using UnityEngine;

public class ListOfAllAbilityCardAbilities : MonoBehaviour
{
    public static ListOfAllAbilityCardAbilities instance;

    private void Awake()
    {
        instance = this;
    }
}
