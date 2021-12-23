using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public int hitpoints;
    public float movementSpeed;
    public GameObject model;
    public Transform respawnPosition;
    public GameObject deathParticlePrefab;
    public delegate void OnPlayerDeath();
    public static event OnPlayerDeath playerDies;

    public float respawnTimer;
    private float currentRespawnTime;

    private bool isDead = false;
    void Update()
    {
        if (!isDead)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
            transform.Translate(direction * movementSpeed * Time.deltaTime);

            if (direction.normalized.magnitude > 0.1f)
                model.transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            if(currentRespawnTime > 0)
            {
                currentRespawnTime -= Time.deltaTime;
            }
            else
            {
                Respawn();
            }
        }
    }

    public void Damage()
    {
        hitpoints -= 1;
        if(hitpoints == 0)
        {
            currentRespawnTime = respawnTimer;
            isDead = true;
            playerDies?.Invoke();
            this.model.SetActive(false);
            this.model.transform.localScale = Vector3.zero;
            Instantiate(deathParticlePrefab, this.transform.position, Quaternion.identity, null);
        }
    }

    public void Respawn()
    {
        this.model.SetActive(true);
        this.model.transform.DOScale(.5f, 1f).SetEase(Ease.OutBounce);
        this.transform.position = respawnPosition.position;
        isDead = false;
        hitpoints = 1;
    }
}
