using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackPlayer : Node
{
    private NavMeshAgent agent;
    private GuardManager guard;
    private Transform target;

    public AttackPlayer(NavMeshAgent agent, Transform target)
    {
        this.agent = agent;
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
            if (Vector3.Distance(target.position, agent.transform.position) < guard.weapon.range)
            {
                FaceTarget(target.position);
                agent.isStopped = true;
                guard.SetState(GuardManager.GuardState.AttackPlayer);
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
        if (Physics.Raycast(agent.transform.position, direction, out hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - agent.transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, .5f);
    }
}
