using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class WeaponCase : MonoBehaviour
{
    public Transform TargetPosition;
    public Weapon[] weapons;
    public Transform[] weaponPositions;

    public Weapon GrabWeapon()
    {
        Weapon selectedWeapon = null;
        for (int i = 0; i < weapons.Length; i++)
        {
            if(weapons[i] != null)
            {
                selectedWeapon = weapons[i];
                weapons[i] = null;
                break;
            }
        }
        return selectedWeapon;
    }

    public void ReturnWeapon(Weapon weapon)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if(weapons[i] == null)
            {
                weapon.transform.parent = weaponPositions[i];
                weapon.transform.DOLocalMove(Vector3.zero, 1f).SetEase(Ease.InOutQuint);
                weapon.transform.DOLocalRotate(Vector3.zero, 1f).SetEase(Ease.InOutQuint);
            }
        }
    }
}
