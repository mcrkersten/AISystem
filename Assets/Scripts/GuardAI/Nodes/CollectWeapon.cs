using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CollectWeapon : Node
{
    private NavMeshAgent agent;
    public CollectWeapon(NavMeshAgent agent)
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
        object lp = GetData("LastPosition");

        //If We already have weapon
        if (w != null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        if (wc == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        if (p == null && lp == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        //If we have a weaponCase in our memory
        weaponCase = (WeaponCase)wc;
        if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            weapon = weaponCase.GrabWeapon();
            agent.GetComponent<GuardManager>().AssignWeapon(weapon);
            parent.parent.SetData("Weapon", weapon);
            state = NodeState.SUCCESS;
            return state;
        }
        return state;
    }
}
