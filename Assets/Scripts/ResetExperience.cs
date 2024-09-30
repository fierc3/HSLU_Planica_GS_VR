using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetExperience : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt(PreferenceCategory.HotAirBalloonVisits.ToString(), 0);
        PlayerPrefs.SetInt(PreferenceCategory.GroundVisits.ToString(), 0);
        PlayerPrefs.SetInt(PreferenceCategory.InspectVisits.ToString(), 0);
        PlayerPrefs.SetInt(PreferenceCategory.InterviewVisits.ToString(), 0);
        PlayerPrefs.Save();
    }
}
