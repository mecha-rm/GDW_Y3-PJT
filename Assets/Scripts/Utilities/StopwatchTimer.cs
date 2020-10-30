using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// stopwatch timer (uses DLL)
public class StopwatchTimer : MonoBehaviour
{
    // dll name
    const string DLL_NAME = "GDW_Y3-Stopwatch";

    // functions from the DLL

    // gets the current time.
    [DllImport(DLL_NAME)]
    private static extern float GetCurrentTime();

    // resets the timer
    [DllImport(DLL_NAME)]
    private static extern void ResetTimer();

    // gets the total time
    [DllImport(DLL_NAME)]
    private static extern float GetTotalTime();

    // gets the total number of splits
    [DllImport(DLL_NAME)]
    private static extern int GetSplitCount();

    // gets the split at the provided index
    [DllImport(DLL_NAME)]
    private static extern float GetSplit(int index);

    // splits the time, recording the current time on the stopwatch.
    [DllImport(DLL_NAME)]
    private static extern void Split();

    // update
    [DllImport(DLL_NAME)]
    private static extern void UpdateTimer(float deltaTime);

    // if 'true', the stopwatch stops getting updates.
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        // resets timer so that the countdown starts at 0.
        ResetTimer();
    }

    // get current time
    public float GetCurrentStopwatchTime()
    {
        return GetCurrentTime();
    }

    // resets the stopwatch
    public void ResetStopwatch()
    {
        ResetTimer();
    }

    // gets the total time
    public float GetTotalStopwatchTime()
    {
        return GetTotalTime();
    }

    // gets the total number of splits
    public int GetSplitAmount()
    {
        return GetSplitCount();
    }

    // gets the split at the provided index
    public float GetSplitAtIndex(int index)
    {
        return GetSplit(index);
    }

    // splits the time, recording the current time on the stopwatch.
    public void SplitTime()
    {
        Split();
    }

    // update
    private void UpdateStopwatch(float deltaTime)
    {
        UpdateTimer(deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        // if the stopwatch is not paused, update the timer.
        if(!paused)
            UpdateTimer(Time.deltaTime);
    }
}
