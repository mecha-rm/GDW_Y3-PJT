using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the item manager, which is a singleton
public class ItemManager
{
    /// <summary>
    /// Basic Setup:
    /// Item boxes save what kind of item components to generate
    /// When a player hits a box, a component is generated, and it attaches to the player
    /// The component is also put into the item manager, where it gets managed.
    /// </summary>

    // instance of singleton
    private static ItemManager instance = null;

    // list of items in the pool
    Queue<FieldItem> itemPool = new Queue<FieldItem>();

    // the prefab for the item box
    public string itemPrefab = "Prefabs/Item Box";

    // constructor
    private ItemManager()
    {
        Start();
    }

    // gets the instance
    public static ItemManager GetInstance()
    {
        // no instance generated
        if (instance == null)
        {
            // generates instance
            instance = new ItemManager();
        }

        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // gets the currenet amount of items in the pool.
    public int GetItemPoolCount()
    {
        return itemPool.Count;
    }

    // sets the total amount of items that are available.
    public void SetItemCount(int itemCount)
    {
        // there are more items that should be available
        if(itemPool.Count > itemCount)
        {
            do
            {
                // removes the item from the list
                FieldItem item = itemPool.Dequeue();
                Object.Destroy(item.gameObject);
            }
            while (itemPool.Count > itemCount);
        }
        // there are less items than should be available
        else if(itemPool.Count < itemCount)
        {
            do
            {
                // generates an item and adds it to the list.
                // TODO: check for component being on prefab?
                GameObject itemBox = GameObject.Instantiate((GameObject)Resources.Load(itemPrefab));
                FieldItem fieldItem = itemBox.GetComponent<FieldItem>();
                itemBox.SetActive(false);

                // missing component
                if (fieldItem == null)
                    fieldItem = itemBox.AddComponent<FieldItem>();

                fieldItem.RandomizeItem();
                itemPool.Enqueue(fieldItem);
            }
            while (itemPool.Count < itemCount);
        }
    }

    // checs to see if the item manageris empty.
    public bool ItemPoolIsEmpty()
    {
        return itemPool.Count == 0;
    }

    // gets a field item
    public FieldItem GetItem()
    {
        // generates new item
        if(itemPool.Count == 0)
        {
            GameObject itemBox = GameObject.Instantiate((GameObject)Resources.Load(itemPrefab));
            FieldItem fieldItem = itemBox.GetComponent<FieldItem>();
            fieldItem.ResetDespawnCountdown();
            fieldItem.RandomizeItem();

            // returns the new filed item.
            return fieldItem;
        }
        else // takes item from the pool
        {
            FieldItem itemBox = itemPool.Dequeue();
            itemBox.ResetDespawnCountdown();
            itemBox.gameObject.SetActive(true);
            itemBox.RandomizeItem(); // randomize item

            // returns item box.
            return itemBox;
        }
    }

    // returns the field item
    public void ReturnItem(FieldItem item)
    {
        // item.ResetDespawnCountdown(); // happens upon being pulled from pool.
        item.gameObject.SetActive(false);
        itemPool.Enqueue(item); // adds item back into pool.
    }
}
