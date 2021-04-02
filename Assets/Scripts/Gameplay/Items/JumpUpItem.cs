using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// jump item
public class JumpUpItem : HeldItem
{
    // Start is called before the first frame update
    // the base speed and the speed multiplayer
    private float baseJump = 1.0F;
    private float jumpMult = 2.0F;
    GameObject itemIcon;
    string itemname = "jumpup";

    // Start is called before the first frame update
    void Start()
    {
        GameObject parentObject = GameObject.Find("Item");
        int childCount = parentObject.transform.childCount;

        for (int index = 0; index < childCount; index++)
        {
            GameObject childObject = parentObject.transform.GetChild(index).gameObject;
            if (childObject.name == itemname)
            {
                childObject.SetActive(true);
                itemIcon = childObject;
            }
            else
            {
                childObject.SetActive(false);
            }
        }
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
        if (itemIcon != null)
        {
            itemIcon.SetActive(false);
        }

        // restores base speed.
        activator.jumpMult = baseJump;
    }

    // Update is called once per frame
    // void Update()
    // {
    // }
}
