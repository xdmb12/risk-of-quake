using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("References")] 
    public Transform orientation;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Dashing")] 
    public float dashForce;
    private float dashUpwardForce;
    public float dashDuration;

    [Header("Cooldown")] 
    public float dashCd;
    private float dashCdTimer;


    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && pm.canMove)
        {
            Dash();
        }

        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }

    private void Dash()
    {
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;

        pm.dashing = true;
        
        Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);
        
        Invoke(nameof(ResetDash), dashDuration);
    }

    private Vector3 delayedForceToApply;

    private void DelayedDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }
    
    private void ResetDash()
    {
        pm.dashing = false;
    }
}
