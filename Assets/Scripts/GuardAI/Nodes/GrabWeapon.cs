using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrabWeapon : Node
{
    private NavMeshAgent agent;
    public GrabWeapon(NavMeshAgent agent)
    {
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        Weapon weapon = null;
        WeaponCase weaponCase = null;
        object weaponData = GetData("Weapon");
        object WeaponCaseData = GetData("WeaponCase");

        //If We already have weapon
        if (weaponData != null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        //If we have a weaponCase in our memory
        if (WeaponCaseData != null)
        {
            weaponCase = (WeaponCase)WeaponCaseData;
            if (!agent.pathPending && agent.remainingDistance < 0.2f)
            {
                weapon = weaponCase.GrabWeapon();
                agent.GetComponent<GuardManager>().AssignWeapon(weapon);
                parent.parent.SetData("Weapon", weapon);
                state = NodeState.SUCCESS;
                return state;
            }
            else
            {
                state = NodeState.FAILURE;
                return state;
            }
        }
        return state;
    }
}
