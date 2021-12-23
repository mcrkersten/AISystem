using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Blinded : Node
{
    private GuardManager guardManager;
    private NavMeshAgent agent;
    public Blinded(NavMeshAgent agent)
    {
        this.guardManager = agent.GetComponent<GuardManager>();
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        if (guardManager.blindedBySmoke)
        {
            agent.isStopped = true;
            state = NodeState.FAILURE;
        }
        else
        {
            state = NodeState.SUCCESS;
        }
        return state;
    }
}
