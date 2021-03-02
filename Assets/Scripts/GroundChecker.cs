using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{

    private bool isGrounded = false;

    private void OnTriggerEnter(Collider other) {
        if(isGrounded != true){
            isGrounded = true;
        }
        
    }

    private void OnTriggerExit(Collider other) {
        isGrounded = false;
    }

    public bool GetIsGrounded(){
        return isGrounded;
    }


}
