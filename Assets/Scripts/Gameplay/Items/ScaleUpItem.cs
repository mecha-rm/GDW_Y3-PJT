using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// makes user larger.
public class ScaleUpItem : HeldItem
{
    // the base speed and the speed multiplayer
    // dying turns this power off for some reason.
    private Vector3 baseScale;
    private Vector3 newScale = new Vector3(2.5F, 2.5F, 2.5F);

    // Start is called before the first frame update
    void Start()
    {
        itemIconName = "scaleup";

        // activates item icon
        ActivateItemIcon();
    }

    // apply the effect to the game object
    protected override void ApplyEffect()
    {
        baseScale = activator.transform.localScale;
        activator.transform.position += new Vector3(0.0F, 2.5F, 0.0F); // TODO: this probably shouldn't be hardcoded.
        activator.transform.localScale = newScale;
    }

    // apply the effect to the player
    protected override void RemoveEffect()
    {
        // deactivates icon.
        DeactivateItemIcon();


        // restores base speed.
        activator.transform.localScale = baseScale;
    }

    // Update is called once per frame
    // void Update()
    // {
    // }
}
