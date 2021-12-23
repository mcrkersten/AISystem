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
        object lp = GetData("LastPosition");

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

        if (p != null || lp != null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        WeaponCase weaponCase = (WeaponCase)wc;
        agent.destination = weaponCase.TargetPosition.transform.position;

        if (Vector3.Distance(agent.transform.position, weaponCase.TargetPosition.transform.position) < .2f)
        {
            agent.autoBraking = true;
            state = NodeState.FAILURE;
        }
        else
        {
            agent.isStopped = false;
            agent.GetComponent<GuardManager>().SetState(GuardManager.GuardState.ReturnWeapon);
            state = NodeState.RUNNING;
        }
        return state;
    }
}
