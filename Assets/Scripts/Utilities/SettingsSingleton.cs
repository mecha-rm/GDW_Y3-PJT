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
}
