using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")] 
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;

    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float maxWallRunTime;
    private float wallRunTimer;
    
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")] 
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")] 
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("Gravity")] 
    public bool useGravity;
    public float gravityCounterForce;
    
    [Header("Camera")]
    [SerializeField] private CinemachineCameraOffset freeLookCamera;
    [SerializeField] private float cameraHorizontalPosition;

    [Header("References")] 
    public Transform orientation;
    private PlayerMovement pm;
    private Rigidbody rb;
    private float startCameraPosition;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if(pm.wallrunning)
            WallRunningMovement();
        
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position + Vector3.up, orientation.right, out rightWallhit,
            wallCheckDistance, whatIsWall);
        
        wallLeft = Physics.Raycast(transform.position + Vector3.up, -orientation.right, out leftWallhit,
            wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if ((wallRight || wallLeft) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            if(!pm.wallrunning)
                StartWallRun();

            if (wallRunTimer > 0)
                wallRunTimer -= Time.deltaTime;

            if (wallRunTimer <= 0)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                WallJump();
            }
        }
        
        else if (exitingWall)
        {
            if(pm.wallrunning)
                StopWallRun();

            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0)
                exitingWall = false;
        }
        else
        {
            if(pm.wallrunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        startCameraPosition = freeLookCamera.m_Offset.x;
        
        pm.wallrunning = true;
        CameraShoulderSwitch(wallRight ? -cameraHorizontalPosition : cameraHorizontalPosition);

        wallRunTimer = maxWallRunTime;
        
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    }
    
    private void WallRunningMovement()
    {
        rb.useGravity = false;

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }
        
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
        
        if(!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }
    }
    
    private void StopWallRun()
    {
        pm.wallrunning = false;
        rb.useGravity = true;
        freeLookCamera.m_Offset.x = startCameraPosition;
    }

    private void WallJump()
    {
        exitingWall = true;
        pm.doubleJump = true;
        exitWallTimer = exitWallTime;
        
        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
    
    void CameraShoulderSwitch(float position)
    {
        freeLookCamera.m_Offset.x = position;
    }
}
