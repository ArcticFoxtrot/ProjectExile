using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Player Movement Components and Objects")]
    [SerializeField] Transform rotatePlayerPoint;
    [SerializeField] Rigidbody playerRb;
    [SerializeField] Transform lookAtCamera;

    [Header("Player Movement Variables")]
    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;
    

    private Vector2 movementDirection;
    private Vector3 lookDirection;
    public float cameraAngle;
    private Vector2 cameraForward;
    private Vector2 cameraRight;

    private float horizontalInput;
    private float verticalInput;
    private float clampedHorizontal = 0f;
    private float clampedVertical = 0f;
    public float atanAngle;
    
    private void HandleCameraDirection()
    {
        //checks the direction to the camera
        lookAtCamera.LookAt(Camera.main.transform.position);
        cameraAngle = lookAtCamera.transform.eulerAngles.y;
        //Debug.Log("Camera angle is "+ cameraAngle);
    }

    private void FixedUpdate() {
        GetMovementInput();
        MovePlayer();
        RotatePlayer();
    }

    private void GetMovementInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        clampedHorizontal = Mathf.Clamp(horizontalInput, -0.7f, 0.7f);
        clampedVertical = Mathf.Clamp(verticalInput, -0.7f, 0.7f);

    }

    private void MovePlayer(){
        //move whatever distance the input is from 0,0 max .7 -> always move to the local forward
        //Vector3 movementDirection = transform.forward * verticalInput * moveSpeed * Time.deltaTime + transform.right * horizontalInput * moveSpeed * Time.deltaTime;
        //transform.position += movementDirection;
        //get distance from zero to the vector from input
        float moveIntensity = Vector2.Distance(Vector2.zero, new Vector2(horizontalInput, verticalInput));
        //add velocity to forward direction based on intensity
        playerRb.velocity = playerRb.transform.forward * moveIntensity * moveSpeed * Time.deltaTime;
    }

    private void RotatePlayer(){
        
        if(horizontalInput != 0f && verticalInput != 0f){
            HandleCameraDirection();
            atanAngle = cameraAngle - 180 + Mathf.Atan2(horizontalInput, verticalInput) * Mathf.Rad2Deg;
            playerRb.MoveRotation(Quaternion.Slerp(rotatePlayerPoint.rotation, Quaternion.Euler(0f, atanAngle, 0f), turnSpeed * Time.deltaTime));
        }
    }
}
