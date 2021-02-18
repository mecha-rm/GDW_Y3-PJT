using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for settings
// interfaces with the SettingsSingleton for the menu.
public class SettingsInterface : MonoBehaviour
{
    GameSettings settings;

    // Start is called before the first frame update
    void Start()
    {
        settings = GameSettings.GetInstance();
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
    public void AdjustVolume()
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

    // Update is called once per frame
    void Update()
    {
    }
}
