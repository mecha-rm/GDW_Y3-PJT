using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for items that are on the field and available for pickup
public class FieldItem : MonoBehaviour
{
    // enum for available items
    public enum itemType
    {
        none, speedUp, jumpUp
    }

    // number of items available
    private const int ITEM_COUNT = 2;

    // the number of the item
    public itemType itemSet = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // gets the total amount of items
    public int GetTotalItemAmount()
    {
        return ITEM_COUNT;
    }

    // randomizes the item number of the 
    public itemType RandomizeItem()
    {
        // randomizes the item
        itemSet = (itemType)Random.Range(1, ITEM_COUNT + 1);

        return itemSet;
    }

    // adds the item component to the player
    public HeldItem AddItemComponent(PlayerObject player)
    {
        HeldItem genItem = null;

        switch(itemSet)
        {
            case 0: // none
                break;
            case itemType.speedUp: // speed
                genItem = player.gameObject.AddComponent<SpeedItem>();
                break;
            case itemType.jumpUp: // jump
                genItem = player.gameObject.AddComponent<JumpItem>();
                break;
        }

        // activates the effect.
        if (genItem != null)
            genItem.ActivateEffect(player);

        return genItem;
    }

    // the item box has collided with something.
    private void OnCollisionEnter(Collision collision)
    {
        // gets the player object
        PlayerObject plyr = collision.gameObject.GetComponent<PlayerObject>();

        // if it wasn't a player, nothing should happen.
        if (plyr == null)
            return;

        // adds the item component.
        AddItemComponent(plyr);

        // deactivates object
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
