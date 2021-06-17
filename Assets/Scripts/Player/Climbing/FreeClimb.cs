using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeClimb : PlayerState
{

    public FreeClimb(PlayerState state) : base(state){

    }

    //[SerializeField] float wallCheckOffset = 1f;
    [Header("Player Climb Variables")]
    [SerializeField] float climbCheckDist = .2f;
    [SerializeField] float climbSpeed = 1f;
    [SerializeField] float climbAdjustAngleSpeed = 1f;

    [Header("Surface Adaptation Variables")]
    [SerializeField] Transform climbSurfaceCheckRaycastPosition;
    [SerializeField] float climbSurfaceCheckRaycastUpperOffset = .5f;
    [SerializeField] float surfaceSlopeClimbMultiplier = 1.5f;
    [SerializeField] LayerMask climbSurfaceCheckIgnoreLayer;
    [SerializeField] Transform ledgePointRaycastPosition;
    [SerializeField] float ledgeClimbSpeed = 3f;

    [Header("Corner Checking Variables")]
    [SerializeField] Transform leftCornerRaycastPosition;
    [SerializeField] Transform rightCornerRaycastPosition;
    [SerializeField] float cornerCheckAngle = .5f;
    [SerializeField] float cornerCheckDist = 1f;
    [SerializeField] float cornerAdjustAngleSpeed = 0.5f;

    public bool isClimbing = false;

    public float horizontalInput;
    private float verticalInput;
    private bool jumpInput;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    private Vector3 velocityVector;
    private Rigidbody playerRb;
    private GameObject climbSurface;
    private bool isActiveState = false;
    public Vector3 surfaceSlopeVector;
    public float surfaceXRotation;
    public float surfaceYRotation;
    public Vector3 lowerSurfaceHitPoint;
    public Vector3 upperSurfaceHitPoint;
    public Vector3 leftRightSurfaceVector;
    public Vector3 climbVector;
    public bool rotate = false;
    public float angle;


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
        //TODO Jump and JumpCheck?
    }

    public void StartClimb(){
        playerRb.useGravity = false;
        isClimbing = true;
        playerRb.velocity = Vector3.zero;
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

    private void SurfaceCheck(){
        Ray lowerRay = new Ray(climbSurfaceCheckRaycastPosition.transform.position, transform.forward);
        Ray upperRay = new Ray(climbSurfaceCheckRaycastPosition.transform.position + new Vector3(0f, climbSurfaceCheckRaycastUpperOffset, 0f), transform.forward);
        Debug.DrawRay(climbSurfaceCheckRaycastPosition.transform.position, transform.forward, Color.green, .1f);
        Debug.DrawRay(climbSurfaceCheckRaycastPosition.transform.position + new Vector3(0f, climbSurfaceCheckRaycastUpperOffset, 0f), transform.forward, Color.green, .1f);
        if(Physics.Raycast(lowerRay, out RaycastHit hitInfo, climbCheckDist + 1f, ~climbSurfaceCheckIgnoreLayer)){
            surfaceXRotation = Vector3.Angle(transform.up, hitInfo.collider.transform.up);
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
            //TransitionToWalk();
        }

        surfaceSlopeVector = upperSurfaceHitPoint - lowerSurfaceHitPoint;

        //check for the angle to which player is climbing horizontally -> turn corners
        Debug.DrawRay(leftCornerRaycastPosition.transform.position, transform.forward + (transform.right * cornerCheckAngle), Color.magenta, .2f);
        Debug.DrawRay(rightCornerRaycastPosition.transform.position, transform.forward + (transform.right * -cornerCheckAngle), Color.magenta, .2f);

        Ray leftCornerRay = new Ray(leftCornerRaycastPosition.transform.position, transform.forward + (transform.right * cornerCheckAngle));
        Ray rightCornerRay = new Ray(rightCornerRaycastPosition.transform.position, transform.forward + (transform.right * -cornerCheckAngle));

        if(isMovingLeft){
            if(Physics.Raycast(leftCornerRay, out RaycastHit leftCornerHitInfo, cornerCheckDist, ~climbSurfaceCheckIgnoreLayer)){
                //Debug.Log("Angle between normals is " + Vector3.SignedAngle(upperHitInfo.normal, leftCornerHitInfo.normal, Vector3.up));¨
                //when going left the y for cross is negative if obtuse angle
                Debug.Log("Cross between normals is " + Vector3.Cross(leftCornerHitInfo.normal, upperHitInfo.normal));
                Debug.Log("Going left!");
                leftRightSurfaceVector = leftCornerHitInfo.point - new Vector3(upperHitInfo.point.x, leftCornerHitInfo.point.y, upperHitInfo.point.z);
                Debug.DrawLine(new Vector3(upperHitInfo.point.x, leftCornerHitInfo.point.y, upperHitInfo.point.z), leftCornerHitInfo.point, Color.white, .2f );
                if(Vector3.Cross(leftCornerHitInfo.normal, upperHitInfo.normal).y <= 0f){
                    angle = -1 * (360f + Vector3.Angle(-1 * transform.right, leftRightSurfaceVector.normalized));
                } else {
                    angle = Vector3.Angle(-1 * transform.right, leftRightSurfaceVector.normalized);
                }
                surfaceYRotation = transform.rotation.eulerAngles.y - angle; 
                //get the angle of the hit point
                if(leftCornerHitInfo.normal != upperHitInfo.normal){
                    Debug.Log("Initiate left corner turn");
                //surfaceYRotation = Vector3.Angle(-transform.right, leftCornerHitInfo.normal);


                }
            }
        } else if(isMovingRight){
            if(Physics.Raycast(rightCornerRay, out RaycastHit rightCornerHitInfo, cornerCheckDist, ~climbSurfaceCheckIgnoreLayer)){
                //Debug.Log("Angle between normals is " + Vector3.SignedAngle(upperHitInfo.normal, rightCornerHitInfo.normal, Vector3.up));
                //when going right the y for cross is negative for acute angle
                Debug.Log("Cross between normals is " + Vector3.Cross(rightCornerHitInfo.normal, upperHitInfo.normal));
                Debug.Log("Going right!");
                leftRightSurfaceVector = rightCornerHitInfo.point - new Vector3(upperHitInfo.point.x, rightCornerHitInfo.point.y, upperHitInfo.point.z);
                Debug.DrawLine(new Vector3(upperHitInfo.point.x, rightCornerHitInfo.point.y, upperHitInfo.point.z), rightCornerHitInfo.point, Color.white, .2f );
                if(Vector3.Cross(rightCornerHitInfo.normal, upperHitInfo.normal).y >= 0f){
                    angle = -1 * Vector3.Angle(transform.right, leftRightSurfaceVector.normalized);
                    surfaceYRotation = transform.rotation.eulerAngles.y + angle;
                } else {
                    angle = Vector3.Angle(-1 * transform.right, leftRightSurfaceVector.normalized);
                    surfaceYRotation = transform.rotation.eulerAngles.y + angle;
                }
                angle = Vector3.Angle(transform.right, leftRightSurfaceVector.normalized);
                //surfaceYRotation = transform.rotation.eulerAngles.y - angle;
                if(rightCornerHitInfo.normal != upperHitInfo.normal){
                    Debug.Log("Initiate right corner turn");
                }
            }
        } else {
            leftRightSurfaceVector = Vector3.zero;
        }
    }

    private void TransitionToWalk()
    {
        //TODO add a check where you see if the player is climbing up or dropping down -> animation transitions are different
         //only trigger this if you are climbing
        Debug.Log("Transition to walk");
        playerAnimationController.HandlePlayerIsClimbingLedge(true);
        //DropFromWall();
        playerRb.velocity = Vector3.zero;
        playerFSM.PushState(playerFSM.climbLedgeState);
        EndPlayerState();
        
    }

    private void FixedUpdate(){
        if(isActiveState){
            Climb();
        }
    }

    private void Climb(){
        //velocityVector = new Vector3(leftRightSurfaceVector.x * Mathf.Abs(horizontalInput) * climbSpeed, Vector3.up.y * verticalInput * climbSpeed, (leftRightSurfaceVector.z + surfaceSlopeVector.z) * verticalInput * climbSpeed * surfaceSlopeClimbMultiplier);
        //playerRb.velocity = velocityVector;
        climbVector = new Vector3(Mathf.Abs(leftRightSurfaceVector.x) * horizontalInput, Vector3.up.y * verticalInput, leftRightSurfaceVector.z * Mathf.Abs(horizontalInput)) * Time.fixedDeltaTime * climbSpeed;
        Debug.DrawRay(transform.position, climbVector.normalized, Color.blue, .2f);
        playerRb.MovePosition(transform.position + climbVector);
        playerRb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.Euler(surfaceXRotation, surfaceYRotation, 0f), climbAdjustAngleSpeed * Time.fixedDeltaTime));

    }

    private void GetClimbInput(){
        horizontalInput = Input.GetAxis("Horizontal"); //left = -1, right = 1
        if(horizontalInput < 0){
            isMovingLeft = true;
        } else {
            isMovingLeft = false;
        }

        if(horizontalInput > 0){
            isMovingRight = true;
        } else {
            isMovingRight = false;
        }
        verticalInput = Input.GetAxis("Vertical"); //up and down
        jumpInput = Input.GetButton("Jump");
        //HandleJump(); //TODO create function to jump off wall
    }

}
