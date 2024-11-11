using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoControls : MonoBehaviour
{

    [SerializeField]
    private VideoPlayer player;

    [SerializeField]
    private VideoClip[] clips;

    private int currentClip = 0;

    public void NextVideo()
    {
        if (player == null || player.isPaused) return;

        currentClip = currentClip >= clips.Length - 1 ? 0 : currentClip + 1 ;
        player.clip = clips[currentClip];
        player.Play();
    }
}
