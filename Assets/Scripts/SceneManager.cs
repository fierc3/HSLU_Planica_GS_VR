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
        EventManager.FireRevealEvent();
    }

    private void OnEnable()
    {
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
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
