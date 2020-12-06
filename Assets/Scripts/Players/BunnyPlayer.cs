using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyPlayer : PlayerObject
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        speedMult = 1.0F;
        knockbackMult = 1.0F;
        jumpMult = 1.5F;
        defenseMult = 1.0F;

        // replacing sounds
        {
            Destroy(sfx_Idle.clip);
            sfx_Idle.clip = (AudioClip)Resources.Load("Audio/Cat/SFX_BUNNY_NOISES_01");
        }
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
