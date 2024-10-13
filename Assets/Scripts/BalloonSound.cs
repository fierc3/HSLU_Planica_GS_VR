using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public AudioClip moveSound;
    public AudioClip directionChangeSound;
    public float soundCooldown = 1.0f; // Time in seconds between sounds

    private Vector3 lastPosition;
    private float lastSoundTime;
    private Transform childTransform;

    private void Start()
    {
        lastPosition = transform.position;
        lastSoundTime = -soundCooldown; // Allow sound to play immediately
        childTransform = transform.GetChild(0).transform;
    }

    private void Update()
    {
        float currentTime = Time.time;
        Vector3 currentPosition = transform.position;

        if (currentPosition != lastPosition)
        {
            if (currentTime - lastSoundTime >= soundCooldown)
            {
                Debug.Log("+++MOVE SOUND: " + currentTime);
                SoundManager.Instance.PlaySound(moveSound, childTransform);
                lastSoundTime = currentTime;
            }

            if (HasDirectionChanged(currentPosition))
            {
                Debug.Log("+++DIRECTION SOUND: " + currentTime);
                SoundManager.Instance.PlaySound(directionChangeSound, childTransform);
                lastSoundTime = currentTime;
            }

            lastPosition = currentPosition;
        }
    }

    private bool HasDirectionChanged(Vector3 currentPosition)
    {
        Vector3 direction = (currentPosition - lastPosition).normalized;
        Vector3 lastDirection = (lastPosition - transform.position).normalized;

        return direction != lastDirection;
    }
}

