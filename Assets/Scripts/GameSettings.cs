using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: rename this
// setting singleton for mass game options
public class GameSettings
{
    // instance of singleton
    private static GameSettings instance = null;

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
