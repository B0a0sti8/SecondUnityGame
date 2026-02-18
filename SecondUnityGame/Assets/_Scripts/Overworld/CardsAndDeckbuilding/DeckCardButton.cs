using UnityEngine;

public class DeckCardButton : MonoBehaviour
{
    public DefaultCardScriptable myCard;

    public void RemoveOneCard()
    {
        DeckbuildingManager.instance.RemoveCardFromDecklist(myCard);
    }
}
