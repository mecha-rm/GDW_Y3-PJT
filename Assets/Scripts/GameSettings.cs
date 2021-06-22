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

    // file name
    private const string FILE_NAME = "Assets/Resources/Saves/gamesettings.dat";
    
    // labels
    private const string LBL_MASTER_VOL = "MSRVOL";
    private const string LBL_BGM_VOL = "BGMVOL";
    private const string LBL_SFX_VOL = "SFXVOL";

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

    // loads the game settings
    public void LoadSettings()
    {
        MetricsLogger prefLog = new MetricsLogger();

        // sets the logger file
        prefLog.SetLoggerFile(FILE_NAME);
        prefLog.LoadMetrics();

        // get the contents
        float value;

        // master volume
        value = prefLog.GetMetricFromLogger(LBL_MASTER_VOL);
        SetMasterVolume(value);

        // bgm
        value = prefLog.GetMetricFromLogger(LBL_BGM_VOL);
        SetBgmVolume(value);

        // sound effects
        value = prefLog.GetMetricFromLogger(LBL_SFX_VOL);
        SetSfxVolume(value);

        // adjust all volume settings
        SettingsInterface.AdjustVolume();
    }

    // saves the game settings
    public void SaveSettings()
    {
        // uses the metric logger to save the settings
        MetricsLogger prefLog = new MetricsLogger();

        // adjusts the volume
        // TODO: maybe move this function or put it into this game settings file.
        SettingsInterface.AdjustVolume();

        // Saving Content to Fle
        // sets the logger file
        prefLog.SetLoggerFile(FILE_NAME);

        // TODO: add function to metrics to see if the file exists.
        // see what happens if the file isn't there.

        // adds the contents
        // master volume
        prefLog.AddMetricToLogger(LBL_MASTER_VOL, masterVolume);

        // bgm volume
        prefLog.AddMetricToLogger(LBL_BGM_VOL, bgmVolume);

        // sound effects
        prefLog.AddMetricToLogger(LBL_SFX_VOL, sfxVolume);

        // saves the data
        prefLog.SaveMetrics();
    }
}
