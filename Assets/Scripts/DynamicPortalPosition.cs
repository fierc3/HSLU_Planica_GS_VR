using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DynamicPortalPosition : MonoBehaviour
{
    private Vector3 OgPosition;

    private void Start()
    {
        OgPosition = transform.position;
    }

    public void SetPortalPosition(Vector3 position)
    {
        if (position == Vector3.zero)
        {
            this.transform.position = OgPosition;
            return;
        }

        var targetPositon = Vector3.Lerp(position, OgPosition, 0.2f);
        StartCoroutine(DelayPosition(targetPositon));
    }

    IEnumerator DelayPosition(Vector3 targetPosition)
    {
        yield return new WaitForSeconds(0.3f); //holy is this ugly...but reliable
        
        this.transform.position = targetPosition;
    }
}
