using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public GameObject model;
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        transform.Translate(direction * movementSpeed * Time.deltaTime);

        if(direction.normalized.magnitude > 0.1f)
            model.transform.rotation = Quaternion.LookRotation(direction);
    }
}
