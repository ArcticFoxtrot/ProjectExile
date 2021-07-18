using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionToWalk : PlayerState
{
    public TransitionToWalk(PlayerState state) : base(state)
    {

    }

    public bool isRotated = false;
    [SerializeField] float rotateUpSpeed = 1f;

    public override void StartPlayerState()
    {
        base.StartPlayerState();
        Debug.Log("Starting to rotate back!");
        isRotated = false;
    }

    public override void RunPlayerState()
    {
        base.RunPlayerState();
        CheckRotation();
        if(!isRotated){
            RotatePlayer();
        } else {
            playerFSM.PushState(defaultState);
            EndPlayerState();
        }
        
       
    }

    private void CheckRotation()
    {
        //checks if the player's transform is pointing upward
        Debug.Log("Angle between up and transform up is " + Vector3.Angle(transform.up, Vector3.up));
        if(Vector3.Angle(transform.up, Vector3.up) < .01f){
            isRotated = true;
        }
    }

    private void RotatePlayer()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation, rotateUpSpeed * Time.deltaTime);
        // Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(transform.forward * -1, upperHitInfo.normal) * transform.rotation, climbAdjustAngleSpeed);
    }
}
