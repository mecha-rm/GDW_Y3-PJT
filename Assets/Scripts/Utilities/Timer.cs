using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// timer - uses DLL.
public class Timer : MonoBehaviour
{
    // timer text
    public Text text;

    // timer
    float timer = 0.0F;

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
        timer += Time.deltaTime;

        // if there is a text object
        if (text != null)
        {
            // // gets the minutes and seconds
            // int min = Mathf.RoundToInt(timer % 6000.0F);
            // int sec = Mathf.RoundToInt(timer - (timer % 6000.0F));
            // 
            // text.text = "Time: " + min + ":" + sec;

            text.text = "Time: " + timer;
        }


    }
}
