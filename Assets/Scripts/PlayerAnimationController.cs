using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{

    private Animator animator;
    private bool isInCombat = false;
    private int combatInputCount;
    private PlayerCombat playerCombat;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        if(!animator || animator == null){
            Debug.Log("Animator not found in PlayerAnimationController");
        }        
        combatInputCount = -1;

        playerCombat = GetComponentInParent<PlayerCombat>();
        if(!playerCombat || playerCombat == null){
            Debug.Log("player animator controller could not find a PlayerCombat component in its parent gameobject");
        }

    }

    public void HandlePlayerSpeed(float normalizedVelocity){
        animator.SetFloat("Speed", normalizedVelocity); //sets animator speed float to normalized speed value between 0 to 1
    }

    public void HandlePlayerIsGrounded(bool isGrounded){
        animator.SetBool("IsGrounded", isGrounded);
    }

    public void HandlePlayerIsClimbing(bool isClimbing)
    {
        animator.SetBool("IsClimbing", isClimbing);
    }

    public void HandlePlayerIsClimbingLedge(bool isClimbingLedge){
        animator.SetBool("IsClimbingLedge", isClimbingLedge);
    }

    public void HandlePlayerClimbBlend(float vertical, float horizontal){
        animator.SetFloat("VerticalInput", vertical);
        animator.SetFloat("HorizontalInput", horizontal);
    }

    private void OnAnimatorMove() {
        transform.parent.position = animator.rootPosition;
    }

    public void HandleRootMotion(bool t){
        animator.applyRootMotion = t;
    }

    public void HandlePlayerIsZTargeting(bool t)
    {
        animator.SetBool("IsZTargeting", t);
    }

    public void HandlePlayerZTargetingBlend(float vertical, float horizontal){
        animator.SetFloat("VerticalInput", vertical);
        animator.SetFloat("HorizontalInput", horizontal);
    }

    public void HandlePlayerIsAttacking(bool t, CombatInputType combatInputType, int comboCount){
        animator.SetTrigger("IsRightHandStriking");
        animator.SetBool("IsInCombat", t);
        isInCombat = true;
    }

    public void HandleCombatAnimations(){
        //increase the input count for each time combat input is given
        animator.SetBool("IsInCombat", true);
        combatInputCount ++;
        animator.SetInteger("CombatInputCount", combatInputCount);
        isInCombat = true;
    }

    public void HandleCombatAnimationFinished(){
        if(isInCombat){
            combatInputCount = -1;
            Debug.Log("Resetting combat animation count");
            animator.SetInteger("CombatInputCount", combatInputCount);
            animator.SetBool("IsInCombat", false);
            isInCombat = false;
            //move out of combat state
            playerCombat.HandleEndState();
        }

    }

    internal void HandleIsClimbing(bool isClimbing)
    {
        animator.SetBool("IsClimbing", isClimbing);
    }
}
