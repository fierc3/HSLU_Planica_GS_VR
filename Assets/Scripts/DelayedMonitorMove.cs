using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DelayedMonitorMove : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    [SerializeField]
    private float moveDuration = 2.0f; // Duration of the move in seconds
    [SerializeField]
    private float yMovement = 0.35f;    
    [SerializeField]
    private float upDelay = 38f;
    [SerializeField]
    private float downDelay = 144f;
    [SerializeField]
    private VideoPlayer player;

    [SerializeField]
    private AudioClip moveSound;

    

    void Start()
    {
        originalPosition = transform.localPosition;
        targetPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + yMovement, transform.localPosition.z);
        StartCoroutine(MoveUpAndDown());
    }

    IEnumerator MoveUpAndDown()
    {
        yield return new WaitForSeconds(1);

        while(Time.timeScale < 1) // this means were still in tutorial mode, very ugly
        {
            Debug.Log("Waiting...: ");
            yield return new WaitForSeconds(1);
        }

        // either now the VV is playing or it isn't, this dependms on the number of air balloon visits.
        int VisitsOverview = PlayerPrefs.GetInt(PreferenceCategory.HotAirBalloonVisits.ToString());
        Debug.Log("Visits Hot Air Balloon: " + VisitsOverview);

        yield return new WaitForSeconds(VisitsOverview > 1 ? 1f : upDelay); // > 1 because by this time it will already be marked as visit 1

        if (player.gameObject.activeInHierarchy)
        {
            player.Play();

            // Move up smoothly
            yield return StartCoroutine(MoveToPosition(targetPosition, moveDuration));

          
            /*
            yield return new WaitForSeconds(downDelay);

            // Move down smoothly
            yield return StartCoroutine(MoveToPosition(originalPosition, moveDuration));
            player.Stop();
            */
        }
    }

    IEnumerator MoveToPosition(Vector3 target, float duration)
    {
        Vector3 start = transform.localPosition;
        float elapsed = 0;
        SoundManager.Instance.PlaySound(moveSound, transform);

        while (elapsed < duration)
        {
            transform.localPosition = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = target;
    }
}

