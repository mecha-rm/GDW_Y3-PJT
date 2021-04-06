using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: rename this
// setting singleton for mass game options
public class GameSettings
{
    // instance of singleton
    private static GameSettings instance = null;

    // the saved name of the system.
    private string screenName = "";

    // the maximum name length
    public const int SCREEN_NAME_MAX_LEN = 16;

    // volume control
    // master volume
    private float masterVolume = 1.0F;
    private float bgmVolume = 1.0F;
    private float sfxVolume = 1.0F;

    // constructor
    private GameSettings()
    {
        Start();
    }

    // gets the instance
    public static GameSettings GetInstance()
    {
        // no instance generated
        if (instance == null) 
        {
            // generates instance
            instance = new GameSettings(); 
        }

        return instance;
    }

    // start function
    void Start()
    {
        // the saved name of the player
        if(screenName == "")
        {
            // ranomizes name
            for (int i = 1; i <= SCREEN_NAME_MAX_LEN; i++)
                screenName += (Random.Range(0, 10)).ToString();
        }

    }

    // gets the screen name.
    public string GetScreenName()
    {
        return screenName;
    }

    // sets the screen name
    public void SetScreenName(string str)
    {
        // empty string passed.
        if (str.Length == 0 || str.Replace(" ", "").Length == 0)
            return;

        // saves new string
        int len = Mathf.Min(str.Length, SCREEN_NAME_MAX_LEN);
        screenName = str.Substring(0, len);
    }


    // updates volume levels for all audio
    public void UpdateVolume()
    {
        AudioSource[] audios = GameObject.FindObjectsOfType<AudioSource>();

        // update volume levels for all audio
        foreach(AudioSource audio in audios)
        {
            if(audio.tag == "BGM") // background music
            {
                audio.volume = bgmVolume;
            }
            else if(audio.tag == "SFX") // sound effects
            {
                audio.volume = sfxVolume;
            }
        }
    }

    // gets the volume
    public float GetMasterVolume()
    {
        return masterVolume;
    }

    // sets the volume
    public void SetMasterVolume(float vol)
    {
        masterVolume = Mathf.Clamp(vol, 0.0F, 1.0F);
        AudioListener.volume = masterVolume;
    }

    // returns the bgm volume.
    public float GetBgmVolume()
    {
        return bgmVolume;
    }

    // sets the bgm volume
    public void SetBgmVolume(float vol)
    {
        bgmVolume = Mathf.Clamp(vol, 0.0F, 1.0F);
    }

    // returns the sfx volume.
    public float GetSfxVolume()
    {
        return sfxVolume;
    }

    // sets the sfx volume
    public void SetSfxVolume(float vol)
    {
        sfxVolume = Mathf.Clamp(vol, 0.0F, 1.0F);
    }
}
