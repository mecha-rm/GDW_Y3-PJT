using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// countdown timer (uses DLL)
public class CountdownTimer : TimerObject
{
    // dll name
    const string DLL_NAME = "GDW_Y3-Countdown";

    // functions from the DLL

    // gets the current time
    [DllImport(DLL_NAME)]
    private static extern float GetCurrentTime();

    // sets the current time
    [DllImport(DLL_NAME)]
    private static extern void SetCurrentTime(float newTime);

    // resets the timer
    [DllImport(DLL_NAME)]
    private static extern void ResetTimer();

    // sets the start time
    [DllImport(DLL_NAME)]
    private static extern void SetStartTime(float time);

    // returns the start time
    [DllImport(DLL_NAME)]
    private static extern float GetStartTime();

    // checks to see if the countdown timer is finished.
    [DllImport(DLL_NAME)]
    private static extern bool IsFinished();

    // update timer
    [DllImport(DLL_NAME)]
    private static extern void UpdateTimer(float deltaTime);

    // if 'true', the stopwatch stops getting updates.
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // get current time value
    public override float GetCurrentTimeValue()
    {
        return GetCurrentTime();
    }

    // set current time value
    public override void SetCurrentTimeValue(float time)
    {
        SetCurrentTime(time);
    }

    // gets the current time
    public float GetCurrentCountdownTime()
    {
        return GetCurrentTime();
    }

    // sets hte current countdown time.
    public void SetCurrentCountdownTime(float time)
    {
        SetCurrentTime(time);
    }

    // resets the countdown timer.
    public void ResetCountdownTimer()
    {
        ResetTimer();
    }

    // sets the start time
    public void SetCountdownStartTime(float time)
    {
        SetStartTime(time);
    }

    // returns the start time
    public float GetCountdownStartTime()
    {
        return GetStartTime();
    }

    // checks to see if the timer is finished
    public bool IsDone()
    {
        return IsFinished();
    }

    // update timer
    private void UpdateCountdownTimer(float deltaTime)
    {
        UpdateTimer(deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        // if the countdown timer is not paused
        if (!paused)
            UpdateTimer(Time.deltaTime);
    }
}
