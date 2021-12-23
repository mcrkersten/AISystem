using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NeedToHide : Node
{
    private GameObject ally;
    private NavMeshAgent agent;
    private List<GuardManager> guardsToHideFrom = new List<GuardManager>();
    private Transform transform;
    private NinjaManager ninjaManager;

    public NeedToHide(GameObject ally , NavMeshAgent agent, List<GuardManager> guardsToHideFrom)
    {
        this.agent = agent;
        transform = agent.transform;
        this.guardsToHideFrom = guardsToHideFrom;
        this.ally = ally;
        this.ninjaManager = this.agent.GetComponent<NinjaManager>();
    }

    public override NodeState Evaluate()
    {
        List<GuardManager> attackingGuards = new List<GuardManager>();
        foreach (GuardManager guard in guardsToHideFrom)
            if(guard.State == GuardManager.GuardState.AttackPlayer || 
                guard.State == GuardManager.GuardState.GoToPlayer)
            {
                attackingGuards.Add(guard);
            }

        if(attackingGuards.Count != 0)
        {
            Debug.Log("NeedToHide");
            parent.SetData("AttackingGuards", attackingGuards);
            state = NodeState.SUCCESS;
            return state;
        }
        else
        {
            ClearData("HidingPlace");
            state = NodeState.FAILURE;
            return state;
        }
    }
}
