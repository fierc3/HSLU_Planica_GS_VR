using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{

    private Globals globals;

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
        StartCoroutine(PlayAnimation(0f, 1f));
    }

    IEnumerator PlayAnimation(float initial, float target)
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second before starting the reveal

        float t = 0f;
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();

        // Initial value of the dissolve amount (you can adjust this as needed)
        float initialDissolveAmount = initial; // Assuming the effect starts fully "dissolved"
        float targetDissolveAmount = target; // Target value to fully "reveal" the object

        while (t < 1f)
        {
            t += Time.deltaTime / 1f; // Adjust the divisor to change the speed of the transition
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
