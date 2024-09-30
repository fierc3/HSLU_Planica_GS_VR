using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicDisplay : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    [SerializeField]
    private float fadeDuration = 1f;
    [SerializeField]
    private int tutorialId = Globals.TUT_START;

    void Start()
    {
        EventManager.DisplayTutorial += OnTutorial;
        canvasGroup = this.GetComponent<CanvasGroup>();
    }

    private void OnTutorial(int id)
    {
        try
        {
            StopAllCoroutines();
        }
        catch (Exception e)
        {
            Debug.LogWarning("No coroutines to stop: " + e.Message);
            return;
        }


        Debug.Log("On tutorial "  + id);
        if(id == tutorialId)
        {
            StartCoroutine(FadeCanvasGroup(true));
        }
        else
        {
            StartCoroutine(FadeCanvasGroup(false));
        }
    }

    public IEnumerator FadeCanvasGroup(bool fadeIn)
    {
        float startAlpha = fadeIn ? 0 : canvasGroup.alpha;
        float endAlpha = fadeIn ? 1 : 0.00f;
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
            progress += rate * Time.deltaTime;

            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
