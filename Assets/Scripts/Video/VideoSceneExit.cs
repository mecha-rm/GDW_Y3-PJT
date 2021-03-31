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

    // in order for the video scene exit to work properly, the script must wait a few update cycles.
    // if this doesn't load in time, the scene will exit without the video playing.
    // as such, this has been changed to be based off of delta time (in econds).
    public float waitTime = 5.0F;
    
    // the alloted itme that has passed.
    public float allottedTime = 0.0F;

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
        // programmed to exit the scene once the video fnishes playing.
        // however, a delay is used in order to give time for the video to actually start.
        // without waiting for the video to start, it would leave immediately.
        if(allottedTime >= waitTime)
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
            // increase alloted time.
            allottedTime += Time.deltaTime;
        }

    }
}
