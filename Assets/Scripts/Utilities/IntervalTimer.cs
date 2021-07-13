using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntervalTimer : MonoBehaviour
{
    // if set to 'true', the interval is applied once the script is started.
    public bool startOnInterval = false;

    // if 'true', intervals are used. If not, a check for each interval always returns true.
    // this pauses the current countdown.
    public bool useIntervals = true;

    // time between intervals (in seconds)
    public float intervalLength = 1.0F;
    public float countdown = 0.0F;

    // Start is called before the first frame update
    void Start()
    {
        // if the program should start on the interval. 
        if (startOnInterval)
            countdown = intervalLength;
    }

    // checks to see if the interval has ended. If useIntervals is set to false, this always returns true.
    // if true, the interval is restarted. If useIntervals is false, the interval timer is set to zero.
    public bool IntervalEnded()
    {
        // if intervals are not being used, this always returns true.
        if (!useIntervals)
            return true;

        // countdown has ended.
        if (countdown <= 0)
        {
            countdown = intervalLength;
            return true;
        }

        // interval has not passed.
        return false;
    }

    // restarts the countdown to be set the current interval
    public void RestartCountdown()
    {
        countdown = intervalLength;
    }

    // Update is called once per frame
    void Update()
    {
        // interval timer
        if (useIntervals && countdown > 0)
        {
            // reduces countdown
            countdown -= Time.deltaTime;

            // countdown has reached zero or below.
            if (countdown < 0)
                countdown = 0.0F;
        }
            
    }
}
