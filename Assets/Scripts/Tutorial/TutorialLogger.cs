using UnityEngine;

// logs tutorial information
// this is just to seperate the tutorial from the main content.
public class TutorialLogger : MetricsLogger
{
    // gets the time from the timer text

    // comment this out if you don't want to view previously stored values
    // Start is called before the first frame update
    void Start()
    {
        // if there is a file to set.
        if (file != "")
        {
            SetLoggerFile(file);
        }
        // if the contents should be loaded form the file.
        if (loadFromFile)
        {
            LoadMetrics();
            int count = GetNumberOfMetrics();
            string loggerFile = GetLoggerFile();

            Debug.Log("Existing Metrics (from " + loggerFile + ")");

            for (int i = 0; i < count; i++)
            {
                // this function call can't be put into the debug log print directly.
                string key = GetKeyFromLoggerAtIndex(i);

                // this can be put into the debug log print, but I did it this way for consistency.
                float val = GetValueFromLoggerAtIndex(i);

                Debug.Log(key + ": " + val);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
