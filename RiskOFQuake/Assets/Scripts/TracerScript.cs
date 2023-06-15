using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracerScript : MonoBehaviour
{

    public Vector3 target;
    public float speed;
    
    private void FixedUpdate()
    {
        Tracer();
        if (transform.position == target)
        {
            Destroy(gameObject);
        }
    }

    void Tracer()
    {
        transform.position += transform.forward * speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
