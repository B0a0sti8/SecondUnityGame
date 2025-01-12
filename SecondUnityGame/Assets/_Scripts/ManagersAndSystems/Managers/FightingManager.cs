using System.Collections.Generic;
using UnityEngine;

public class FightingManager : MonoBehaviour
{
    public static FightingManager instance;

    private void Awake()
    {
        instance = this;
    }


    public void DoDamage(TokenSlot source, List<TokenSlot> targets)
    {

    }

    public void DoHealing(TokenSlot source, List<TokenSlot> targets)
    {

    }
}
