using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{

    private Globals globals;
    [SerializeField]
    private bool SkipTransition = false;
    [SerializeField]
    private float Delay = 1f;
    [SerializeField]
    private float Duration = 1f;
    [SerializeField]
    private float FromValue = 0f;
    [SerializeField]
    private float ToValue = 1f;

    private void OnEnable()
    {
        globals = GameObject.FindGameObjectWithTag("Globals").GetComponent<Globals>();
        EventManager.HidePlayer += HidePlayerHandler;
        EventManager.RevealPlayer += RevealPlayerHandler;
    }

    private void OnDisable()
    {
        EventManager.HidePlayer -= HidePlayerHandler;
        EventManager.RevealPlayer -= RevealPlayerHandler;
    }


    private void HidePlayerHandler()
    {
        globals.Mode = Mode.NonInteractive;
        StartCoroutine(PlayAnimation(1f, 0f));
    }    
    
    private void RevealPlayerHandler()
    {
        globals.Mode = Mode.NonInteractive;
        Debug.Log("stargin reveal animation");
        if (SkipTransition)
        {
            StartCoroutine(PlayAnimation(1f, 1.1f));
        }
        else
        {
            StartCoroutine(PlayAnimation(this.FromValue, this.ToValue));
        }
    }

    IEnumerator PlayAnimation(float initial, float target)
    {
        yield return new WaitForSeconds(this.Delay); // Wait for 1 second before starting the reveal

        float t = 0f;
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();

        // Initial value of the dissolve amount
        float initialDissolveAmount = initial; // Assuming the effect starts fully "dissolved"
        float targetDissolveAmount = target; // Target value to fully "reveal" the object

        while (t < 1f)
        {
            t += Time.deltaTime / this.Duration; // Adjust the divisor to change the speed of the transition
            float dissolveAmount = Mathf.Lerp(initialDissolveAmount, targetDissolveAmount, t);
            meshRenderer.material.SetFloat("_Dissolve", dissolveAmount);
            yield return null; // Wait until the next frame
        }

        if(target > initial)
        {
            // Reveal is done
            globals.Mode = Mode.Interactive;
        }
        else
        {
            // Hide is done
            globals.Mode = Mode.NonInteractive;
            EventManager.FireHideCompleteEvent();
        }
    }
}
