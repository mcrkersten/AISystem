using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SearchForPlayer : Node
{
    private Transform transform;
    private Transform target;
    private GuardManager guardManager;

    private LineRenderer visionLineRenderer;
    private LineRenderer navigationLineRenderer;
    private Vector3 smokePosition;

    public SearchForPlayer(Transform transform, Transform target)
    {
        this.transform = transform;
        this.target = target;
        guardManager = this.transform.GetComponent<GuardManager>();
        visionLineRenderer = guardManager.visionLineRenderer;
        visionLineRenderer.positionCount = 2;

        navigationLineRenderer = guardManager.lastPositionLineRenderer;
        navigationLineRenderer.enabled = false;
        navigationLineRenderer.startColor = guardManager.lineRendereColorHit;
        navigationLineRenderer.endColor = guardManager.lineRendereColorHit;
    }

    public override NodeState Evaluate()
    {
        Vector3 offset = Vector3.up * .5f;
        visionLineRenderer.SetPosition(0, transform.position + offset);
        navigationLineRenderer.SetPosition(0, transform.position + offset);
        DrawNavigation();

        RaycastHit hit;
        Vector3 direction = (target.position + offset) - (transform.position + offset);
        if (Physics.Raycast(transform.position + offset, direction, out hit, Mathf.Infinity))
        {
            visionLineRenderer.SetPosition(1, hit.point);


            if (hit.transform.CompareTag("Player"))
            {
                OnPlayerTag();
                state = NodeState.SUCCESS;
                return state;
            }
            else if (hit.transform.CompareTag("Smoke"))
            {
                OnSmokeTag(hit);
                state = NodeState.SUCCESS;
                return state;
            }

            if (guardManager.blindedBySmoke)
            {
                guardManager.blindedBySmoke = !CheckIfSmokeIsCleared();
            }
            else
            {
                OnNoTag();
                state = NodeState.SUCCESS;
                return state;
            }
        }

        return state;
    }

    private void OnPlayerTag()
    {
        visionLineRenderer.startColor = guardManager.lineRendereColorHit;
        visionLineRenderer.endColor = guardManager.lineRendereColorHit;

        parent.parent.SetData("Player", target);
        ClearData("LastPosition");
        guardManager.seenPlayerLastFrame = true;
        guardManager.blindedBySmoke = false;
        guardManager.playerDied = false;
    }

    private void OnSmokeTag(RaycastHit hit)
    {
        ClearData("Player");
        guardManager.blindedBySmoke = true;
        guardManager.SetState(GuardManager.GuardState.Blinded);
        visionLineRenderer.startColor = guardManager.lineRenderColorStart;
        visionLineRenderer.endColor = guardManager.lineRenderColorEnd;

        if (guardManager.seenPlayerLastFrame && !guardManager.playerDied)
        {
            smokePosition = hit.point;
            parent.parent.SetData("LastPosition", target.position);
            navigationLineRenderer.enabled = true;
        }
        guardManager.seenPlayerLastFrame = false;
    }

    private void OnNoTag()
    {
        visionLineRenderer.startColor = guardManager.lineRenderColorStart;
        visionLineRenderer.endColor = guardManager.lineRenderColorEnd;

        if (guardManager.seenPlayerLastFrame && !guardManager.playerDied)
        {
            parent.parent.SetData("LastPosition", target.position);
            navigationLineRenderer.enabled = true;
        }

        guardManager.blindedBySmoke = false;
        guardManager.seenPlayerLastFrame = false;
        ClearData("Player");
    }

    private bool CheckIfSmokeIsCleared()
    {
        Vector3 offset = Vector3.up * .5f;
        RaycastHit hit;
        Vector3 direction = (smokePosition) - (transform.position + offset);
        Debug.DrawRay(transform.position + offset, direction, Color.blue);
        if (Physics.Raycast(transform.position + offset, direction, out hit, Mathf.Infinity))
        {
            Debug.DrawLine(transform.position + offset, hit.point, Color.green);
            if (hit.transform.CompareTag("Smoke"))
            {
                return false;
            }
        }
        return true;
    }

    private void DrawNavigation()
    {
        int pointCount = guardManager.navMeshAgent.path.corners.Length;
        navigationLineRenderer.positionCount = pointCount;
        for (int i = 0; i < pointCount; i++)
        {
            navigationLineRenderer.SetPosition(i, guardManager.navMeshAgent.path.corners[i]);
        }
    }
}
