using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.Interaction.Toolkit.Utilities;
using UnityEngine.SocialPlatforms;

public class ViewResetter : MonoBehaviour
{
    public XROrigin xrOrigin;
    public Vector3 startPosition;
    public bool isHotAirBalloon = true;
    private bool isInitialized;
    private TrackingOriginModeFlags previousTrackingMode = TrackingOriginModeFlags.Unknown;

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
        var current = subsystem.GetTrackingOriginMode();
        Debug.Log("CurrentMode: " + current);
        Debug.Log("previousTrackingMode: " + previousTrackingMode);
        Debug.Log("xrOrigin.CameraYOffset" + xrOrigin.CameraYOffset);
        Debug.Log("xrOrigin.GetCameraFloorWorldPosition();" + xrOrigin.GetCameraFloorWorldPosition());


        //xrOrigin.Camera.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, 0.2f, Camera.main.transform.localPosition.z);
        if (previousTrackingMode == TrackingOriginModeFlags.Floor && current == TrackingOriginModeFlags.Floor && isInitialized)
        {
            // This must be a quest link triggered reset, because we switch between floor and device
            // Your custom logic here
            Debug.Log("N0 OnTrackingOriginUpdated: " + xrOrigin.transform.localPosition);
            Debug.Log("N0 OnTrackingOriginUpdated: " + xrOrigin.transform.localScale);
            // Debug.Log("N0 OnTrackingOriginUpdated: " + subsystem.);

            Debug.Log("Tracking origin updated (recentered). " + xrOrigin.CurrentTrackingOriginMode);
        }
           //xrOrigin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
           // xrOrigin.MoveCameraToWorldLocation(new Vector3(startPosition.x, xrOrigin.Camera.transform.position.y, startPosition.z));
        previousTrackingMode = current;
    }

    XRInputSubsystem GetXRInputSubsystem()
    {
        List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetInstances(subsystems);
        return subsystems.FirstOrDefault();
    }


    private void Awake()
    {
        Debug.Log("N0 wake: " + xrOrigin.transform.position);  
    }

    void Start()
    {
        startPosition = xrOrigin.transform.position;
        Debug.Log("N0 Start: " + xrOrigin.transform.position);
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
            var logger = FindAnyObjectByType<PlayerPositionLogger>();
            if (logger != null)
            {
                logger.ExitLogging(true);
            }
            System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe")); // Start a new instance of the application
            Application.Quit(); // Quit the current instance
        }


        if (Input.GetKeyDown(KeyCode.X))
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
        Debug.Log("Reiniting floor mode");
        xrOrigin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
        yield return new WaitForSeconds(0.1f);
        xrOrigin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;

        if (isHotAirBalloon)
        {
            Vector3 parentPosition = transform.parent.position; // Assuming the moving object is the parent
            Vector3 targetPosition = new Vector3(parentPosition.x, xrOrigin.transform.position.y + xrOrigin.CameraInOriginSpaceHeight, parentPosition.z);
            Debug.Log("Parent Position: " + parentPosition);
            Debug.Log("Target Position: " + targetPosition);
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

