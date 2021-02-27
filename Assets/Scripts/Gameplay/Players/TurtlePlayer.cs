using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtlePlayer : PlayerObject
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        speedMult = 1.0F;
        knockbackMult = 1.0F;
        jumpMult = 1.0F;
        defenseMult = 1.5F;

        // replacing sounds

        // idle sound
        if(sfx_Idle.clip == null)
        {
            // Destroy(sfx_Idle.clip);
            sfx_Idle.clip = (AudioClip)Resources.Load("Audio/Cat/SFX_TURTLE_NOISES_01");
        }

        // death sound
        if (sfx_Death.clip == null)
        {
            // Destroy(sfx_Idle.clip);
            sfx_Death.clip = (AudioClip)Resources.Load("Audio/Cat/SFX_TURTLE_DEATH");
        }
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
