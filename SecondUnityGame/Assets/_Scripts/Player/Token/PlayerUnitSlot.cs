using UnityEngine;
using System.Collections.Generic;

public class PlayerUnitSlot : DefaultTokenSlot
{
    GameObject energyBar;
    Color myDefaultBackgroundColor;

    public override void Start()
    {
        base.Start();
        myDefaultBackgroundColor = transform.Find("Background").GetComponent<SpriteRenderer>().color;
    }

    public override void OnTokenSet()
    {
        base.OnTokenSet();
        TurnAndEnemyManager.instance.CheckEnemyTargets();
    }

    public void HighlightBackground(bool isMarked)
    {
        Color newCol = myDefaultBackgroundColor;
        if (isMarked) newCol.a = 0.85f;
        transform.Find("Background").GetComponent<SpriteRenderer>().color = newCol;
    }
}
