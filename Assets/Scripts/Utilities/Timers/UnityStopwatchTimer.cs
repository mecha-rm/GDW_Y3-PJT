using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityStopwatchTimer : TimerBehaviour
{
    // current time
    public float currentTime = 0.0F;

    // the split.
    float split = 0.0F;

    // splits the time
    public List<float> splits = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // resets the timer
    public void ResetTimer()
    {
        splits.Clear();
        currentTime = 0.0F;
    }

    // gets the current time
    public override float GetCurrentTimeValue()
    {
        return currentTime;
    }

    // sets hte current time
    public override void SetCurrentTimeValue(float time)
    {
        currentTime = time;
    }

    // returns the total time that has passed.
    public float GetTotalTime()
    {
        // the total time
        float totalTime = 0.0F;

        foreach (float value in splits)
            totalTime += value;

        // increments with current time.
        totalTime += currentTime;
        
        // returns the total time
        return totalTime;
    }

    // gets the total number of splits.
    public int GetSplitCount()
    {
        return splits.Count;
    }

    // gets the split at the provided index.
    // if there is no value at the provided index, then 0 is returned.
    public float GetSplit(int index)
    {
        // returns the split value.
        if (index >= 0 && index < splits.Count)
            return splits[index];
        else
            return -1;
    }

    // splits the time.
    public void Split()
    {
        splits.Add(currentTime); // saves the current time
        currentTime = 0.0F; // sets the current time back to 0.
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
    }
}
