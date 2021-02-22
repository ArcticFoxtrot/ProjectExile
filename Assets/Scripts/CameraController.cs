using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    private float horizontal;
    private float vertical;
    private float cameraRotate = 0f;

    [SerializeField] Transform cameraFollow;
    [SerializeField] float cameraRotationSpeed = 1f;
    [SerializeField] float cameraHorizontalSpeed = 1f;

    private void OnCameraMovement(InputValue value){
        Debug.Log("Camera movement! " + value.Get<Vector2>());
        horizontal = value.Get<Vector2>().x;
        vertical = value.Get<Vector2>().y;
        RotateCamera();
    }

    private void RotateCamera()
    {
        cameraRotate = cameraFollow.transform.rotation.y + (horizontal * cameraRotationSpeed * Time.deltaTime);
        cameraFollow.transform.Rotate(0f, cameraRotate, 0f);
    }

    private void FixedUpdate(){
        
        
        //move camera up/down when vertical input

    }



}
