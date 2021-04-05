using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// speed up
public class SpeedUpItem : HeldItem
{
    // the base speed and the speed multiplayer
    private float baseSpeed = 1.0F;
    private float speedMult = 2.28F;

    // Start is called before the first frame update
    void Start()
    {
        itemIconName = "speedup";

        // activates item icon
        ActivateItemIcon();
    }

    // apply the effect to the game object
    protected override void ApplyEffect()
    {
        baseSpeed = activator.speedMult;
        activator.speedMult = activator.speedMult * speedMult;
    }

    // apply the effect to the player
    protected override void RemoveEffect()
    {
        // deactivates icon.
        DeactivateItemIcon();

        activator.speedMult = baseSpeed;
    }

    // Update is called once per frame
    // void Update()
    // {
    // }
}
