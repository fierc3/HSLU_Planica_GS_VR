using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    // Define a delegate and an event for a specific action
    public delegate void ActionEvent();
    public static event ActionEvent HidePlayer;
    public static event ActionEvent RevealPlayer;
    public static event ActionEvent HideComplete;
    public static event Action<string> SwitchScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to invoke the event
    public static void FireHideEvent()
    {
        HidePlayer?.Invoke();
    }

    public static void FireRevealEvent()
    {
        RevealPlayer?.Invoke();
    }

    public static void FireSwitchSceneEvent(string sceneName)
    {
        SwitchScene?.Invoke(sceneName);
    }

    public static void FireHideCompleteEvent()
    {
        HideComplete?.Invoke();
    }
}
