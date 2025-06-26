using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    public static PlayerObject instance;
    public float baseDmgHealVal;
    public List<float> dmgHealModifiersAdd;
    public List<float> dmgHealModifiersMult;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        baseDmgHealVal = 50;
    }
}
