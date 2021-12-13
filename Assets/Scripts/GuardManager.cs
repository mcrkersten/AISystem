using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GuardManager : MonoBehaviour
{
    public GuardState state;
    public Weapon weapon;
    public Transform weaponPosition;

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


    public enum GuardState
    {
        Patrol = 0,
        GoForWeapon,
        GoToPlayer,
        GoToLastKnowLocation,
        AttackPlayer,
        ReturnWeapon
    }
}
