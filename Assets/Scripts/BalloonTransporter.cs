using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;
using static UnityEngine.GraphicsBuffer;

public class BalloonTransporter : MonoBehaviour
{
    IEnumerable<GameObject> splats;
    bool rightArrowPressed = false;
    bool rightTriggered = false;
    bool singleTriggered = false;
    Vector3 originalPosition;
    Quaternion originalRotation;

    // Teleporting
    Globals globals;
    private string sceneName = "InspectScene";

    [SerializeField]
    int active = 0;

    // Portal attributes
    [SerializeField]
    MeshRenderer portal;
    [SerializeField]
    private float startValue = 20.0f;
    [SerializeField]
    private float endValue = 0.9f;
    [SerializeField]
    private float duration = 2.0f;
    
    // Movement attributes
    [SerializeField]
    private float moveDuration = 12.0f;
    [SerializeField]
    private SplineAnimate splineAnimate;

    // Start is called before the first frame update
    void Start()
    {
        this.globals = GameObject.FindGameObjectWithTag("Globals").GetComponent<Globals>();
        return;
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
        if (singleTriggered) return;

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
        singleTriggered = true;
        Debug.Log("Next Splat Please: " + active);
        this.splineAnimate.Pause();
        StartCoroutine(ShowPortal());
        StartCoroutine(MoveToTarget());
        return;
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

    private IEnumerator ShowPortal()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float currentValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            portal.material.SetFloat("_Brightness", currentValue);
            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set
        portal.material.SetFloat("_Brightness", endValue);
    }

    private IEnumerator MoveToTarget()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = portal.gameObject.transform.position;
        float timeElapsed = 0f;

        while (timeElapsed < moveDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / moveDuration;

            Vector3 curvePosition = (1 - t) * (1 - t) * startPosition + 2 * (1 - t) * t * portal.gameObject.transform.position + t * t * endPosition;
            transform.position = curvePosition;

            Vector3 direction = (endPosition - transform.position).normalized;
            direction.y = 0; // Keep the y component zero to avoid tilting up or down
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * (1 / duration));

            if(Vector3.Distance(transform.position, endPosition) < 5)
            {
                GoToLocation();
                yield break;
            }

            yield return null;
        }

        // Ensure the object reaches the exact target position at the end
        transform.position = endPosition;
    }

    public void GoToLocation()
    {
        EventManager.HideComplete += OnHidden;
        globals.Mode = Mode.NonInteractive;
        EventManager.FireHideEvent();
    }

    private void OnHidden()
    {
        EventManager.HideComplete -= OnHidden;
        Debug.Log("OnHidden for " + sceneName);
        EventManager.FireSwitchSceneEvent(sceneName);
    }

}