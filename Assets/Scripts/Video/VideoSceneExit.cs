using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

// exits the video scene.
public class VideoSceneExit : MonoBehaviour
{
    // the controller for the video
    public VideoPlayer vp;

    // leads to the next scene.
    public string nextScene = "";

    // in order for the video scene exit to work properly, the script must wait one update cycle.
    private int waitCycles = 5;

    // Start is called before the first frame update
    void Start()
    {
        // searches for video player if it hasn't been set.
        if (vp == null)
            vp = GetComponent<VideoPlayer>();
    }
    

    // Update is called once per frame
    void Update()
    {
        // waits for at least one cycle.
        // this is done because it is programed to finish once the video finishes.
        // however, without waiting for the video to start, it would leave automatically.
        if(waitCycles <= 0)
        {
            // Debug.Log("Skip Available");

            // if any key has been pressed, the intro skips.
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(nextScene);
            }

            // if the video clip has stopped, leave the scene.
            if (!vp.isPlaying)
            {
                SceneManager.LoadScene(nextScene);
            }
        }
        else
        {
            // reduces wait cycle countdown
            waitCycles--;

            // below 0
            if (waitCycles < 0)
                waitCycles = 0;
        }

    }
}
