using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetExperience : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetFloat("AirBalloonVisit", 0);
        PlayerPrefs.SetFloat("GroundVisit", 0);
        PlayerPrefs.Save();
    }
}
