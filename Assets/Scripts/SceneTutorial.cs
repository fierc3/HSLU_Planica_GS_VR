using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Splines;

public class SceneTutorial : MonoBehaviour
{
    [SerializeField]
    private PreferenceCategory category;
    private SplineAnimate[] splineAnimates;
    private HoverboardController[] hoverboards;

    [SerializeField]
    private VolPlayer PostTutorialVolumetricVideo;

    [SerializeField]
    private AudioClip buttonClickSound;

    // Start is called before the first frame update
    void Start()
    {
        this.splineAnimates = FindObjectsOfType<SplineAnimate>();
        this.hoverboards = FindObjectsOfType<HoverboardController>();

        var count = PlayerPrefs.GetInt(category.ToString());
        PlayerPrefs.SetInt(category.ToString(), count + 1);
        if (count > 0)
        {
            foreach (var board in this.hoverboards)
            {
                board.gameObject.SetActive(false);
            }
            Destroy(gameObject);
            Time.timeScale = 1f;
            return;
        }

        SetPlayOnAllSplines(false);
        SetPlayOnHoverboards(false);
        Time.timeScale = 0.99f;
    }

    public void ConfirmButtonPressed()
    {
        FinishTutorial();
        SoundManager.Instance.PlaySound(buttonClickSound, transform);
    }

    public void FinishTutorial()
    {
        SetPlayOnAllSplines(true);
        SetPlayOnHoverboards(true);

        if (PostTutorialVolumetricVideo)
        {
            PostTutorialVolumetricVideo.gameObject.SetActive(true);
        }

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
            board.enabled = play;
        }
    }
}
