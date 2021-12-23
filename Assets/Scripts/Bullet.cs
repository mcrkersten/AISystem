using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public float bulletVelocity;
    private void Start()
    {
        rb.AddForce(transform.forward * bulletVelocity, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("Damage");
            collision.transform.GetComponent<PlayerController>().Damage();
            collision.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        Destroy(this.gameObject);
    }
}
