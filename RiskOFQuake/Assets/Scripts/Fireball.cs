using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed;
    public float verticalForce;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        rb.AddForce(transform.up * verticalForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
