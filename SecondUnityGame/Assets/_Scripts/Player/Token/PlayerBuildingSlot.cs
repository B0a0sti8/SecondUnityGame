using UnityEngine;

public class PlayerBuildingSlot : DefaultTokenSlot
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        isBuildingSlot = true;
    }
}
