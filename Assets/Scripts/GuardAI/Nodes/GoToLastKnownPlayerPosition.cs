using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToLastKnownPlayerPosition : Node
{
    private NavMeshAgent agent;
    private GuardManager guard;
    private Transform target;
    private LayerMask layerMask;

    public GoToLastKnownPlayerPosition(NavMeshAgent agent, Transform target, LayerMask layerMask)
    {
        this.agent = agent;
        this.layerMask = layerMask;
        this.target = target;
        guard = agent.GetComponent<GuardManager>();
    }

    public override NodeState Evaluate()
    {
        object w = GetData("Weapon");
        object lp = GetData("LastPosition");
        object p = GetData("Player");

        //No need to go to last position
        if(p != null)
        {
            state = NodeState.FAILURE;
            return state;
        }

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
            state = NodeState.FAILURE;
            agent.autoBraking = true;
        }
        else
        {
            agent.isStopped = false;
            guard.state = GuardManager.GuardState.GoToLastKnowLocation;
            state = NodeState.RUNNING;
        }
        
        return state;
    }
}
