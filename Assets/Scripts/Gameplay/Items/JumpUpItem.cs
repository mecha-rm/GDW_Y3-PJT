﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// jump up item
public class JumpUpItem : HeldItem
{
    // Start is called before the first frame update
    // the base speed and the speed multiplayer
    private float baseJump = 1.0F;
    private float jumpMult = 2.0F;

    // Start is called before the first frame update
    void Start()
    {
        itemIconName = "jumpup";

        // activates item icon
        ActivateItemIcon();
    }

    // apply the effect to the game object
    protected override void ApplyEffect()
    {
        baseJump = activator.jumpMult;
        activator.jumpMult = activator.speedMult * jumpMult;
    }

    // apply the effect to the player
    protected override void RemoveEffect()
    {
        // deactivates icon.
        DeactivateItemIcon();

        // restores base speed.
        activator.jumpMult = baseJump;
    }

    // Update is called once per frame
    // void Update()
    // {
    // }
}
