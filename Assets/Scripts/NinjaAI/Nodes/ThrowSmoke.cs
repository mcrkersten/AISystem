using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class ThrowSmoke : Node
{
    private NavMeshAgent agent;
    private Transform player;
    private GameObject smokeBombPrefab;
    private NinjaManager ninjaManager;
    public ThrowSmoke(NavMeshAgent agent, NinjaManager ninjaManager, Transform player)
    {
        this.agent = agent;
        this.player = player;
        this.ninjaManager = ninjaManager;
    }

    public override NodeState Evaluate()
    {
        object ag = GetData("AttackingGuards");
        if (ag == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        object hp = GetData("HidingPlace");
        if (hp == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Transform hidingPlace = (Transform)hp;

        if(Vector3.Distance(hidingPlace.position, agent.transform.position) < .2f)
        {
            List<GuardManager> attackingGuards = (List<GuardManager>)ag;
            foreach (GuardManager p in attackingGuards)
            {
                if (ninjaManager.CanFireSmokebomb)
                {
                    Vector3 position = CalculateSmokePositions(p);
                    ninjaManager.ThrowSmokebomb(position);

                    state = NodeState.SUCCESS;
                    Debug.Log("Throw grenade");
                    return state;
                }
                else
                {
                    ninjaManager.SetState(NinjaManager.NinjaState.Reloading);
                    state = NodeState.RUNNING;
                    return state;
                }
            }
        }
        state = NodeState.FAILURE;
        return state;
    }

    private Vector3 CalculateSmokePositions(GuardManager guard)
    {

        Vector3 pos = Vector3.Lerp(guard.transform.position, player.position, .5f);
        return pos;
    }
}

public struct GuardStruct
{
    public GameObject gameObject;
    public GuardManager.GuardState state;
    public GuardStruct(GuardManager.GuardState state, GameObject gameObject)
    {
        this.state = state;
        this.gameObject = gameObject;
    }
}
