using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPlayer : PlayerObject
{
    // Start is called before the first frame update

    private AudioSource source;

    void Start()
    {
        base.Start();

        source = GetComponent<AudioSource>();

        speedMult = 1.5F;
        knockbackMult = 1.0F;
        jumpMult = 1.0F;
        defenseMult = 1.0F;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        source.Play();
    }
}
