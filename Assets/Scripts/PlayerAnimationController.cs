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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandlePlayerSpeed(float normalizedVelocity){
        animator.SetFloat("Speed", normalizedVelocity); //sets animator speed float to normalized speed value between 0 to 1
    }
}
