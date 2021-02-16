using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPlayer : PlayerObject
{
    // Start is called before the first frame update

    void Start()
    {
        base.Start();

        // replacing sounds
        {
            // TODO: randomize sound for cat meow
            // Destroy(sfx_Idle.clip); // regular destroy didn't work.
            DestroyImmediate(sfx_Idle.clip, true);
            sfx_Idle.clip = (AudioClip)Resources.Load("Audio/Cat/SFX_CAT_MEOW_01");

            // Destroy(sfx_Death.clip);
            DestroyImmediate(sfx_Death.clip, true);
            sfx_Death.clip = (AudioClip)Resources.Load("Audio/Cat/SFX_CAT_SCREAM");
        }

        speedMult = 1.5F;
        knockbackMult = 1.0F;
        jumpMult = 1.0F;
        defenseMult = 1.0F;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
