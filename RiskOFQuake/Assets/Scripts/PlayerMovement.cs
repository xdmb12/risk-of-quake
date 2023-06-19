using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] 
    public float runningSpeed;
    public float maxRunningSpeed;
    public float groundDrag;
    private float playerSpeed;
    public float coefDecreaseSpeed;
    public float stopVelocity;

    public float dashSpeed;

    [Header("Jump")]
    public float jumpForce;
    public float gravity;

    public int availableDoubleJumps;
    public int maxAvailableDoubleJumps;
    //public float jumpCooldown;
    //private bool readyToJump;
    [HideInInspector] public bool doubleJump;
    public float airMultiplier;

    [Header("Another stuff")]
    public Transform orientation;

    public TMP_Text groundedText;
    public TMP_Text velocityText;
    public TMP_Text stateText;
    public Animator animator;
    
    [Header("Ground Check")] 
    // public float playerHeight;
    public LayerMask whatIsGround;
    [HideInInspector] public bool grounded;
    

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;
    private CharacterController controller;

    private MovementState state;
    private enum MovementState
    {
        walking,
        wallrunning,
        dashing,
        air
    }

    [HideInInspector] public bool dashing;
    [HideInInspector] public bool wallrunning;

    public bool cooldownMoving;
    public bool canMove;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canMove = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.5f, whatIsGround);

        MyInput();
        //SpeedControl();
        StateHandler();
        Animations();

        // if (state == MovementState.walking || state == MovementState.wallrunning)
        // {
        //     rb.drag = groundDrag;
        // }
        // else
        // {
        //     rb.drag = 0;
        // }

        if (grounded || wallrunning)
        {
            availableDoubleJumps = maxAvailableDoubleJumps;
        }
        
        if(canMove)
        {
            runningSpeed -= Time.deltaTime * coefDecreaseSpeed;
        }

        groundedText.text = $"Ground is: {grounded}";
        //velocityText.text = $"{rb.velocity.magnitude}";
        stateText.text = $"{state}";
    }

    private void FixedUpdate()
    {
        if(!cooldownMoving)
            MovePlayer();

        // if (runningSpeed <= 0)
        // {
        //     canMove = false;
        // }
    }

    private void Animations()
    {
        animator.SetFloat("InputY", verticalInput);
        animator.SetFloat("InputX", horizontalInput);
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // if (Input.GetKeyDown(KeyCode.Space) && canMove)
        // {
        //     if(grounded)
        //     {
        //         Jump();
        //     }
        //     else if (availableDoubleJumps >= 1 && !wallrunning )
        //     {
        //         availableDoubleJumps--;
        //         Jump();
        //     }
        // }
    }

    private void StateHandler()
    {
        if (dashing)
        {
            state = MovementState.dashing;
            playerSpeed = dashSpeed;
        }
        
        else if (wallrunning)
        {
            state = MovementState.wallrunning;
        }
        
        else if (grounded)
        {
            state = MovementState.walking;
            playerSpeed = runningSpeed;
        }
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        float currentY = moveDirection.y;

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDirection.y = jumpForce;
            }
            else
            {
                moveDirection.y = currentY;
            }
        }
        else
        {
            moveDirection.y = gravity;
        }
        

        controller.Move(moveDirection * playerSpeed);
    }

    // void SpeedControl()
    // {
    //     Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    //
    //     if (flatVel.magnitude > playerSpeed)
    //     {
    //         Vector3 limitedVel = flatVel.normalized * playerSpeed;
    //         
    //         rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
    //     }
    // }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down);
        Gizmos.DrawRay(transform.position + Vector3.up, Vector3.right);
    }
}
