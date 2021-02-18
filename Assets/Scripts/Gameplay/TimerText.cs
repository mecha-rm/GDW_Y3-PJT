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

    // Start is called before the first frame update
    void Start()
    {
        // initializes timers if they don't exist.
        if(text == null)
        {
            // if no text object has been set, it gets the text component.
            text = GetComponent<Text>();
        }


        if (timer1 == null)
        {
            // searches for countdown timer attached to object.
            timer1 = GetComponent<CountdownTimer>();

            // if no timer is attached, a new one is made.
            if (timer1 == null)
            {
                timer1 = gameObject.AddComponent(typeof(CountdownTimer)) as CountdownTimer;
            }
        }

        if (timer2 == null)
        {
            // searches for a stopwatch timer attached to the object.
            timer2 = GetComponent<StopwatchTimer>();

            // if no timer is attached, a new one is made.
            if (timer2 == null)
            {
                timer2 = gameObject.AddComponent(typeof(StopwatchTimer)) as StopwatchTimer;
            }
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
            switch (timerNumber)
            {
                case 1: // countdown
                    timer1.paused = paused;
                    text.text = "Timer: " + timer1.GetCurrentCountdownTime().ToString();
                    break;

                case 2: // stopwatch
                    timer2.paused = paused;
                    text.text = "Timer: " + timer2.GetCurrentStopwatchTime().ToString();
                    break;

                default:
                    break;
            }
        }
    }
}
