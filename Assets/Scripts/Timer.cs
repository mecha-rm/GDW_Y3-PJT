using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //Init text for timer
    public Text timerText;

    public Text warningText;

    //Starting time
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        //This is temporary starting time, will be changed to whatever user sets it to.
        startTime = 20.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float t = startTime - Time.time;

        string seconds = t.ToString();

        timerText.text = seconds;
     
        //When timer hits 10 seconds, display warning text
        if (t <= 10.0f)
        {
            warningText.text = (int)(t) + " seconds left";
        }
    }
}