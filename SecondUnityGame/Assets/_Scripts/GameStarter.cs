using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] GameObject myMainSystems;

    private void Awake()
    {
        Instantiate(myMainSystems);
        Destroy(gameObject);
    }
}
