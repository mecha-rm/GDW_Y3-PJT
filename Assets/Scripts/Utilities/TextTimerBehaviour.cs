using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// timer - uses DLL.
public class TextTimerBehaviour : MonoBehaviour
{
    // timer text
    public Text text;

    // start and end of text string.
    public string strStart = "Time: ";
    public string strEnd = "";

    // timer
    float timer = 0.0F;

    // speed of the timer
    // a negative number will make it go down, a positive number will make it go up.
    float speed = 1.0F;

    // Start is called before the first frame update
    void Start()
    {
        if (text == null)
            text = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // adds to timer
        timer += Time.deltaTime * speed;

        // if there is a text object
        if (text != null)
        {
            // // gets the minutes and seconds
            // int min = Mathf.RoundToInt(timer % 6000.0F);
            // int sec = Mathf.RoundToInt(timer - (timer % 6000.0F));
            // 
            // text.text = "Time: " + min + ":" + sec;

            // now formatted.
            text.text = strStart + timer.ToString("#") + strEnd;
        }


    }
}
