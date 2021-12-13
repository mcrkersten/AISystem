using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReturnToWeaponCase : Node
{
    private NavMeshAgent agent;
    public ReturnToWeaponCase(NavMeshAgent agent)
    {
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        object wc = GetData("WeaponCase");
        object w = GetData("Weapon");
        object p = GetData("Player");

        if (wc == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        //We have no weapon
        if (w == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        if (p != null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        WeaponCase weaponCase = (WeaponCase)wc;
        agent.destination = weaponCase.TargetPosition.transform.position;

        if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            agent.autoBraking = true;
            state = NodeState.FAILURE;
        }
        else
        {
            agent.GetComponent<GuardManager>().state = GuardManager.GuardState.ReturnWeapon;
            state = NodeState.RUNNING;
        }

        return state;
    }
}
