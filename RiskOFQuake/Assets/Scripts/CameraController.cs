using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform playerModel;
    [SerializeField] private Transform orientation;
    private WallRunning _wallRunning;

    [Header("Camera")]
    [SerializeField] public CinemachineCameraOffset freeLookCamera;
    [SerializeField] public float rightHorizontalPosition;
    private float cameraHorizontalPosition;
    public bool leftShoulder;

    [SerializeField] private float rotationSpeed;
    void Start()
    {
        _wallRunning = GetComponent<WallRunning>();
        CameraShoulderSwitch(rightHorizontalPosition);
        leftShoulder = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        var localRotation = orientation.localRotation;
        float x = localRotation.eulerAngles.x;
        float z = localRotation.eulerAngles.z;
        
        localRotation = Quaternion.Euler(x, mainCamera.localRotation.eulerAngles.y, z);
        orientation.localRotation = localRotation;

        playerModel.localRotation = Quaternion.Lerp(playerModel.localRotation, localRotation, rotationSpeed);
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            CameraShoulderSwitch(leftShoulder ? -rightHorizontalPosition : rightHorizontalPosition);

            leftShoulder = !leftShoulder;
        }

    }
    
    public void CameraShoulderSwitch(float position)
    {
        freeLookCamera.m_Offset.x = position;
    }
}
