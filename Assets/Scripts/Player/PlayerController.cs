using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerState
{

    public PlayerController(PlayerState state) : base(state){

    }

    [Header("Player Movement Components and Objects")]
    [SerializeField] Transform rotatePlayerPoint;
    [SerializeField] Rigidbody playerRb;
    [SerializeField] Transform lookAtCamera;
    [SerializeField] PlayerAnimationController playerAnimationController;
    [SerializeField] GroundChecker groundChecker;
    [SerializeField] FreeClimb freeClimb;
    [SerializeField] ZTargeting zTargeting;

    [Header("Player Movement Variables")]
    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;
    [SerializeField] float groundCheckDist = 2f;
    [SerializeField] LayerMask groundCheckIgnoreLayer;
    [SerializeField] GameObject slopeCheck;
    [SerializeField] float slopeCheckDist;
    [SerializeField] float maximumSlopeAngle;

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
    public Vector3 surfaceAdjustedVector;
    private float surfaceNormalAngle;
    private bool isActiveState = false;
    public bool isJumping = false;

    public override void StartPlayerState()
    {
        base.StartPlayerState();
        isActiveState = true;
        playerAnimationController.HandleRootMotion(false);
    }

    public override void RunPlayerState()
    {
        base.RunPlayerState();
        GetMovementInput();
        isGrounded = GroundCheck();
        //SlopeCheck();
        playerAnimationController.HandlePlayerIsGrounded(isGrounded);
    }

    public override void EndPlayerState()
    {
        isActiveState = false;
        base.EndPlayerState();

    }

    private void FixedUpdate() {
        if(isActiveState){
            MovePlayer();
            RotatePlayer();
        }
    }

    private void HandleCameraDirection()
    {
        //checks the direction to the camera
        lookAtCamera.LookAt(Camera.main.transform.position);
        cameraAngle = lookAtCamera.transform.eulerAngles.y;
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
            } else {
                currentJumpForce = jumpForce;
                shouldJump = true;
            }
        }
 
    }

    private void MovePlayer(){
        if(isGrounded){
            Vector3 slopeVector = SlopeCheck();
            float moveIntensity = Vector2.Distance(Vector2.zero, new Vector2(horizontalInput, verticalInput));
            //velocityVector = new Vector3((playerRb.transform.forward.x + slopeVector.x)  * moveIntensity * moveSpeed, 0f, (playerRb.transform.forward.z + slopeVector.z) * moveIntensity * moveSpeed);
            velocityVector = new Vector3((slopeVector.x)  * moveIntensity * moveSpeed, 0f, (slopeVector.z) * moveIntensity * moveSpeed); 
            playerAnimationController.HandlePlayerSpeed(Mathf.Clamp01(moveIntensity));
            yVelocity = slopeVector.y * moveIntensity * moveSpeed;
            if(shouldJump){
                yVelocity = jumpForce;
                isJumping = true;
                shouldJump = false;
            }
        } else {
            yVelocity = yVelocity - playerGravity;
            if(!isJumping){
                velocityVector = new Vector3(transform.forward.x * 0.1f * moveSpeed, 0f, transform.forward.z * 0.1f * moveSpeed);
                //switch isJumping back to false when the jump has ended
            }
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

    private bool GroundCheck(){
        //CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        RaycastHit hit;
        Ray ray = new Ray(transform.position + new Vector3(0, 0.1f, 0), Vector3.down);
        
        if(Physics.Raycast(ray, out hit, groundCheckDist, ~groundCheckIgnoreLayer)){
            if(isJumping == true && isGrounded == false){
                isJumping = false;
            }
            return true;
        } else {
            return false;
        }
        
    }


    public override void StateCheck()
    {
        base.StateCheck();
        ClimbCheck();
        ZTargetCheck();
    }

    private void ZTargetCheck()
    {
        if(Input.GetKeyDown(KeyCode.U)){
            playerFSM.PushState(zTargeting);
            EndPlayerState();
        }
    }

    private Vector3 SlopeCheck()
    {
        Ray ray = new Ray(slopeCheck.transform.position, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, slopeCheckDist,~groundCheckIgnoreLayer)){
            //Debug.Log("Hitting object at position " + hitInfo.point);
            Vector3 v = hitInfo.point - transform.position;
            Debug.DrawLine(hitInfo.point, transform.position, Color.red, Vector3.Distance(hitInfo.point, transform.position));
            float groundAngle = Vector3.Angle(v, transform.forward);
            //if hit point is higher than the transform, then the angle is upward and we need to check if we can walk/run on this surface or if we need to transition to climb
            if(hitInfo.point.y > transform.position.y){
                //Debug.Log("Going uphill");
                if(groundAngle > maximumSlopeAngle){
                    Debug.Log("Transition to climb");
                }
            }
            

            return v;
        } else {
            return transform.forward;
        }

    }

    public void ClimbCheck(){
        
        //raycast forward to see if there is a surface to climb
        Vector3 rayCastStartPos = new Vector3(transform.position.x, transform.position.y + playerFSM.wallCheckOffset, transform.position.z);
        Ray ray = new Ray(rayCastStartPos, transform.forward);
        Debug.DrawRay(rayCastStartPos, transform.forward, Color.magenta, .1f);
        if(Physics.Raycast(ray, out RaycastHit hit, playerFSM.climbCheckDist, ~playerFSM.climbCheckIgnoreLayer)){
            playerAnimationController.HandlePlayerIsClimbing(true);
            playerFSM.PushState(freeClimb);
            EndPlayerState();
        } else {
            return;
        }
    }

}
