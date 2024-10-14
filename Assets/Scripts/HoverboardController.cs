using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverboardController : MonoBehaviour
{
    public float defaultTargetHeight = 30f;
    public float duration = 5f;
    public float shakeIntensity = 0.1f;
    public float shakeFrequency = 20f;
    public float swayIntensity = 0.5f;
    public float swaySpeed = 1f;
    public float hoverTime = 30f;

    private Vector3 originalPosition;
    private float elapsedTime = 0f;
    private bool isSwaying = false;
    private float swayTime = 0f;

    void Start()
    {
        RunAnimation(defaultTargetHeight);
    }

    private void RunAnimation(float targetHeight)
    {
        originalPosition = transform.localPosition;
        StartCoroutine(Move(targetHeight));
    }

    IEnumerator Move(float targetHeight)
    {
        Vector3 targetPosition = originalPosition + new Vector3(0, targetHeight, 0);
        while (elapsedTime < duration)
        {
            transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / duration);
            //Shake();
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = targetPosition;
        isSwaying = true;

        // Wait for hoverTime before moving back down
        yield return new WaitForSeconds(hoverTime);

        // Move back down
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.localPosition = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPosition;
        isSwaying = false;
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isSwaying)
        {
            Sway();
        }
    }

    void Sway()
    {
        swayTime += Time.deltaTime * swaySpeed;
        float swayOffset = Mathf.Sin(swayTime) * swayIntensity;
        transform.localPosition = new Vector3(transform.localPosition.x, originalPosition.y + defaultTargetHeight + swayOffset, transform.localPosition.z);
    }
}
