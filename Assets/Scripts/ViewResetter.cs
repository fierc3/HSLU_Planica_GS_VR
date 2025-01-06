using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

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
            StartCoroutine(Reset());
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
