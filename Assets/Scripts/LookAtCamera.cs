using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    public Camera playerCamera;


    // Start is called before the first frame update
    void Start()
    {
       playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player camera is assigned
        if (playerCamera != null)
        {
            // Make the canvas face the camera
            transform.LookAt(
                transform.position + playerCamera.transform.rotation * Vector3.forward,
                //playerCamera.transform.rotation * Vector3.up);
                Vector3.up);
        }
    }
}
