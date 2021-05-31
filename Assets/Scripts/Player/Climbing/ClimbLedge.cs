using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbLedge : PlayerState
{
    public ClimbLedge(PlayerState state) : base(state){

    }

    [SerializeField] Transform ledgePointRaycastPosition;
    [SerializeField] float ledgeClimbCheckDist = .5f;
    [SerializeField] float climbLedgeSpeed = 1f;
    [SerializeField] float ledgePointAllowedDistance = .1f;

    private bool isActiveState = false;
    private Rigidbody playerRb;
    private PlayerAnimationController playerAnimationController;
    private bool climbPointChecked = false;
    private Vector3 ledgePoint;


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
        
    }

    public override void RunPlayerState() {
        base.RunPlayerState();
        ClimbOverLedge();
        //GetClimbInput();
        //SurfaceCheck();
    }

    public override void EndPlayerState()
    {
        isActiveState = false;
        climbPointChecked = false;
        base.EndPlayerState();
    }

    private void ClimbOverLedge()
    {
        //Move this to its own PlayerState?

        //handle movement to top of ledge
        //get point to which we move the RB
       
        Debug.DrawRay(ledgePointRaycastPosition.transform.position, Vector3.down, Color.green, 15f);
        if(climbPointChecked == false){
            Ray ledgeRay = new Ray(ledgePointRaycastPosition.transform.position, Vector3.down);
            if(Physics.Raycast(ledgeRay, out RaycastHit ledgeHitInfo, ledgeClimbCheckDist, ~playerFSM.climbCheckIgnoreLayer)){
                climbPointChecked = true;
                ledgePoint = ledgeHitInfo.point;
                //translate the RB position to ledgepoint
               
                //playerRb.transform.position = ledgePoint;
                
                //rotate the player back to vertical 
                playerAnimationController.HandlePlayerIsClimbing(false);
                playerAnimationController.HandlePlayerIsClimbingLedge(true);

            }
        }

        if(climbPointChecked && Vector3.Distance(this.transform.position, ledgePoint) >= ledgePointAllowedDistance){
            Vector3 targetVector = ledgePoint - this.transform.position;
            gameObject.GetComponent<Collider>().isTrigger = true;
            playerRb.MovePosition(playerRb.transform.position + targetVector * Time.fixedDeltaTime * climbLedgeSpeed);
        }

        if(climbPointChecked && Vector3.Distance(this.transform.position, ledgePoint) <= ledgePointAllowedDistance){
            playerAnimationController.HandlePlayerIsClimbingLedge(false);
            gameObject.GetComponent<Collider>().isTrigger = false;
            playerRb.isKinematic = false;
            EndPlayerState();
        }
        //once ledge has been climbed, make sure no velocity is applied anymore and we are back to using gravity
    }


}