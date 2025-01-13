using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayBox : MonoBehaviour
{
    [SerializeField] 
    private VideoPlayer videoPlayer;
    private GameObject head;
    private Collider playBoxCollider;

    void Start()
    {
        if(videoPlayer == null)
        {
            Debug.Log("No video player assigned, will ignore PlayBox");
            return;
        }

        head = GameObject.FindGameObjectWithTag("MainCamera");
        playBoxCollider = GetComponent<Collider>();
        StartCoroutine(CheckHeadPosition());
    }


    IEnumerator CheckHeadPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (playBoxCollider.bounds.Contains(head.transform.position))
            {
                // Head is inside the collider
                if (!videoPlayer.isPlaying)
                {
                    videoPlayer.Play();
                }
            }
            else
            {
                // Head is outside the collider
                if (videoPlayer.isPlaying)
                {
                    videoPlayer.Stop();
                }
            }
        }
    }
}

