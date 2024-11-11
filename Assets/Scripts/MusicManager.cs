using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (audioSource == null) return;

        // Check the scene name or index and play the corresponding music
        AudioClip newMusicClip = GetMusicForScene(scene.name);
        if (newMusicClip != null && audioSource.clip != newMusicClip)
        {
            PlayMusic(newMusicClip, 0.05f);
        }
    }

    private AudioClip GetMusicForScene(string sceneName)
    {
        var scene = sceneName.ToLower();
        if (scene == "arenascene" || scene == "stagescene")
        {
            return groundMusic;
        }

        if (scene == "roundbuildingscene" || scene == "woodenbuildingscene" || scene == "metaltowerscene")
        {
            return flyingMusic;
        }

        if (scene == "inspectscene" || scene == "btsscene")
        {
            return interactiveMusic;
        }

        return null;
    }

    public void PlayMusic(AudioClip musicClip, float volume = 1.0f)
    {
        audioSource.clip = musicClip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Assign these in the inspector
    public AudioClip flyingMusic;
    public AudioClip groundMusic;
    public AudioClip interactiveMusic;
}
