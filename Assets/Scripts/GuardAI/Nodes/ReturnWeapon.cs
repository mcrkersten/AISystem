using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReturnWeapon : Node
{
    private NavMeshAgent agent;
    public ReturnWeapon(NavMeshAgent agent)
    {
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        Weapon weapon = null;
        WeaponCase weaponCase = null;

        object w = GetData("Weapon");
        object wc = GetData("WeaponCase");
        object p = GetData("Player");

        //We have no weapon
        if (w == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        if (wc == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        if (p != null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        weapon = (Weapon)w;
        weaponCase = (WeaponCase)wc;
        if (Vector3.Distance(agent.transform.position, weaponCase.TargetPosition.transform.position) < .2f)
        {
            weaponCase.ReturnWeapon(weapon);
            ClearData("Weapon");
            ClearData("WeaponCase");
            state = NodeState.SUCCESS;
            return state;
        };
        state = NodeState.FAILURE;
        return state;
    }
}
