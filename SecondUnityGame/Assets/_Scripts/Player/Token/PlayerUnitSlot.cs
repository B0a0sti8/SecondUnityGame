using UnityEngine;
using System.Collections.Generic;

public class PlayerUnitSlot : DefaultTokenSlot
{
    GameObject energyBar;

    public override void Start()
    {
        base.Start();
    }

    public override void OnMouseOver()
    {
        base.OnMouseOver();
        if (transform.GetComponentInChildren<PlayerToken>() == null) return;

        transform.GetComponentInChildren<PlayerToken>().transform.Find("Canvas").Find("EnergyBar").gameObject.SetActive(true);
    }

    public override void OnMouseExit()
    {
        base.OnMouseOver();

        if (transform.GetComponentInChildren<PlayerToken>() == null) return;
        transform.GetComponentInChildren<PlayerToken>().transform.Find("Canvas").Find("EnergyBar").gameObject.SetActive(false);
    }
}
