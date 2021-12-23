using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToWeaponCase : Node
{
    private NavMeshAgent agent;

    public GoToWeaponCase(NavMeshAgent agent)
    {
        this.agent = agent;
    }


    public override NodeState Evaluate()
    {
        object wc = GetData("WeaponCase");
        object w = GetData("Weapon");

        if (w != null)//We have weapon
        {
            state = NodeState.FAILURE;
            return state;
        }

        if(wc == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        WeaponCase target = (WeaponCase)GetData("WeaponCase");
        agent.destination = target.TargetPosition.transform.position;
        if (Vector3.Distance(agent.transform.position, target.TargetPosition.position) < 0.2f)
        {
            state = NodeState.SUCCESS;
        }
        else
        {
            agent.GetComponent<GuardManager>().SetState(GuardManager.GuardState.GoForWeapon);
            state = NodeState.RUNNING;
        }

        return state;
    }
}
