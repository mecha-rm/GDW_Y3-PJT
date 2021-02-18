using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

// a manager for playing a video.
public class VideoPlayerController : MonoBehaviour
{
    // TODO: the video player controller doesn't really have many features.
    // this probably won't be needed for the final game, but it's here for later just in case.


    // the video
    public VideoPlayer video;

    // Start is called before the first frame update
    void Start()
    {
        // if the video has not been set, it searches for the component.
        if (video == null)
            video = GetComponent<VideoPlayer>();
    }

    // plays the video
    public void Play()
    {
        video.Play();
    }

    // checks to see if the video is paused.
    public bool IsPaused()
    {
        return video.isPaused;
    }

    // pauses the video
    public void Pause()
    {
        video.Pause();
    }

    // stops the video
    public void Stop()
    {
        video.Stop();
    }

    // skip to the end of the video.
    public void Skip()
    {
        video.time = video.length;
    }

    // public void FastForward()
    // {
    //     video.playbackSpeed = 2.0F;
    // }

    // public void Rewind(float time)
    // {
    //     video.time -= time;
    // }

    // Update is called once per frame
    void Update()
    {
        
    }
}
