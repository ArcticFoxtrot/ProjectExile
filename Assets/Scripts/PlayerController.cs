using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Player Movement Components and Objects")]


    [Header("Player Movement Variables")]

    private Vector2 movementDirection;
    private Vector3 lookDirection;

    private Rigidbody rb;
    private Camera mainCamera;
    
    private void OnMovement(InputValue value){
        movementDirection = value.Get<Vector2>();
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        if(!rb || rb == null){
            Debug.LogWarning("PlayerController cannot find Rigidbody");
        }

        mainCamera = Camera.main;


    }


    private void FixedUpdate() {
        MovePlayer();
    }

    private void MovePlayer(){
        //move forward from camera direction

    }
}
