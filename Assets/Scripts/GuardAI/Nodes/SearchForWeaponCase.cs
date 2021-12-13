using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchForWeaponCase : Node
{
    private Transform transform;
    private List<WeaponCase> weaponCases = new List<WeaponCase>();

    public SearchForWeaponCase(Transform transform, List<WeaponCase> weaponCases)
    {
        this.transform = transform;
        this.weaponCases = weaponCases;
    }

    public override NodeState Evaluate()
    {
        object wc = GetData("WeaponCase");
        object w = GetData("Weapon");
        object p = GetData("Player");

        //No Player to attack
        if(p == null)
        {
            state = NodeState.FAILURE;
            return state;
        }
        
        //We already found a weaponCase
        if (wc != null)
        {
            state = NodeState.SUCCESS;
            return state;
        }

        //We already have a weapon
        if (w != null)
        {
            state = NodeState.FAILURE;
            return state;
        }


        WeaponCase closestCase = GetClosestWeaponCase();
        if (closestCase != null)
        {
            state = NodeState.SUCCESS;
            parent.parent.SetData("WeaponCase", closestCase);
        }
        else
        {
            state = NodeState.FAILURE;
        }

        return state;
    }

    private WeaponCase GetClosestWeaponCase()
    {
        WeaponCase closestCase = null;
        float closestDistance = float.MaxValue;
        foreach (WeaponCase weaponCase in weaponCases)
        {
            float distance = Vector3.Distance(weaponCase.transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestCase = weaponCase;
                closestDistance = distance;
            }
        }
        return closestCase;
    }
}
