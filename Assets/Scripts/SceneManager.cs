using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    private void Start()
    {
        var active = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        StartScene(active, LoadSceneMode.Single);
    }

    void StartScene(Scene scene, LoadSceneMode m)
    {
        Debug.Log("Reveal");
        EventManager.FireRevealEvent();
    }

    private void OnEnable()
    {
        Debug.Log("On Enable Scene Manager");
        EventManager.SwitchScene += SwitchSceneHandler;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += StartScene;
    }

    private void OnDisable()
    {
        EventManager.SwitchScene -= SwitchSceneHandler;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= StartScene;
    }

    private void SwitchSceneHandler(string sceneName)
    {
        Debug.Log("Should switch scenes now to " + sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
