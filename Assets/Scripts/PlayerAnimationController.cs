using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if(!animator || animator == null){
            Debug.Log("Animator not found in PlayerAnimationController");
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
}
