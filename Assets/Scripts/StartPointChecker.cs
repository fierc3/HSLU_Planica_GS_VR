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
 