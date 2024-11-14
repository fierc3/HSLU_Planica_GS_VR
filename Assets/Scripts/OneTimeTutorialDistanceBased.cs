using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeTutorialDistanceBased : MonoBehaviour
{
    private GameObject head;
    [SerializeField]
    private float distance = 0.5f;
    [SerializeField]
    private Canvas TutorialCanvas;

    private bool IsCleaningUp = false;

    // Start is called before the first frame update
    void Start()
    {
        head = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsCleaningUp) return;

        if (
            Mathf.Abs(head.transform.position.x - this.transform.position.x) > distance
            || Mathf.Abs(head.transform.position.z - this.transform.position.z) > distance
            )
        {
            IsCleaningUp = true;
            Debug.Log("Player moved away, time to delete Tutorial");
            //TutorialCanvas.gameObject.transform.parent.gameObject.SetActive(false);
            try
            {
                TutorialCanvas.GetComponent<SceneTutorial>().FinishTutorial();
            }catch (Exception ex)
            {
                Time.timeScale = 1.0f;
                Debug.LogException(ex);
            }
        }
    }
}
