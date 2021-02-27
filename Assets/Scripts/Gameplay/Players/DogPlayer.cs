using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPlayer : PlayerObject
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        speedMult = 1.0F;
        knockbackMult = 1.5F;
        jumpMult = 1.0F;
        defenseMult = 1.0F;

        // death sound
        if (sfx_Death.clip == null)
        {
            // Destroy(sfx_Idle.clip);
            sfx_Death.clip = (AudioClip)Resources.Load("Audio/Cat/SFX_DOG_DEATH");
        }
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
