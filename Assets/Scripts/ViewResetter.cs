using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;

public class ViewResetter : MonoBehaviour
{
    public XROrigin xrOrigin;
    public Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        //xrOrigin = FindAnyObjectByType<XROrigin>();
        startPosition = xrOrigin.transform.localPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // Change the key as needed 
        {
            Debug.Log("Resetting");
            //StartCoroutine(Reset());
            ResetSubsystem();
            xrOrigin.transform.localPosition = startPosition;
            /*
            InputTracking.Recenter();
            */

        }

        if (Input.GetKeyDown(KeyCode.N)) // Change the key as needed 
        {
            Debug.Log("Restarting");
            System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe")); // Start a new instance of the application
            Application.Quit(); // Quit the current instance
        }


        if (Input.GetKeyDown(KeyCode.X)) // Change the key as needed 
        {
            Debug.Log("Exiting");
            var logger = FindAnyObjectByType<PlayerPositionLogger>();
            if(logger != null)
            {
                logger.ExitLogging(true);
            }
            Application.Quit(); // Quit the current instance
        }

    }


    void ResetSubsystem()
    {
        var inputSubsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetInstances(inputSubsystems);
        Debug.Log("Subsystems: " + inputSubsystems.Count);

        foreach (var subsystem in inputSubsystems)
        {
            Debug.Log(subsystem.ToString());
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

    IEnumerator Reset()
    {
        xrOrigin.transform.localPosition = startPosition;
        yield return new WaitForSeconds(0.05f);
        xrOrigin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        yield return new WaitForSeconds(0.1f);
        xrOrigin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
    }
}
