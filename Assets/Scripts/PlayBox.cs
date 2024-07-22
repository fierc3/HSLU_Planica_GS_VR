using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayBox : MonoBehaviour
{
    [SerializeField] 
    private VideoPlayer videoPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (videoPlayer == null)
        {
            return;
        }

        videoPlayer.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if (videoPlayer == null)
        {
            return;
        }

        videoPlayer.Stop();
    }
}
