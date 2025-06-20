using System.Collections.Generic;
using UnityEngine;

public class PlayerTokenAbilityPrefab : MonoBehaviour
{
    public int energyCost;
    public int range;
    public int abilityCheckPoints;
    public List<GameObject> currentTargets;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public bool StartUsingAbility()
    {
        // Checke Ressourcen usw. Wenn verfügbar, return true;

        return true;
    }

    public virtual void UseAbility()
    {

    }

    public void ApplyAbilityEffect()
    {

    }
}
