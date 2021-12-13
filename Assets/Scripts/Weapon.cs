using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float range;
    public float cooldownTime;
    private float currentCooldown;

    public void Fire()
    {
        if(currentCooldown > cooldownTime)
        {
            currentCooldown = 0f;
        }
    }

    private void Update()
    {
        if(currentCooldown < cooldownTime)
            currentCooldown += Time.deltaTime;
    }
}
