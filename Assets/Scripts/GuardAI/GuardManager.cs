using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using TMPro;


public class GuardManager : MonoBehaviour
{

    private GuardState state;
    public GuardState State { get { return state; } }

    public NavMeshAgent navMeshAgent;
    public Weapon weapon;
    public Transform weaponPosition;

    [Header("Navigation visualization")]
    public Color lineRenderColorStart;
    public Color lineRenderColorEnd;
    public Color lineRendereColorHit;
    public LineRenderer visionLineRenderer;
    public LineRenderer lastPositionLineRenderer;

    public Cloud cloudElement;

    public bool blindedBySmoke;
    public bool seenPlayerLastFrame;
    public bool playerDied;

    public void FireWeapon()
    {
        weapon.Fire();
    }

    public void AssignWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        this.weapon.transform.parent = weaponPosition;
        this.weapon.transform.DOLocalMove(Vector3.zero, 1f).SetEase(Ease.InOutQuint);
        this.weapon.transform.DOLocalRotate(Vector3.zero, 1f).SetEase(Ease.InOutQuint);
    }

    public void SetState(GuardState state)
    {
        if(state != this.state)
        {
            this.state = state;
            switch (state)
            {
                case GuardState.Patrol:
                    cloudElement.SetText("Patrolling");
                    break;
                case GuardState.GoForWeapon:
                    cloudElement.SetText("Collect weapon");
                    break;
                case GuardState.GoToPlayer:
                    cloudElement.SetText("Go to player");
                    break;
                case GuardState.GoToLastKnowLocation:
                    cloudElement.SetText("Go to last location");
                    break;
                case GuardState.AttackPlayer:
                    cloudElement.SetText("Attack player");
                    break;
                case GuardState.ReturnWeapon:
                    cloudElement.SetText("Returning weapon");
                    break;
                case GuardState.Blinded:
                    cloudElement.SetText("Blinded");
                    break;
                default:
                    return;
            }
        }
    }

    public enum GuardState
    {
        noState = 0,
        Patrol,
        GoForWeapon,
        GoToPlayer,
        GoToLastKnowLocation,
        AttackPlayer,
        ReturnWeapon,
        Blinded,
    }
}
