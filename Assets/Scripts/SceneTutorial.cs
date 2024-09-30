using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SceneTutorial : MonoBehaviour
{
    [SerializeField]
    private PreferenceCategory category;
    private SplineAnimate[] splineAnimates;
    private HoverboardController[] hoverboards;

    // Start is called before the first frame update
    void Start()
    {
        this.splineAnimates = FindObjectsOfType<SplineAnimate>();
        this.hoverboards = FindObjectsOfType<HoverboardController>();

        var count = PlayerPrefs.GetInt(category.ToString());
        PlayerPrefs.SetInt(category.ToString(), count + 1);
        if (count > 100000)
        {
            Destroy(gameObject);
            return;
        }

        SetPlayOnAllSplines(false);
        SetPlayOnHoverboards(false);
        Time.timeScale = 0.8f;
    }

    public void ConfirmButtonPressed()
    {
        SetPlayOnAllSplines(true);
        SetPlayOnHoverboards(true);
        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }

    private void SetPlayOnAllSplines(bool play)
    {
        foreach (var splineAnimate in this.splineAnimates)
        {
            splineAnimate.enabled = play;
        }
    }

    private void SetPlayOnHoverboards(bool play)
    {
        foreach (var board in this.hoverboards)
        {
            board.gameObject.SetActive(play);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
