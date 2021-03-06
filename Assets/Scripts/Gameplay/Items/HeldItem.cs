﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: have the items be random drops that appear on the field.
// script for gameplay items
// NOTE: this is for items that are being held by a player.
public abstract class HeldItem : MonoBehaviour
{
    // determines whether the item's effect is timed or not.
    // if 'true', the item's effect runs out after (X) amount of time has passed.
    // if 'false', the item must be manually turned off by calling DeactivateEffect()
    protected bool timedItem = true;
    
    // the maximum effect time and the current effect time.
    public float maxEffectTime = 10.0F;
    public float currEffectTime = 0.0F;

    // the player that activated the effect.
    protected PlayerObject activator = null;

    // item icon to be activated upon being applied.
    protected GameObject itemIcon = null;

    // item icon name
    protected string itemIconName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // checks to see if the item is timed.
    public bool IsTimedItem()
    {
        return timedItem;
    }

    // returns the maximum effect time.
    public float GetMaximumEffectTime()
    {
        return maxEffectTime;
    }

    // sets the maximum effect time. It cannot be less than 0.
    // an effect time of 0 means the time won't get triggered.
    protected void SetMaximumEffectTime(float maxTime)
    {
        maxEffectTime = (maxTime >= 0.0F) ? maxTime : maxEffectTime;
    }

    // resets the countdown until the item times out.
    public void ResetCountdown()
    {
        currEffectTime = maxEffectTime;
    }

    // activates the effect for the gameplay item
    public void ActivateEffect(PlayerObject player)
    {
        currEffectTime = maxEffectTime;
        activator = player;
        ApplyEffect();
    }

    // actiates item icon with name
    public void ActivateItemIcon(string iconName)
    {
        itemIconName = iconName;
        ActivateItemIcon();
    }

    // enables item icon
    public void ActivateItemIcon()
    {
        // finds parent object.
        // TODO: this should save the parent to be optimized.
        GameObject parentObject = GameObject.Find("Item");
        int childCount = parentObject.transform.childCount;

        // turns off all children.
        for (int index = 0; index < childCount; index++)
        {
            // child object
            GameObject childObject = parentObject.transform.GetChild(index).gameObject;
            

            // checks for proper item
            if (childObject.name == itemIconName)
            {
                childObject.SetActive(true);
                itemIcon = childObject;
            }
            else
            {
                // if it's not the item graphic, hide it.
                if(childObject.name != "Item Graphic")
                    childObject.SetActive(false);
            }
        }
    }

    // disables item icon
    public void DeactivateItemIcon()
    {
        // deactivates item icon
        if (itemIcon != null)
        {
            itemIcon.SetActive(false);
        }
        else
        {
            Debug.LogAssertion("No item icon set.");
            
        }
    }


    // applies an effect for the item.
    // the activator is set before calling this function.
    protected abstract void ApplyEffect();

    // de-activates the effect.
    public void DeactiveEffect()
    {
        currEffectTime = 0.0F;
        RemoveEffect();

        // removes this component from the player and destroys itself.
        Destroy(this);

        // activator = null; // remove player
        // enabled = false; // de-activate item
    }

    // function for removing the effect.
    // the activator is removed after this function is called.
    protected abstract void RemoveEffect();

    // Update is called once per frame
    protected void Update()
    {
        // timed item
        if (timedItem)
        {
            if (currEffectTime > 0.0F)
                currEffectTime -= Time.deltaTime;

            // if the effect time has run out.
            if (currEffectTime <= 0.0F)
            {
                // deactivates the effect, and destroies this object.
                DeactiveEffect();
            }
        }
    }
}
