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
    private const int ITEM_COUNT = 3;

    // the number of the item
    private int itemNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // randomizes the item number of the 
    public itemType RandomizeItem()
    {
        // randomizes the item
        itemNumber = Random.Range(1, ITEM_COUNT + 1);

        return (itemType)itemNumber;
    }    

    // Update is called once per frame
    void Update()
    {
        
    }
}
