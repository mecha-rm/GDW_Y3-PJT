using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// script for settings
// interfaces with the SettingsSingleton for the menu.
public class SettingsInterface : MonoBehaviour
{
    // settings object (only one instance exists)
    GameSettings settings;

    // if 'true', the volume is adjusted at the start.
    public bool adjustSettingsOnStart = true;

    // sliders
    public Slider msrVolSlider = null;
    public Slider bgmVolSlider = null;
    public Slider sfxVolSlider = null;

    // grabs the instance from the start.
    private void Awake()
    {
        // grab instance
        settings = GameSettings.GetInstance();

        // find master volume
        if(msrVolSlider == null)
        {
            GameObject go = GameObject.Find("Master Volume Slider");
            if (go != null)
                msrVolSlider = go.GetComponent<Slider>();
        }

        // find bgm volume
        if (bgmVolSlider == null)
        {
            GameObject go = GameObject.Find("BGM Volume Slider");
            if (go != null)
                bgmVolSlider = go.GetComponent<Slider>();
        }

        // find master volume
        if (sfxVolSlider == null)
        {
            GameObject go = GameObject.Find("SFX Volume Slider");
            if (go != null)
                sfxVolSlider = go.GetComponent<Slider>();
        }
        

        // setting the default values
        if (msrVolSlider != null) // master
            msrVolSlider.value = settings.GetMasterVolume();

        if (bgmVolSlider != null) // bgm
            bgmVolSlider.value = settings.GetBgmVolume();

        if (sfxVolSlider != null) // sfx
            sfxVolSlider.value = settings.GetSfxVolume();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(adjustSettingsOnStart)
            settings.LoadSettings(); // loads the settings from the file.
    }

    // gets the volume
    public float GetMasterVolume()
    {
        return settings.GetMasterVolume();
    }

    // sets the volume
    public void SetMasterVolume(float vol)
    {
        settings.SetMasterVolume(vol);
    }

    // returns the bgm volume.
    public float GetBgmVolume()
    {
        return settings.GetBgmVolume();
    }

    // sets the bgm volume
    public void SetBgmVolume(float vol)
    {
        settings.SetBgmVolume(vol);
    }

    // returns the sfx volume.
    public float GetSfxVolume()
    {
        return settings.GetSfxVolume();
    }

    // sets the sfx volume
    public void SetSfxVolume(float vol)
    {
        settings.SetSfxVolume(vol);
    }

    // updates the volume with new settings
    public static void AdjustVolume()
    {
        // finds all audio sources
        AudioSource[] audios = FindObjectsOfType<AudioSource>();

        // gets the game settings
        GameSettings settings = GameSettings.GetInstance();

        // gets the volume for the bgm and the sfx.
        // the master volume does not need to be set since that's done by Unity.
        float bgmVolume = settings.GetBgmVolume();
        float sfxVolume = settings.GetSfxVolume();

        // adjusts the volume settings.
        foreach (AudioSource audio in audios)
        {
            // adjusts volume
            switch (audio.tag)
            {
                case "BGM":
                    audio.volume = bgmVolume;
                    break;

                case "SFX":
                    audio.volume = sfxVolume;
                    break;
            }
        }
    }

    // Load Settings
    public void LoadSettings()
    {
        // loads the settings from the file.
        settings.LoadSettings();
    }

    // Save Settings
    public void SaveSettings()
    {
        // saves the settings to the file.
        settings.SaveSettings();
    }
}
