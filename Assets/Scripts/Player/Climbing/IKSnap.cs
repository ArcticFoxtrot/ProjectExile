using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKSnap : MonoBehaviour
{
    [SerializeField] Transform playerObject;
    [SerializeField] LayerMask ignorePlayerLayer;
    [SerializeField] Transform handIKRaycastPosition;
    [SerializeField] Transform leftHandIKRaycastPosition;
    [SerializeField] Transform rightHandIKRaycastPosition;
    [SerializeField] float handIKRaycastDist = 0.5f;
    [SerializeField] float handIKRaycastAngle = 0.5f;
    [SerializeField] float handIKRaycastAngleBetween = .5f;
    [SerializeField] float handIKWeight = 1f;
    [SerializeField] Vector3 leftHandOffset;
    [SerializeField] Vector3 rightHandOffset;

    [SerializeField] Transform leftFootIKRaycastPosition;
    [SerializeField] Transform rightFootIKRaycastPosition;
    [SerializeField] float footIKRaycastDist = 0.5f;
    [SerializeField] Vector3 leftFootOffset;
    [SerializeField] Vector3 rightFootOffset;
    public Quaternion leftFootRotOffset;
    public Quaternion rightFootRotOffset;
    public float playerYRotation;
    public float raycastposRotY;
    

    

    public bool useIK;

    public bool leftHandIK;
    public bool rightHandIK;
    public bool leftFootIK;
    public bool rightFootIK;

    public Vector3 leftHandPos;
    public Vector3 rightHandPos;

    public Vector3 leftFootPos;
    public Vector3 rightFootPos;

    public Quaternion leftHandRot;
    public Quaternion rightHandRot;

    public Quaternion leftFootRot;
    public Quaternion rightFootRot;

    private Vector3 leftHandOriginalPos;
    private Vector3 rightHandOriginalPos;



    [SerializeField] Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        if(!animator){
            Debug.Log("IK Snap could not find Animator component");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleTransformRotations();
        RaycastHit leftHandHit;
        RaycastHit rightHandHit;
        RaycastHit rightFootHit;
        RaycastHit leftFootHit;
        //Debug.DrawRay(handIKRaycastPosition.transform.position, -handIKRaycastPosition.transform.up + playerObject.transform.forward + (playerObject.transform.right * handIKRaycastAngleBetween), Color.green, handIKRaycastDist);
        //Debug.DrawRay(handIKRaycastPosition.transform.position, -handIKRaycastPosition.transform.up + playerObject.transform.forward - (playerObject.transform.right * handIKRaycastAngleBetween), Color.green, handIKRaycastDist);
        Debug.DrawRay(leftHandIKRaycastPosition.position, playerObject.transform.forward, Color.green, handIKRaycastDist);
        Debug.DrawRay(rightHandIKRaycastPosition.position, playerObject.transform.forward, Color.green, handIKRaycastDist);
        Debug.DrawRay(leftFootIKRaycastPosition.transform.position, transform.forward, Color.cyan, footIKRaycastDist);
        Debug.DrawRay(rightFootIKRaycastPosition.transform.position, transform.forward, Color.cyan, footIKRaycastDist);

        //left hand IK check
        /*
        if(Physics.Raycast(handIKRaycastPosition.transform.position, -handIKRaycastPosition.transform.up + playerObject.transform.forward - (playerObject.transform.right * handIKRaycastAngleBetween), out leftHandHit, handIKRaycastDist, ~ignorePlayerLayer)){
            leftHandIK = true;
            leftHandPos = leftHandHit.point - leftHandOffset;
            leftHandPos.x = leftHandOriginalPos.x;
            leftHandPos.z = leftFootPos.z - leftHandOffset.z;
            leftHandRot = Quaternion.FromToRotation(Vector3.up, leftHandHit.normal);
        } else {
            leftHandIK = false;
        }
        */

        if(Physics.Raycast(leftHandIKRaycastPosition.position, playerObject.transform.forward, out leftHandHit, handIKRaycastDist, ~ignorePlayerLayer)){
            leftHandIK = true;
            leftHandPos = leftHandHit.point - leftHandOffset;
            leftHandPos.x = leftHandOriginalPos.x;
            //leftHandPos.z = leftFootPos.z - leftHandOffset.z;
            leftHandRot = Quaternion.FromToRotation(Vector3.up, leftHandHit.normal);
        } else {
            leftHandIK = false;
        }

        /*
        if(Physics.Raycast(handIKRaycastPosition.transform.position, -handIKRaycastPosition.transform.up + playerObject.transform.forward + (playerObject.transform.right * handIKRaycastAngleBetween), out rightHandHit, handIKRaycastDist, ~ignorePlayerLayer)){
            rightHandIK = true;
            rightHandPos = rightHandHit.point - rightHandOffset;
            rightHandPos.x = rightHandOriginalPos.x;
            rightHandPos.z = rightFootPos.z - rightHandOffset.z;
            rightHandRot = Quaternion.FromToRotation(Vector3.up, rightHandHit.normal);
        } else {
            rightHandIK = false;
        }
        */

        if(Physics.Raycast(rightHandIKRaycastPosition.position, playerObject.transform.forward, out rightHandHit, handIKRaycastDist, ~ignorePlayerLayer)){
            rightHandIK = true;
            rightHandPos = rightHandHit.point - rightHandOffset;
            rightHandPos.x = rightHandOriginalPos.x;
            //rightHandPos.z = rightFootPos.z - rightHandOffset.z;
            rightHandRot = Quaternion.FromToRotation(Vector3.up, rightHandHit.normal);
        } else {
            rightHandIK = false;
        }


        if(Physics.Raycast(leftFootIKRaycastPosition.transform.position, transform.forward, out leftFootHit, footIKRaycastDist, ~ignorePlayerLayer)){
            leftFootIK = true;
            leftFootPos = leftFootHit.point - leftFootOffset;
            leftFootRot = Quaternion.FromToRotation(Vector3.up, leftFootHit.normal) * leftFootRotOffset;
        } else {
            leftFootIK = false;
        }

        if(Physics.Raycast(rightFootIKRaycastPosition.transform.position, transform.forward, out rightFootHit, footIKRaycastDist, ~ignorePlayerLayer)){
            rightFootIK = true;
            rightFootPos = rightFootHit.point - rightFootOffset;
            rightFootRot = Quaternion.FromToRotation(Vector3.up, rightFootHit.normal) * rightFootRotOffset;
        } else {
            rightFootIK = false;
        }


    }

    private void HandleTransformRotations()
    {
        playerYRotation = playerObject.transform.rotation.y;
    }

    private void OnAnimatorIK(int layerIndex) {
        if(useIK){

            leftHandOriginalPos = animator.GetIKPosition(AvatarIKGoal.LeftHand);
            rightHandOriginalPos = animator.GetIKPosition(AvatarIKGoal.RightHand);
            if(leftHandIK){
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, handIKWeight);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRot);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, handIKWeight);
            }

            if(rightHandIK){
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, handIKWeight);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandRot);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, handIKWeight);

            }

            if(leftFootIK){
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPos);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRot);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
            }

            if(rightFootIK){
                animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPos);
                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
                animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRot);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
            }

        }
    }

    public Vector3 GetRightHandIKPos(){
        return Vector3.zero;
    }
}
