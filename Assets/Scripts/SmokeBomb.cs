using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SmokeBomb : MonoBehaviour
{
    [Range(0f,-1f)]
    public float throwSpeedReducer;
    private float position = 0;
    public float throwHeight;
    public Vector3 startPos;
    public Vector3 endPos;

    [Header("Particle Systems")]
    public ParticleSystem smokeTrail;
    public ParticleSystem smokeScreen;

    public Collider sphereCollider;

    private bool smokeIsActivated;

    public float life;
    private float actualLife;


    private void Update()
    {
        actualLife += Time.deltaTime;
        position += (Time.deltaTime /  (1f - throwSpeedReducer));
        position = Mathf.Clamp(position, 0f, 1f);
        this.transform.position = Parabola(startPos, endPos, throwHeight, position);

        if(position == 1f && !smokeIsActivated)
        {
            smokeIsActivated = true;
            ActivateSmoke();
        }

        if (actualLife > life)
            Destroy(this.gameObject);
    }

    public void ActivateSmoke()
    {
        smokeScreen.Emit(200);
        sphereCollider.enabled = true;
    }

    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
}
