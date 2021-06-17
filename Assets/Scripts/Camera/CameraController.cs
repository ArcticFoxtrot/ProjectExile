using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private float horizontal;
    private float vertical;

    private float cameraHorizontalInput;
    private float cameraVerticalInput;

    public Vector2 cameraMovement;

    [SerializeField] Transform cameraFollow;
    [SerializeField] float cameraPanSpeed = 1f;
    [SerializeField] float cameraTiltSpeed = 1f;
    [SerializeField] float cameraTiltUpperLimit = 340f;
    [SerializeField] float cameraTiltLowerLimit = 40f;
    [SerializeField] PlayerController playerController;

    private void Update() {
        cameraHorizontalInput = Input.GetAxis("RightStick Horizontal");
        cameraVerticalInput = Input.GetAxis("RightStick Vertical");
        horizontal = cameraHorizontalInput;
        vertical = cameraVerticalInput;
        PanCamera(horizontal);
        TiltCamera(vertical);

    }

    private void TiltCamera(float v)
    {
        cameraFollow.transform.rotation *= Quaternion.AngleAxis(v * cameraTiltSpeed* Time.deltaTime, Vector3.right );

        Vector3 angles = cameraFollow.transform.localEulerAngles;
        angles.z = 0f;

        float angle = cameraFollow.transform.localEulerAngles.x;

        if(angle > 180 && angle < cameraTiltUpperLimit){
            angles.x = cameraTiltUpperLimit;
        } else if(angle < 180 && angle > cameraTiltLowerLimit){
            angles.x = cameraTiltLowerLimit;
        }

        cameraFollow.transform.localEulerAngles = angles;
    }

    private void PanCamera(float h)
    {
        cameraFollow.transform.rotation *= Quaternion.AngleAxis(h * cameraPanSpeed * Time.deltaTime, Vector3.up);
    }

}
