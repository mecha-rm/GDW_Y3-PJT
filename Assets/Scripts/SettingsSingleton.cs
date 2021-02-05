using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// setting singleton for mass game options
public class SettingsSingleton
{
    // instance of singleton
    private static SettingsSingleton instance = null;

    // volume control
    // master volume
    private float masterVolume = 1.0F;
    private float bgmVolume = 1.0F;
    private float sfxVolume = 1.0F;

    // constructor
    private SettingsSingleton()
    {
        Start();
    }

    // gets the instance
    public static SettingsSingleton GetInstance()
    {
        // no instance generated
        if (instance == null) 
        {
            // generates instance
            instance = new SettingsSingleton(); 
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

    // game bgm volume getter and setter
    public float BgmVolume
    {
        get { return bgmVolume; }
        set 
        {
            bgmVolume = Mathf.Clamp(value, 0.0F, 1.0F);
        }
    }

    // game sound effect volume getter and setter
    public float SfxVolume
    {
        get { return sfxVolume; }
        set 
        {
            sfxVolume = Mathf.Clamp(value, 0.0F, 1.0F);
        }
    }
}
