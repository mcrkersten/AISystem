using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class SearchHidingPlaces : Node
{
    private List<Transform> hidingPlaces = new List<Transform>();
    private NavMeshAgent agent;
    private NinjaManager ninjaManager;
    public SearchHidingPlaces(NavMeshAgent agent, List<Transform> hidingPlaces)
    {
        this.agent = agent;
        this.hidingPlaces = hidingPlaces;
        this.ninjaManager = agent.transform.GetComponent<NinjaManager>();
    }

    public override NodeState Evaluate()
    {
        object ag = GetData("AttackingGuards");
        if (ag == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        List<GuardManager> attackingGuards = (List<GuardManager>)ag;
        List<Transform> goodHidingPlaces = GetHidingPlaces(attackingGuards);
        Transform bestHidingPlace = GetBestHidingPlace(goodHidingPlaces);

        if(bestHidingPlace != null)
        {
            Debug.Log("SearchHidingPlaces");
            state = NodeState.SUCCESS;
            parent.parent.parent.SetData("HidingPlace", bestHidingPlace);
        }
        else
        {
            state = NodeState.FAILURE;
        }
        return state;
    }

    private Transform GetBestHidingPlace(List<Transform> places)
    {
        float smallestDistance = float.MaxValue;
        Transform bestHidingPlace = null;
        foreach (Transform place in places)
        {
            float distance = Vector3.Distance(place.position, this.ninjaManager.player.transform.position);
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                bestHidingPlace = place;
            }
        }
        Debug.Log("Best " + bestHidingPlace);
        return bestHidingPlace;
    }

    private List<Transform> GetHidingPlaces(List<GuardManager> attackingGuards)
    {
        List<Transform> goodHidingPlaces = new List<Transform>();
        foreach (GuardManager guard in attackingGuards)
        {
            foreach (Transform hidingPlace in hidingPlaces)
            {
                SpriteRenderer spr = hidingPlace.GetComponent<SpriteRenderer>();
                if (!GuardCanSeePosition(guard.transform, hidingPlace))
                {
                    goodHidingPlaces.Add(hidingPlace);
                    spr.color = ninjaManager.hiddenHidingPlace;
                    hidingPlace.DOScale(1f, .5f).SetEase(Ease.OutBounce);
                }
                else
                {
                    spr.color = ninjaManager.exposedHidingPlace;
                    hidingPlace.DOScale(.5f, .5f).SetEase(Ease.OutBounce);
                }
            }
        }
        return goodHidingPlaces;
    }

    private bool GuardCanSeePosition(Transform guard, Transform hidingPlace)
    {
        Vector3 offset = Vector3.up * .5f;
        RaycastHit hit;
        Vector3 direction = (guard.position + offset) - (hidingPlace.position + offset);
        if (Physics.Raycast(hidingPlace.position + offset, direction, out hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag("Guard"))
            {
                Debug.DrawLine(hidingPlace.position + offset, hit.point, Color.red, 5f);
                return true;
            }
        }
        return false;
    }
}
