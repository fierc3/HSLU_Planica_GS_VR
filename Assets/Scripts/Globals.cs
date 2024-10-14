using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public Mode Mode { get; set; } = Mode.NonInteractive;

    public static int TUT_DISMISS { get; } = -1;
    public static int TUT_START { get; } = 1;
    public static int TUT_BAD_CALIBRATION { get; } = 2;
}

public enum Mode
{
    NonInteractive,
    Interactive
}

