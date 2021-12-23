using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class GoToPlayerLocation : Node
{
    private NavMeshAgent agent;
    private GuardManager guard;

    public GoToPlayerLocation(NavMeshAgent agent, Transform target)
    {
        this.agent = agent;
        guard = agent.GetComponent<GuardManager>();
    }

    public override NodeState Evaluate()
    {
        object w = GetData("Weapon");
        object p = GetData("Player");

        //No Player to attack
        if (p == null)
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

        Transform playerTransform = (Transform)p;
        agent.destination = playerTransform.position;

        Weapon weapon = (Weapon)w;
        //If we are in range we can exit this node
        if(Vector3.Distance(agent.transform.position, playerTransform.position) < weapon.range)
        {
            agent.isStopped = false;
            state = NodeState.FAILURE;
        }
        else
        {
            guard.SetState(GuardManager.GuardState.GoToPlayer);
            state = NodeState.RUNNING;
        }

        return state;
    }
}
