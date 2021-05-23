using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [SerializeField] Transform playerTransform;
    [SerializeField] float  playerTransformOffsetX;
    [SerializeField] float  playerTransformOffsetY;
    [SerializeField] float  playerTransformOffsetZ;

    private Vector3 offset;

    private void Start() {
        offset = new Vector3(playerTransformOffsetX, playerTransformOffsetY, playerTransformOffsetZ);
    }

    void FixedUpdate()
    {
        this.transform.position = playerTransform.position + offset;
    }
}
