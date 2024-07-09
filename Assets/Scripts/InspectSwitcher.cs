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
        Debug.Log("Reseting Splats Container " + originalPosition);
        splats.First().transform.parent.parent.transform.position = originalPosition;
        splats.First().transform.parent.parent.transform.rotation = originalRotation;
    }
}
