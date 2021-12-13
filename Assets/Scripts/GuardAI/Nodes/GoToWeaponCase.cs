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
        object w = GetData("Weapon");
        if (w != null)//We have weapon
        {
            state = NodeState.SUCCESS;
            return state;
        }

        object wc = GetData("WeaponCase");
        if(wc != null)//We found weaponCase
        {
            WeaponCase target = (WeaponCase)GetData("WeaponCase");
            agent.destination = target.TargetPosition.transform.position;
            if (!agent.pathPending && agent.remainingDistance < 0.2f)
            {
                state = NodeState.SUCCESS;
            }
            else
            {
                agent.GetComponent<GuardManager>().state = GuardManager.GuardState.GoForWeapon;
                state = NodeState.RUNNING;
            }
        }
        return state;
    }
}
