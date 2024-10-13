using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseTeleporter : MonoBehaviour
{
    [SerializeField] 
    string sceneName;

    [SerializeField]
    private AudioClip buttonClickSound;


    private Globals globals;
    // Start is called before the first frame update
    void Start()
    {
        globals = GameObject.FindGameObjectWithTag("Globals").GetComponent<Globals>();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public void GoToLocation()
    {
        SoundManager.Instance.PlaySound(buttonClickSound, transform);
        EventManager.HideComplete += OnHidden;
        globals.Mode = Mode.NonInteractive;
        EventManager.FireHideEvent();
    }

    private void OnHidden ()
    {
        EventManager.HideComplete -= OnHidden;
        Debug.Log("OnHidden for " + sceneName);
        EventManager.FireSwitchSceneEvent(sceneName);
    }
}

