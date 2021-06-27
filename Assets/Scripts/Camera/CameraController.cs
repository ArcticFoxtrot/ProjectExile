using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    private float horizontal;
    private float vertical;

    private float cameraHorizontalInput;
    private float cameraVerticalInput;

    private CinemachineVirtualCamera virtualCamera;

    public Vector2 cameraMovement;



    [SerializeField] Transform cameraFollow;
    [SerializeField] float cameraPanSpeed = 1f;
    [SerializeField] float cameraTiltSpeed = 1f;
    [SerializeField] float cameraTiltUpperLimit = 340f;
    [SerializeField] float cameraTiltLowerLimit = 40f;
    [SerializeField] PlayerController playerController;

    private Transform currentCameraFollow;

    private void Start() {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if(!virtualCamera || virtualCamera == null){
            Debug.Log("No virtual camera found in CameraController");
        } else {
            currentCameraFollow = cameraFollow;
            virtualCamera.m_LookAt = currentCameraFollow;
            virtualCamera.m_Follow = currentCameraFollow;
        }
    }

    private void Update() {
        cameraHorizontalInput = Input.GetAxis("RightStick Horizontal");
        cameraVerticalInput = Input.GetAxis("RightStick Vertical");
        horizontal = cameraHorizontalInput;
        vertical = cameraVerticalInput;
        PanCamera(horizontal);
        TiltCamera(vertical);

    }

    public void SetLookAtTarget(Transform t){
        Debug.Log("Got here and setting t as target " + t.name);
        virtualCamera.m_LookAt = t;
        currentCameraFollow = t;
    }

    public void SetFollowTarget(Transform t){
        virtualCamera.m_Follow = t;
        currentCameraFollow = t;
    }

    public Transform GetCameraFollowTarget(){
        return cameraFollow;
    }

    private void TiltCamera(float v)
    {
        currentCameraFollow.transform.rotation *= Quaternion.AngleAxis(v * cameraTiltSpeed* Time.deltaTime, Vector3.right );

        Vector3 angles = currentCameraFollow.transform.localEulerAngles;
        angles.z = 0f;

        float angle = currentCameraFollow.transform.localEulerAngles.x;

        if(angle > 180 && angle < cameraTiltUpperLimit){
            angles.x = cameraTiltUpperLimit;
        } else if(angle < 180 && angle > cameraTiltLowerLimit){
            angles.x = cameraTiltLowerLimit;
        }

        currentCameraFollow.transform.localEulerAngles = angles;
    }

    private void PanCamera(float h)
    {
        currentCameraFollow.transform.rotation *= Quaternion.AngleAxis(h * cameraPanSpeed * Time.deltaTime, Vector3.up);
    }

}
