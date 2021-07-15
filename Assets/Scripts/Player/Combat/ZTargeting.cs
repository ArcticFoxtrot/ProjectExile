using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZTargeting : PlayerState
{


    public Enemy targetedEnemy;
    private Vector3 lookAtPos;
    private bool isTargeting = false;
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject zTargetPosition;
    [SerializeField] GameObject slopeCheck;
    [SerializeField] float slopeCheckDist;
    [SerializeField] LayerMask groundCheckIgnoreLayer;
    [SerializeField] PlayerAnimationController playerAnimationController;
    private float horizontalInput;
    private float verticalInput;
    private bool jumpInput;
    private Vector3 verticalVector;
    private Vector3 velocityVector;
    private float yVelocity;
    public Vector3 dirToTarget;
    [SerializeField] Rigidbody playerRb;
    [SerializeField] float moveSpeed;

    public ZTargeting(PlayerState state, Enemy enemy) : base(state)
    {
        if(enemy){
            targetedEnemy = enemy;
        }
    }

    public static ZTargeting GetZTargeting(){
        return GameObject.FindGameObjectWithTag("Player").GetComponent<ZTargeting>();
    }


    public override void StartPlayerState()
    {
        base.StartPlayerState();
        if(!targetedEnemy || targetedEnemy == null){
            targetedEnemy = FindTargetedEnemy();
            Debug.Log("Closest enemy is " + targetedEnemy.name);
        }
        cameraController.SetLookAtTarget(zTargetPosition.transform);
        cameraController.SetFollowTarget(zTargetPosition.transform);
        cameraController.SetIsZTargeting(true);
        isTargeting = true;
        playerAnimationController.HandlePlayerIsZTargeting(isTargeting);
        
    }

    public override void RunPlayerState()
    {
        base.RunPlayerState();
        lookAtPos = GetLookAtPosition();
        zTargetPosition.transform.position = lookAtPos;
        //make character face the targeted enemy
    }

    private void FixedUpdate() {
        if(isTargeting){
            GetMovementInput();
            MovePlayer();
            RotateToTarget();
        }
    }

    private Vector3 GetLookAtPosition()
    {
        //get vector between player and targeted enemy
        Vector3 toTarget = cameraController.GetCameraFollowTarget().transform.position + targetedEnemy.transform.position;
        //get halfway point between player and target
        Vector3 halfway = toTarget / 2f;
        return halfway;
    }

    private Enemy FindTargetedEnemy(){
        //TODO add ability to swap target by pressing left/right
        //Finds the gameobject which is closest to the center of the screen
        Enemy[] enemyArray = FindObjectsOfType<Enemy>();
        //List<Enemy> enemies = new List<Enemy>(enemyArray);
        Enemy closestEnemy = null;
        float dot = -2f;
        foreach (Enemy e in enemyArray)
        {
            Vector3 localPos = Camera.main.transform.InverseTransformPoint(e.transform.position).normalized;
            float comp = Vector3.Dot(localPos, Camera.main.transform.forward);
            if(comp > dot){
                dot = comp;
                closestEnemy = e;
            }
        }

        return closestEnemy;
    }

    private void GetMovementInput(){
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        jumpInput = Input.GetButton("Jump");
    }

    private void RotateToTarget(){
        transform.LookAt(targetedEnemy.transform);
    }


    private void MovePlayer(){
        //with vertical input move player toward or away from targeted player
        Vector3 slopeVector = SlopeCheck();
        float moveIntensity = Vector2.Distance(Vector2.zero, new Vector2(horizontalInput, verticalInput));
        dirToTarget = targetedEnemy.transform.position - transform.position;
        dirToTarget.y = 0;
        dirToTarget.Normalize();
        float forward = dirToTarget.z * moveSpeed * verticalInput;

        //with horizontal input move player left or right in relation to the forward vector
        Vector3 leftFromTarget = Vector3.Cross(dirToTarget, Vector3.up).normalized;

        Vector3 forwardVector = (transform.forward + dirToTarget) * moveSpeed * verticalInput * Time.fixedDeltaTime;
        Vector3 leftVector = (transform.right + leftFromTarget * -1) * moveSpeed * horizontalInput * Time.fixedDeltaTime;
        playerRb.velocity = forwardVector + leftVector;
        
        Debug.DrawRay(transform.position, dirToTarget, Color.green, .2f);
        Debug.DrawRay(transform.position, dirToTarget * -1, Color.red, .2f);
        Debug.DrawRay(transform.position, leftFromTarget, Color.blue, .2f);
        Debug.DrawRay(transform.position, leftFromTarget * -1, Color.yellow, .2f);
        
        verticalVector = dirToTarget + slopeVector;
        playerAnimationController.HandlePlayerZTargetingBlend(verticalInput, horizontalInput);
        
    }

    private Vector3 SlopeCheck()
    {
        Ray ray = new Ray(slopeCheck.transform.position, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, slopeCheckDist,~groundCheckIgnoreLayer)){
            Vector3 v = hitInfo.point - transform.position;
            Debug.DrawLine(hitInfo.point, transform.position, Color.red, Vector3.Distance(hitInfo.point, transform.position));
            return v;
        } else {
            return transform.forward;
        }

    }

    public GameObject GetZTarget(){
        return targetedEnemy.gameObject;
    }


    public override void StateCheck()
    {
        base.StateCheck();
        if(Input.GetKeyDown(KeyCode.U) || zTargetPosition == null || !zTargetPosition ){
            if(isTargeting){
                playerFSM.PushState(defaultState);
                cameraController.SetLookAtTarget(cameraController.GetCameraFollowTarget());
                cameraController.SetFollowTarget(cameraController.GetCameraFollowTarget());
                EndPlayerState();
                cameraController.SetIsZTargeting(false);
                playerAnimationController.HandlePlayerIsZTargeting(false);
            }

        }

        //get combat input to move to combat state
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            playerFSM.PushState(playerFSM.combatState);
            playerFSM.combatState.SetStartCombatInput(CombatInputType.Block);
            playerFSM.combatState.SetStartingState(playerFSM.zTargetState);
            EndPlayerState();
        }
    }


    public override void EndPlayerState()
    {
        isTargeting = false;
        base.EndPlayerState();
    }

}
