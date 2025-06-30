using UnityEngine;

public class PlayerBuildingSlot : DefaultTokenSlot
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        isBuildingSlot = true;
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
