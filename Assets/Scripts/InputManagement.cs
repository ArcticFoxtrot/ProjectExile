using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagement : MonoBehaviour
{

    private float horizontalInput;
    private float verticalInput;
    private float cameraHorizontalInput;
    private float cameraVerticalInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Debug.Log("camera input is " + cameraHorizontalInput + " and " + cameraVerticalInput);
    }
}
