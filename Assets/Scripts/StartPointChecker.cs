using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class StartPointChecker : MonoBehaviour
{
    private GameObject head;
    private bool isOutOfBounds = false;

    // Start is called before the first frame update
    void Start()
    {
        head = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // Change the key as needed 
        {
            var inputSubsystems = new List<XRInputSubsystem>();
            SubsystemManager.GetInstances(inputSubsystems);
            Debug.Log("Subsystems: " + inputSubsystems.Count);

            foreach (var subsystem in inputSubsystems)
            {
                if (subsystem.TryRecenter())
                {
                    Debug.Log("View recentered successfully.");
                }
                else
                {
                    Debug.LogWarning("Failed to recenter view.");
                }
            }
        }

        var distance = 0.5f;
        if(
            Mathf.Abs(head.transform.position.x - this.transform.position.x) > distance
            || Mathf.Abs(head.transform.position.z - this.transform.position.z) > distance
            )
        {
            if (!isOutOfBounds) // was it previously not out of bounds, then we should notify
            {
                EventManager.FireDisplayTutorial(Globals.TUT_BAD_CALIBRATION);
            }
            isOutOfBounds = true;
        }
        else
        {
            if (isOutOfBounds) // was it previously out of bounds, now not anymore, then we should notify
            {
                EventManager.FireDisplayTutorial(Globals.TUT_START);
            }
            isOutOfBounds = false;
        }
        
    }
}
 