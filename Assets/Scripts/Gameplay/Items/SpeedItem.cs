using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : HeldItem
{
    // the base speed and the speed multiplayer
    private float baseSpeed = 1.0F;
    private float speedMult = 2.28F;
    GameObject itemIcon;
    string itemname = "speedup";

    // Start is called before the first frame update
    void Start()
    {
        GameObject parentObject = GameObject.Find("Item");
        int childCount = parentObject.transform.childCount;

        for (int index  = 0; index < childCount; index++)
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
        baseSpeed = activator.speedMult;
        activator.speedMult = activator.speedMult * speedMult;
    }

    // apply the effect to the player
    protected override void RemoveEffect()
    {
        // restores base speed.
        if (itemIcon != null)
        {
        itemIcon.SetActive(false);
        }

        activator.speedMult = baseSpeed;
    }

    // Update is called once per frame
    // void Update()
    // {
    // }
}
