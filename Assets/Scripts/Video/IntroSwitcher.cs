using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

// intro switcher
public class IntroSwitcher : MonoBehaviour
{
    // the builder for the game, which needs to have its build function disabled.
    public GameBuilder builder;

    // the title object. It will only countdown
    public GameObject titleObject;

    // video scene
    public const string VIDEO_SCENE = "VideoPlayer";

    // enables countdown to happen.
    public bool enableTimer = true;

    // if 'true', it only counts down when the title is active.
    public bool countOnTitleActive = true;

    // wait time (in seconds)
    public float waitTime = 30.0F;

    // the current time
    public float currTime = 0.0F;


    // Start is called before the first frame update
    void Start()
    {
        // finds the game builder.
        if (builder == null)
            builder = FindObjectOfType<GameBuilder>();

        // finds title object.
        if (titleObject == null)
            titleObject = GameObject.Find("Title");

    }

    // plays the intro of the game.
    public void PlayIntro()
    {
        // disables the build functions
        builder.loadGame = false;
        SceneManager.LoadScene(VIDEO_SCENE);
    }

    // Update is called once per frame
    void Update()
    {
        // timer is enabled.
        if (enableTimer)
        {
            // first checks to see if the title object is set.
            bool titleSet = titleObject != null;

            // if the title has been set, now check for 'countOnTitleActive' to see if that's true.
            if (titleSet)
                titleSet = titleObject.activeSelf; // checks to see if the object is active.

            // increments timer
            if(!countOnTitleActive || (countOnTitleActive && titleSet))
            {
                currTime += Time.deltaTime;
                currTime = Mathf.Clamp(currTime, 0, waitTime);
            }
        }        

        // intro should be played.
        if(currTime >= waitTime)
        {
            PlayIntro();
        }
    }
}
