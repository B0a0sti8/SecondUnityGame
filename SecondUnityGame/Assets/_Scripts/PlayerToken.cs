using UnityEngine;

public class PlayerToken : MonoBehaviour
{
    private PlayerTokenScriptable myToken;

    public void SetToken(PlayerTokenScriptable newToken)
    {
        myToken = newToken;
        UpdatePlayerToken();

    }

    void UpdatePlayerToken()
    {
        transform.Find("Picture").GetComponent<SpriteRenderer>().sprite = myToken.tokenSprite;
    }
}
