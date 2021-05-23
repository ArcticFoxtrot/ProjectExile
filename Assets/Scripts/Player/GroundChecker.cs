using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{

    private bool isGrounded = false;

    private void OnTriggerStay(Collider other) {
        if(other.tag != "Player"){
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
