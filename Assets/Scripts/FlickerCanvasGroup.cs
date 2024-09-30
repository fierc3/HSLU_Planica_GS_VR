using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerCanvasGroup : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float flickerSpeed = 1.0f; // Adjust this to change the speed of flickering

    private bool increasing = true;
    private float targetAlpha;

    void Start()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        targetAlpha = 0.7f;
    }

    void Update()
    {
        if (increasing)
        {
            canvasGroup.alpha += flickerSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= targetAlpha)
            {
                increasing = false;
                targetAlpha = 0.3f;
            }
        }
        else
        {
            canvasGroup.alpha -= flickerSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= targetAlpha)
            {
                increasing = true;
                targetAlpha = 0.7f;
            }
        }
    }
}
