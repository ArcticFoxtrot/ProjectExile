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
    [SerializeField] PlayerAnimationController playerAnimationController;
    [SerializeField] GroundChecker groundChecker;

    [Header("Player Movement Variables")]
    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;
    [SerializeField] float groundCheckDist = 2f;
    [SerializeField] LayerMask groundCheckIgnoreLayer;

    [Header("Player Jump Variables")]
    [Tooltip("Time in seconds for pressing jump button to achieve more jump force")]
    [SerializeField] float highJumpThreshold = .5f;
    [SerializeField] float highJumpModifier = 1f;
    [SerializeField] float jumpForce = 10f;
 
    [Header("Player Fall variables")]
    [SerializeField] float playerGravity;
    

    private Vector2 movementDirection;
    private Vector3 lookDirection;
    private float cameraAngle;
    private Vector2 cameraForward;
    private Vector2 cameraRight;

    private float horizontalInput;
    private float verticalInput;
    private bool jumpInput;
    public bool shouldJump = false;
    private float jumpPressTime;
    private float jumpReleaseTime;
    private float currentJumpForce;
    private float atanAngle;
    public bool isGrounded;
    public bool highPoint = false;
    public float yVelocity;
    public Vector3 velocityVector;
    


    private void Update() {
        GetMovementInput();
        //ApplyGravity(); //also handles jumping
        isGrounded = GroundCheck();
        //isGrounded = groundChecker.GetIsGrounded();
        }

    private void FixedUpdate() {
        MovePlayer();
        RotatePlayer();
    }

    private void HandleCameraDirection()
    {
        //checks the direction to the camera
        lookAtCamera.LookAt(Camera.main.transform.position);
        cameraAngle = lookAtCamera.transform.eulerAngles.y;
        //Debug.Log("Camera angle is "+ cameraAngle);
    }


    private void GetMovementInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        jumpInput = Input.GetButton("Jump");
        HandleJump();
        
    }

    private void HandleJump()
    {
        if(!isGrounded){return;}
        if(Input.GetButtonDown("Jump")){
            jumpPressTime = Time.time;
        }
        if(Input.GetButtonUp("Jump")){
            jumpReleaseTime = Time.time;
            if(jumpReleaseTime - jumpPressTime > highJumpThreshold){
                currentJumpForce = jumpForce * highJumpModifier;
                shouldJump = true;
                Debug.Log("Jump high");
            } else {
                currentJumpForce = jumpForce;
                shouldJump = true;
                Debug.Log("Jump low");
            }
        }
 
    }

    private void MovePlayer(){
        if(isGrounded){
            float moveIntensity = Vector2.Distance(Vector2.zero, new Vector2(horizontalInput, verticalInput));
            velocityVector = new Vector3(playerRb.transform.forward.x  * moveIntensity * moveSpeed, 0f, playerRb.transform.forward.z * moveIntensity * moveSpeed); 
            playerAnimationController.HandlePlayerSpeed(Mathf.Clamp01(moveIntensity));
            yVelocity = 0f;
            if(shouldJump){
                yVelocity = jumpForce;
                shouldJump = false;
            }
        } else {
            yVelocity = yVelocity - playerGravity;
        }

        velocityVector.y = yVelocity;
        playerRb.velocity = velocityVector;
    }

    private void RotatePlayer(){
        
        if(horizontalInput != 0f || verticalInput != 0f){
            HandleCameraDirection();
            atanAngle = cameraAngle - 180 + Mathf.Atan2(horizontalInput, verticalInput) * Mathf.Rad2Deg;
            playerRb.MoveRotation(Quaternion.Slerp(rotatePlayerPoint.rotation, Quaternion.Euler(0f, atanAngle, 0f), turnSpeed * Time.fixedDeltaTime));
        }
    }

    private void Jump(){
        playerRb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
        shouldJump = false;
    }

    private bool GroundCheck(){
        Ray ray = new Ray(transform.position + new Vector3(0, 0.1f, 0), Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, groundCheckDist, ~groundCheckIgnoreLayer)){
            return true;
        } else {
            return false;
        }
    }

/*
    private void ApplyGravity(){
        if(playerRb.velocity.y < 0 && highPoint == false){
            Debug.Log("Got to falling!");
            highPoint = true;
            
        }

        if(isGrounded){
            highPoint = false;
        }

        if(highPoint == true){
            currentGravity = Mathf.Lerp()
        }
            
        if(shouldJump){
            Debug.Log("Got to jumping!");
            highPoint = false;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            shouldJump = false;
        }
    }

*/
}
