using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeClimb : PlayerState
{

    public FreeClimb(PlayerState state) : base(state){

    }

    //[SerializeField] float wallCheckOffset = 1f;
    [SerializeField] float climbCheckDist = .2f;
    [SerializeField] float climbSpeed = 1f;
    [SerializeField] float climbAdjustAngleSpeed = 1f;
    [SerializeField] Transform climbSurfaceCheckRaycastPosition;
    [SerializeField] float climbSurfaceCheckRaycastUpperOffset = .5f;
    [SerializeField] float surfaceSlopeClimbMultiplier = 1.5f;
    [SerializeField] LayerMask climbSurfaceCheckIgnoreLayer;
    [SerializeField] Transform ledgePointRaycastPosition;
    [SerializeField] float ledgeClimbSpeed = 3f;

    public bool isClimbing = false;

    private float horizontalInput;
    private float verticalInput;
    private bool jumpInput;
    private Vector3 velocityVector;
    private Rigidbody playerRb;
    private GameObject climbSurface;
    private bool isActiveState = false;
    public Vector3 surfaceSlopeVector;
    private float surfaceXRotation;
    public Vector3 lowerSurfaceHitPoint;
    public Vector3 upperSurfaceHitPoint;

    private PlayerAnimationController playerAnimationController;

    public override void StartPlayerState() {
        isActiveState = true;
        playerRb = GetComponent<Rigidbody>();
        playerAnimationController = GetComponentInChildren<PlayerAnimationController>();
        if(!playerRb || playerRb == null){
            Debug.LogWarning("Freeclimb component could not find a Rigidbody!");
        }
        if(!playerAnimationController || playerAnimationController == null){
            Debug.LogWarning("Freeclimb component could not find a PlayerAnimationController component!");
        }
        StartClimb();
    }

    public override void RunPlayerState() {
        base.RunPlayerState();
        GetClimbInput();
        SurfaceCheck();
    }

    public override void EndPlayerState()
    {
        isActiveState = false;
        base.EndPlayerState();
    }

    public override void StateCheck()
    {
        base.StateCheck();
        WalkCheck();
        //TODO Jump and JumpCheck?
    }

    public void StartClimb(){
        playerRb.useGravity = false;
        isClimbing = true;
        //ToggleFreezeRotations(true);
    }

    private void ToggleFreezeRotations(bool t)
    {
        if(t == true){
            playerRb.constraints = RigidbodyConstraints.FreezeRotationY;
            playerRb.constraints = RigidbodyConstraints.FreezePositionZ;
        } else {
            playerRb.constraints = RigidbodyConstraints.None;
        }

    }


    private void WalkCheck()
    {
        //This method does the necessary checks to change state from climb back to walk/run
        //first we check the angle of the slope we are on
    }

    private void SurfaceCheck(){
        Ray lowerRay = new Ray(climbSurfaceCheckRaycastPosition.transform.position, transform.forward);
        Ray upperRay = new Ray(climbSurfaceCheckRaycastPosition.transform.position + new Vector3(0f, climbSurfaceCheckRaycastUpperOffset, 0f), transform.forward);
        Debug.DrawRay(climbSurfaceCheckRaycastPosition.transform.position, transform.forward, Color.green, .1f);
        Debug.DrawRay(climbSurfaceCheckRaycastPosition.transform.position + new Vector3(0f, climbSurfaceCheckRaycastUpperOffset, 0f), transform.forward, Color.green, .1f);
        if(Physics.Raycast(lowerRay, out RaycastHit hitInfo, climbCheckDist + 1f, ~climbSurfaceCheckIgnoreLayer)){
            surfaceXRotation = Vector3.Angle(Vector3.up, hitInfo.collider.transform.up) - 90f;
            //Debug.Log("surfaceSlopeVector is " + surfaceSlopeVector);
            lowerSurfaceHitPoint = hitInfo.point;
        } else {
            surfaceXRotation = 0f;
            lowerSurfaceHitPoint = Vector3.zero;
        }

        if(Physics.Raycast(upperRay, out RaycastHit upperHitInfo, climbCheckDist + 1f, ~climbSurfaceCheckIgnoreLayer)){
            upperSurfaceHitPoint = upperHitInfo.point;
        } else {
            upperSurfaceHitPoint = Vector3.zero;
            //we need to transition to climbing up since the higher check fails
            TransitionToWalk();
        }

        surfaceSlopeVector = upperSurfaceHitPoint - lowerSurfaceHitPoint;
/*
                if(horizontalInput != 0f || verticalInput != 0f){
            HandleCameraDirection();
            atanAngle = cameraAngle - 180 + Mathf.Atan2(horizontalInput, verticalInput) * Mathf.Rad2Deg;
            playerRb.MoveRotation(Quaternion.Slerp(rotatePlayerPoint.rotation, Quaternion.Euler(0f, atanAngle, 0f), turnSpeed * Time.fixedDeltaTime));
        }
        */
    }

    private void TransitionToWalk()
    {
        //TODO add a check where you see if the player is climbing up or dropping down -> animation transitions are different
        ClimbLedge(); //only trigger this if you are climbing
        playerAnimationController.HandlePlayerIsClimbingLedge(true);
        //DropFromWall();
        EndPlayerState();
        playerFSM.PushState(defaultState);
    }

    private void ClimbLedge()
    {
        //Move this to its own PlayerState?

        //handle movement to top of ledge
        //get point to which we move the RB
        Vector3 ledgePoint;
        Ray ledgeRay = new Ray(ledgePointRaycastPosition.transform.position, Vector3.down);
        Debug.DrawRay(ledgePointRaycastPosition.transform.position, Vector3.down, Color.green, 15f);
        Debug.Log("ClimbLedge called!");
        if(Physics.Raycast(ledgeRay, out RaycastHit ledgeHitInfo, climbCheckDist, ~climbSurfaceCheckIgnoreLayer)){
            ledgePoint = ledgeHitInfo.point;
            Debug.Log("ClimbLedge point is " + ledgePoint);
            //translate the RB position to ledgepoint
            playerAnimationController.HandlePlayerIsClimbing(false);
            playerAnimationController.HandlePlayerIsClimbingLedge(true);
        }
    }

    private void FixedUpdate(){
        if(isActiveState){
            Climb();
        }
    }

    private void Climb(){
        float moveIntensity = Vector2.Distance(Vector2.zero, new Vector2(horizontalInput, verticalInput)); //get if player wants to move slow or fast
        velocityVector = new Vector3(playerRb.transform.right.x * horizontalInput * climbSpeed, playerRb.transform.up.y * verticalInput * climbSpeed, surfaceSlopeVector.z * verticalInput * climbSpeed * surfaceSlopeClimbMultiplier);
        playerRb.velocity = velocityVector;
        playerRb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.Euler(surfaceXRotation, 0f, 0f), climbAdjustAngleSpeed * Time.fixedDeltaTime));
    }

    private void GetClimbInput(){
        horizontalInput = Input.GetAxis("Horizontal"); //left = -1, right = 1
        verticalInput = Input.GetAxis("Vertical"); //up and down
        jumpInput = Input.GetButton("Jump");
        //HandleJump(); //TODO create function to jump off wall
    }

}
