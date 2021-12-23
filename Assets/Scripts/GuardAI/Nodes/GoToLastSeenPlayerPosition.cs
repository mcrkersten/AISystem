using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToLastSeenPlayerPosition : Node
{
    private NavMeshAgent agent;
    private GuardManager guard;

    public GoToLastSeenPlayerPosition(NavMeshAgent agent, Transform target)
    {
        this.agent = agent;
        guard = agent.GetComponent<GuardManager>();
    }

    public override NodeState Evaluate()
    {
        object w = GetData("Weapon");
        object lp = GetData("LastPosition");

        //No Player to attack
        if (lp == null)
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


        Vector3 position = (Vector3)lp;
        agent.destination = position;

        if (Vector3.Distance(agent.transform.position, position) < .2f)
        {
            ClearData("LastPosition");
            guard.lastPositionLineRenderer.enabled = false;
            state = NodeState.FAILURE;
            agent.autoBraking = true;
        }
        else
        {
            agent.isStopped = false;
            guard.SetState(GuardManager.GuardState.GoToLastKnowLocation);
            state = NodeState.RUNNING;
        }
        
        return state;
    }
}
