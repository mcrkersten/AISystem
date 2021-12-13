using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchForPlayer : Node
{
    private Transform transform;
    private Transform target;
    private LayerMask layerMask;

    public SearchForPlayer(Transform transform, Transform target, LayerMask layerMask)
    {
        this.transform = transform;
        this.target = target;
        this.layerMask = layerMask;
    }

    public override NodeState Evaluate()
    {


        RaycastHit hit;
        Vector3 direction = target.position - transform.position;
        Debug.DrawRay(transform.position, direction, Color.red);
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, layerMask))
        {
            parent.SetData("Player", target);
            parent.SetData("LastPosition", target.position);
            state = NodeState.SUCCESS;
        }
        else
        {
            ClearData("Player");
            state = NodeState.SUCCESS;
        }
        return state;
    }
}
