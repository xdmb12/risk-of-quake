using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed;
    public float damage;
    public Vector3 target;
    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        
        rb.velocity = transform.forward * speed;

        Destroy(gameObject, 7f);
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
