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
    private bool isZTargeting = false;
    private float zTargetY = 0f;

    private CinemachineVirtualCamera virtualCamera;

    public Vector2 cameraMovement;



    [SerializeField] Transform cameraFollow;
    [SerializeField] float cameraPanSpeed = 1f;
    [SerializeField] float cameraTiltSpeed = 1f;
    [SerializeField] float cameraTiltUpperLimit = 340f;
    [SerializeField] float cameraTiltLowerLimit = 40f;
    [SerializeField] float zTargetingMaxRotation = 90f;
    public float zYMax;
    [SerializeField] float zTargetingMinRotation = -90f;
    public float zYMin;
    public float yAngle;
    [SerializeField] PlayerController playerController;
    [SerializeField] Transform playerLookAt;

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

    public void SetIsZTargeting(bool t){
        isZTargeting = t;
        if(isZTargeting == true){
            //reset the start rotation of the targeting object so that the rotation limits can be adjusted

        }
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
        if(!isZTargeting){
            currentCameraFollow.transform.rotation *= Quaternion.AngleAxis(h * cameraPanSpeed * Time.deltaTime, Vector3.up);  
        }
        if(isZTargeting){
            HandleZTargetingLookAtAngle(h);
        }
       
    }

    private void HandleZTargetingLookAtAngle(float h){
        //stay in an 180 arc of the player
        //gets angle between camera and the player look at object's forward by checking the cross product for location and then limiting based on that
        //TODO fix for "janky" movement of the camera, dampening of rotation?
        Vector3 cross = Vector3.Cross(virtualCamera.transform.forward, playerLookAt.forward);
        if(cross.y > 0f){
            Debug.Log("CrossY is bigger than 0f");
            //on the right side of the lookAt
            float angleBetween = Vector3.Angle(virtualCamera.transform.forward, playerLookAt.forward);
            if(angleBetween < 90f && h > 0){
                Debug.Log("Reached upper limit");
                //cannot rotate to the right
            } else {
                currentCameraFollow.transform.rotation *= Quaternion.AngleAxis(h * cameraPanSpeed * Time.deltaTime, Vector3.up); 
            }
        } 
        if(cross.y < 0f){
            Debug.Log("CrossY is smaller than 0f");
            //on the left side of the lookAt
            float angleBetween = Vector3.Angle(virtualCamera.transform.forward, playerLookAt.forward);
            if(angleBetween < 90f && h < 0){
                Debug.Log("Reached lower limit");
                //cannot rotate to the left
            } else {
                currentCameraFollow.transform.rotation *= Quaternion.AngleAxis(h * cameraPanSpeed * Time.deltaTime, Vector3.up); 
            }
        }

    }

}
