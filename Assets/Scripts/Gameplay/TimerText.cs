using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerText : MonoBehaviour
{
    // text object
    public Text text = null;

    // the timer being used
    // the timer being 0 or negative means no timer is being used.
    public int timerNumber = 0;

    // countdown timer
    public CountdownTimer timer1 = null;
    public float countdownStart = 0;

    // stop watch timer
    public StopwatchTimer timer2 = null;

    // pause time
    public bool paused = false;

    // override the pause variable in the individual timers
    public bool overrideTimerPause = true;

    // Start is called before the first frame update
    void Start()
    {
        // initializes timers if they don't exist.
        if(text == null)
        {
            // if no text object has been set, it gets the text component.
            text = GetComponent<Text>();
        }

        // countdown timer
        if (timer1 == null)
        {
            // searches for countdown timer attached to object.
            timer1 = FindObjectOfType<CountdownTimer>();

            // if no timer is attached, a new one is made.
            if (timer1 == null)
            {
                timer1 = gameObject.AddComponent(typeof(CountdownTimer)) as CountdownTimer;
            }

            // sets countdown start time.
            timer1.SetCountdownStartTime(countdownStart, true);
        }

        // stopwatch timer.
        if (timer2 == null)
        {
            // searches for a stopwatch timer attached to the object.
            timer2 = FindObjectOfType<StopwatchTimer>();

            // if no timer is attached, a new one is made.
            if (timer2 == null)
            {
                timer2 = gameObject.AddComponent(typeof(StopwatchTimer)) as StopwatchTimer;
            }
        }
    }


    // gets the active timer.
    public TimerObject GetActiveTimer()
    {
        // gets the actve timer.
        TimerObject timeObject = null;

        switch (timerNumber)
        {
            case 1: // countdown
                timeObject = timer1;
                break;

            case 2: // stopwatch
                timeObject = timer2;
                break;

            default:
                break;
        }

        return timeObject;
    }


    // pauses the active timer
    public void PauseActiveTimer()
    {
        // checks timer number
        switch (timerNumber)
        {
            case 1: // countdown
                timer1.paused = true;
                break;

            case 2: // stopwatch
                timer2.paused = true;
                break;

            default:
                break;
        }
    }

    // unpauses the active timer
    public void UnpauseActiveTimer()
    {
        // checks timer number
        switch (timerNumber)
        {
            case 1: // countdown
                timer1.paused = false;
                break;

            case 2: // stopwatch
                timer2.paused = false;
                break;

            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if the text object has been set.
        if (text != null)
        {
            // sets the countdown start time if it has been changed.
            if (countdownStart != timer1.GetCountdownStartTime())
                timer1.SetCountdownStartTime(countdownStart);

            // the number of the timer that should be used.
            // now formatted with ToString("F2").
            switch (timerNumber)
            {
                case 1: // countdown
                    if(overrideTimerPause)
                        timer1.paused = paused;
                   
                    text.text = "Timer: " + timer1.GetCurrentCountdownTime().ToString("F2");
                    break;

                case 2: // stopwatch
                    if (overrideTimerPause)
                        timer2.paused = paused;

                    text.text = "Timer: " + timer2.GetCurrentStopwatchTime().ToString("F2");
                    break;

                default:
                    break;
            }
        }
    }
}
