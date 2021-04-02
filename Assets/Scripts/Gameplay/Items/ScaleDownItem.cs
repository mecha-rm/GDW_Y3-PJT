using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleDownItem : HeldItem
{
    // the base speed and the speed multiplayer
    private Vector3 baseScale;
    private Vector3 newScale = new Vector3(0.6F, 0.6F, 0.6F);
    GameObject itemIcon;
    string itemname = "scaledown";

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
        baseScale = activator.transform.localScale;
        activator.transform.localScale = newScale;
    }

    // apply the effect to the player
    protected override void RemoveEffect()
    {
        if (itemIcon != null)
        {
            itemIcon.SetActive(false);
        }
        // restores base speed.
        activator.transform.localScale = baseScale;
        activator.transform.position += new Vector3(0.0F, 5.0F, 0.0F); // TODO: this probably shouldn't be hardcoded.
    }

    // Update is called once per frame
    // void Update()
    // {
    // }
}
