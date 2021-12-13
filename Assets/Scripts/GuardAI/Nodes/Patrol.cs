
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : Node
{
    private Transform[] waypoints;
    private int destPoint = 0;
    private NavMeshAgent agent;

    public Patrol(NavMeshAgent agent, Transform[] waypoints)
    {
        this.waypoints = waypoints;
        this.agent = agent;
        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (waypoints.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = waypoints[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % waypoints.Length;
    }


    public override NodeState Evaluate()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.2f)
            GotoNextPoint();

        state = NodeState.RUNNING;
        agent.GetComponent<GuardManager>().state = GuardManager.GuardState.Patrol;
        agent.autoBraking = false;
        return state;
    }
}