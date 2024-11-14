using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InspectSwitcher : MonoBehaviour
{
    IEnumerable<GameObject> splats;
    bool rightArrowPressed = false;
    bool rightTriggered = false;
    Vector3 originalPosition;
    Quaternion originalRotation;

    [SerializeField]
    int active = 0;

    private bool isResetting = false;

    // Start is called before the first frame update
    void Start()
    {
        splats = GameObject.FindGameObjectsWithTag("InspectorSplat");
        Debug.Log("Splats: " + splats.Count());
        foreach (GameObject go in splats)
        {
            go.SetActive(false);
        }
        Debug.Log("Activating first find: " + splats.First().name);
        splats.First().SetActive(true);
        Debug.Log(splats.First().GetComponentInParent<Transform>().name);
        originalPosition = splats.First().transform.parent.parent.transform.position;
        originalRotation = splats.First().transform.parent.parent.transform.rotation;
        StartCoroutine(DelayedReset());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) // Detect right arrow key press
        {
            rightArrowPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow)) // Detect right arrow key release
        {
            rightArrowPressed = false;
            rightTriggered = false;
        }

        if (rightArrowPressed && !rightTriggered)
        {
            rightTriggered = true;
            NextSplat();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) // Detect right arrow key press
        {
            ResetSplat();
        }
    }


    public void NextSplat()
    {
        Debug.Log("Next Splat Please: " + active);
        splats.ElementAt(active).SetActive(false);
        active = (active + 1) % splats.Count();
        splats.ElementAt(active).SetActive(true);
    }

    public void ResetSplat()
    {
        if (isResetting) return;

        isResetting = true;
        Debug.Log("Reseting Splats Container " + originalPosition);
        //
        // Get the Rigidbody component
        Rigidbody rb = splats.First().transform.parent.parent.GetComponent<Rigidbody>();

        // Disable the Rigidbody
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Reset position and rotation
        splats.First().transform.parent.parent.transform.position = originalPosition;
        splats.First().transform.parent.parent.transform.rotation = originalRotation;

        StartCoroutine(StoppingSplat(rb));
    }

    IEnumerator StoppingSplat(Rigidbody rb)
    {
        yield return new WaitForSeconds(0.33f);
        // Re-enable the Rigidbody
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        isResetting = false;
    }

    IEnumerator DelayedReset()
    {
        yield return new WaitForSeconds(0.33f);
        ResetSplat();
    }
}
