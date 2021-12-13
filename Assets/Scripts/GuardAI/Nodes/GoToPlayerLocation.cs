using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class GoToPlayerLocation : Node
{
    private NavMeshAgent agent;
    private GuardManager guard;
    private Transform target;
    private LayerMask layerMask;
    public GoToPlayerLocation(NavMeshAgent agent, Transform target, LayerMask layerMask)
    {
        this.agent = agent;
        this.layerMask = layerMask;
        this.target = target;
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
            guard.state = GuardManager.GuardState.GoToPlayer;
            state = NodeState.FAILURE;
        }
        else
        {
            Debug.Log("Go to player");
            state = NodeState.RUNNING;
        }

        return state;
    }
}
