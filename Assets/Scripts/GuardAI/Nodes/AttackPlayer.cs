using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackPlayer : Node
{
    private NavMeshAgent agent;
    private GuardManager guard;
    private Transform target;
    private LayerMask layerMask;

    public AttackPlayer(NavMeshAgent agent, Transform target, LayerMask layerMask)
    {
        this.agent = agent;
        this.layerMask = layerMask;
        this.target = target;
        this.guard = agent.GetComponent<GuardManager>();
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


        if (CanSeePlayer())
        {
            if (agent.remainingDistance < guard.weapon.range)
            {
                agent.isStopped = true;
                guard.state = GuardManager.GuardState.AttackPlayer;
                guard.FireWeapon();
                state = NodeState.SUCCESS;
                return state;
            }
        }

        state = NodeState.FAILURE;
        return state;
    }

    private bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 direction = target.position - agent.transform.position;
        Debug.DrawRay(agent.transform.position, direction, Color.cyan);
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(agent.transform.position, direction, out hit, Mathf.Infinity, layerMask))
        {
            return true;
        }

        return false;
    }
}
