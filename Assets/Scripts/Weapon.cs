using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float range;
    public float cooldownTime;
    private float currentCooldown;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    public void Fire()
    {
        if(currentCooldown > cooldownTime)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation ,null);
            currentCooldown = 0f;
        }
    }

    private void Update()
    {
        if(currentCooldown < cooldownTime)
            currentCooldown += Time.deltaTime;
    }
}
