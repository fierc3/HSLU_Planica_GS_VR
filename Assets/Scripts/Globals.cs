using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public Mode Mode { get; set; } = Mode.NonInteractive;
}

public enum Mode
{
    NonInteractive,
    Interactive
}
