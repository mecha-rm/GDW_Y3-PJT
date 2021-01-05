using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// menu audio 
public class MenuAudio : MonoBehaviour
{
    // audio sources
    public AudioSource sfx_ButtonClick = null;
    public AudioSource sfx_ButtonEnter = null;

    // TODO: make sound for clicking nothing?

    // Start is called before the first frame update
    void Start()
    {
        // button click is null
        if(sfx_ButtonClick == null)
        {
            sfx_ButtonClick = (AudioSource)Resources.Load("Audio/SFXs/SFX_BUTTON_CLICK_SHORT");
        }

        // button enter is null
        if(sfx_ButtonEnter == null)
        {
            sfx_ButtonEnter = (AudioSource)Resources.Load("Audio/SFXs/SFX_BUTTON_CLICK");
        }
    }

    // plays the button click sound effect
    public void PlayButtonClick()
    {
        sfx_ButtonClick.Play();
    }

    // plays the button click sound effect
    public void PlayButtonEnter()
    {
        sfx_ButtonEnter.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
