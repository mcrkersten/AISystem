using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : Node
{
    private Transform target;
    private NavMeshAgent agent;
    private NinjaManager ninjaManager;
    public FollowPlayer(NavMeshAgent agent ,Transform target)
    {
        this.agent = agent;
        this.ninjaManager = agent.transform.GetComponent<NinjaManager>();
        this.target = target;
    }

    public override NodeState Evaluate()
    {
        object hp = GetData("HidingPlace");
        if (hp != null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        ninjaManager.SetState(NinjaManager.NinjaState.Following);
        agent.stoppingDistance = ninjaManager.followDistance;
        agent.destination = target.position;
        state = NodeState.RUNNING;
        return state;
    }
}
