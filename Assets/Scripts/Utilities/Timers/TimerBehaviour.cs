using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// behaviour for timers.
public abstract class TimerBehaviour : MonoBehaviour
{
    // gets the current time value
    public abstract float GetCurrentTimeValue();

    // sets the current time value
    public abstract void SetCurrentTimeValue(float time);
}
