using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class FixPosition : MonoBehaviour
{
    private XROrigin xrOrigin;
    public float desiredHeight = 1.8f; // Set your desired height here

    void Start()
    {
        xrOrigin = GetComponent<XROrigin>();
        StartCoroutine(SetHeightAfterDelay());
    }

    IEnumerator SetHeightAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // Wait for a short delay
        Vector3 currentPosition = this.transform.position;
        xrOrigin.MoveCameraToWorldLocation(new Vector3(currentPosition.x, currentPosition.y  + desiredHeight, currentPosition.z));
    }
}
