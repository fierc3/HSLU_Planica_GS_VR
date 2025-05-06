using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;

public class ViewResetter : MonoBehaviour
{
    public XROrigin xrOrigin;
    public Vector3 startPosition;
    public bool isHotAirBalloon = true;
    private bool isInitialized;

    void OnEnable()
    {
        XRInputSubsystem subsystem = GetXRInputSubsystem();
        if (subsystem != null)
        {
            subsystem.trackingOriginUpdated += OnTrackingOriginUpdated;
        }
        OpenXRSettings.SetAllowRecentering(true);
    }

    void OnDisable()
    {
        XRInputSubsystem subsystem = GetXRInputSubsystem();
        if (subsystem != null)
        {
            subsystem.trackingOriginUpdated -= OnTrackingOriginUpdated;
        }
    }

    void OnTrackingOriginUpdated(XRInputSubsystem subsystem)
    {
        // We keep this for debugging purposes
        var current = subsystem.GetTrackingOriginMode();
        Debug.Log("CurrentMode: " + current);
    }

    XRInputSubsystem GetXRInputSubsystem()
    {
        List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetInstances(subsystems);
        return subsystems.FirstOrDefault();
    }

    void Start()
    {
        startPosition = xrOrigin.transform.position;
    }

    void Update()
    {
        if (!isInitialized && IsHardwarePresent())
        {
            isInitialized = true;
            StartCoroutine(InitializeCoroutine());
        }

        if (Input.GetKeyDown(KeyCode.R)) 
        {
            Debug.Log("Resetting");
            isInitialized = false;
            ResetSubsystem();

        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("Restarting");

            /*
            var logger = FindAnyObjectByType<PlayerPositionLogger>();
            if (logger != null)
            {
                logger.ExitLogging(true);
            }
            */
            System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe")); // Start a new instance of the application
            Application.Quit(); // Quit the current instance
        }


        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Exiting");
            /*
            var logger = FindAnyObjectByType<PlayerPositionLogger>();
            if(logger != null)
            {
                logger.ExitLogging(true);
            }
            */
            Application.Quit(); // Quit the current instance
        }

    }

    public static bool IsHardwarePresent()
    {
        var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
        SubsystemManager.GetInstances<XRDisplaySubsystem>(xrDisplaySubsystems);

        foreach (var xrDisplay in xrDisplaySubsystems)
        {
            if (xrDisplay.running)
            {
                Debug.Log("XR Display is Running");
                return true;
            }
        }

        return false;
    }

    private IEnumerator InitializeCoroutine()
    {
        xrOrigin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
        yield return new WaitForSeconds(0.1f);
        xrOrigin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;

        if (isHotAirBalloon)
        {
            Vector3 parentPosition = transform.parent.position; // Assuming the moving object is the parent
            Vector3 targetPosition = new Vector3(parentPosition.x, xrOrigin.transform.position.y + xrOrigin.CameraInOriginSpaceHeight, parentPosition.z);
            xrOrigin.MoveCameraToWorldLocation(targetPosition);
            xrOrigin.Camera.transform.localPosition = Vector3.zero; // Reset camera's local position
        }
        else
        {
           xrOrigin.MoveCameraToWorldLocation(new Vector3(startPosition.x, startPosition.y + xrOrigin.CameraInOriginSpaceHeight, startPosition.z));
        }
    }

    private void ResetSubsystem()
    { 
        var inputSubsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetInstances(inputSubsystems);
        Debug.Log("Subsystems: " + inputSubsystems.Count);

        foreach (var subsystem in inputSubsystems)
        {
            subsystem.Stop();
            subsystem.Start();

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
}

