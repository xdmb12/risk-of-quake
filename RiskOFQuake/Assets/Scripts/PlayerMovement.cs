using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] 
    public float moveSpeed;
    public float groundDrag;

    [Header("Jump")]
    public float jumpForce;
    //public float jumpCooldown;
    //private bool readyToJump;
    public bool doubleJump;
    public float airMultiplier;
    
    [Header("Dash")] 
    public float dashForce;
    public float dashCooldown;
    private bool readyToDash;
    
    [Header("Another stuff")]
    public Transform orientation;

    public TMP_Text groundedText;
    public TMP_Text velocityText;
    public TMP_Text stateText;
    
    [Header("Ground Check")] 
    // public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;
    

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;
    // public float velocity;
    // public bool isGround;
    
    public MovementState state;
    public enum MovementState
    {
        walking,
        wallrunning,
        air
    }

    public bool wallrunning;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ResetDash();
        // leftShoulder = false;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.5f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        if (grounded)
            rb.drag = groundDrag;
        else
        {
            rb.drag = 0;
        }

        groundedText.text = $"Ground is: {grounded}";
        velocityText.text = $"{rb.velocity.magnitude}";
        stateText.text = $"{state}";
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(grounded)
            {
                doubleJump = true;
                Jump();
            }
            else if (doubleJump && !wallrunning)
            {
                doubleJump = false;
                Jump();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && readyToDash)
        {
            readyToDash = false;
            
            Dash();
            
            Invoke(nameof(ResetDash), dashCooldown);
        }
    }
    
    private void StateHandler()
    {
        if (wallrunning)
        {
            state = MovementState.wallrunning;
        }
        
        if (grounded)
        {
            state = MovementState.walking;
        }
        
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        if(grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void Dash()
    {
        rb.AddForce(orientation.forward * dashForce * 20f, ForceMode.Impulse);
        rb.AddForce(transform.up * 2, ForceMode.Impulse);
    }

    // void ResetJump()
    // {
    //     readyToJump = true;
    //     doubleJump = true;
    // }

    void ResetDash()
    {
        readyToDash = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down);
        Gizmos.DrawRay(transform.position + Vector3.up, Vector3.right);
    }
}
