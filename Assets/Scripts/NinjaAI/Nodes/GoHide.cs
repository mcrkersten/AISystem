using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoHide : Node
{
    public NavMeshAgent agent;
    public NinjaManager ninjaManager;
    public GoHide(NavMeshAgent agent, NinjaManager ninjaManager)
    {
        this.agent = agent;
        this.ninjaManager = ninjaManager;
    }

    public override NodeState Evaluate()
    {
        object hp = GetData("HidingPlace");
        if(hp == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Transform hidingPlace = (Transform)hp;
        if(Vector3.Distance(agent.transform.position, hidingPlace.position) < .2f)
        {
            state = NodeState.SUCCESS;
            Debug.Log("Hiding");
        }
        else
        {
            state = NodeState.RUNNING;
            Debug.Log("GoHide");
            agent.destination = hidingPlace.position;
            agent.stoppingDistance = .1f;
            ninjaManager.SetState(NinjaManager.NinjaState.Hiding);
        }
        return state;
    }
}
