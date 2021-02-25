using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [SerializeField] Transform playerTransform;

    void FixedUpdate()
    {
        this.transform.position = playerTransform.position;
    }
}
