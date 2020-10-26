using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : PlayerObject
{
    // Start is called before the first frame update
    void Start()
    {
        speedMult = 1.5F;
        knockbackMult = 1.0F;
        jumpMult = 1.0F;
        defenseMult = 1.0F;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
