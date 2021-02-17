using System.Collections;
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
    bool timedItem = true;
    
    // the maximum effect time and the current effect time.
    protected float maxEffectTime = 10.0F;
    private float currEffectTime = 0.0F;

    // the player that activated the effect.
    protected PlayerObject activator = null;

    // Start is called before the first frame update
    void Start()
    {
        
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

    // activates the effect for the gameplay item
    public void ActivateEffect(PlayerObject player)
    {
        currEffectTime = maxEffectTime;
        activator = player;
        ApplyEffect();
    }

    // applies an effect for the item.
    // the activator is set before calling this function.
    protected abstract void ApplyEffect();

    // de-activates the effect.
    public void DeactiveEffect()
    {
        currEffectTime = 0.0F;
        RemoveEffect();
        activator = null;
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
                DeactiveEffect();
            }
        }
    }
}
