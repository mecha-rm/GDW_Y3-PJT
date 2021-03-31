using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityCountdownTimer : TimerBehaviour
{
    // current time
    public float currentTime = 0.0F;

    // start time
    public float startTime = 0.0F;

    // if 'true', the timer is paused.
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // resets the timer
    public void ResetTimer()
    {
        currentTime = startTime;
    }

    // gets the current time value
    public override float GetCurrentTimeValue()
    {
        return currentTime;
    }

    // sets the current time value
    public override void SetCurrentTimeValue(float time)
    {
        currentTime = time;
    }

    // sets the start time
    public void SetStartTime(float time)
    {
        startTime = time;
    }

    // gets the start time
    public float GetStartTime()
    {
        return startTime;
    }

    // checks to see if the countdown timer is finished.
    public bool IsFinished()
    {
        // checks countdown.
        if (currentTime <= 0)
            return true;
        else
            return false;
    }

    // Update is called once per frame
    void Update()
    {

        // if the timer is not paused.
        if(!paused)
        {
            currentTime -= Time.deltaTime;

            // if the current time has reached 0.
            if (currentTime < 0.0F)
                currentTime = 0.0F;
        }
    }

}

