using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for settings
// interfaces with the SettingsSingleton for the menu.
public class SettingsInterface : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // gets the volume
    public float GetMasterVolume()
    {
        return SettingsSingleton.GetInstance().GetMasterVolume();
    }

    // sets the volume
    public void SetMasterVolume(float vol)
    {
        SettingsSingleton.GetInstance().SetMasterVolume(vol);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
