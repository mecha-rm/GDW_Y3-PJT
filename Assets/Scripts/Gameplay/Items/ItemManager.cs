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

                // item was set to null.
                if (item == null)
                    continue;

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

    // creates a field item
    public FieldItem CreateItem()
    {
        // sets item values
        GameObject itemBox = GameObject.Instantiate((GameObject)Resources.Load(itemPrefab));
        FieldItem fieldItem = itemBox.GetComponent<FieldItem>();
        fieldItem.ResetDespawnCountdown();
        fieldItem.RandomizeItem();

        // returns the new field item.
        return fieldItem;
    }

    // gets a field item
    public FieldItem GetItem()
    {
        // generates new item
        if(itemPool.Count == 0)
        {
            // creates the item.
            return CreateItem();
        }
        else // takes item from the pool
        {
            // original
            // FieldItem itemBox = itemPool.Dequeue();
            // itemBox.ResetDespawnCountdown();
            // itemBox.gameObject.SetActive(true);
            // itemBox.RandomizeItem(); // randomize item

            // new
            FieldItem itemBox = null;

            // checks for null values
            do
            {
                // gets item
                itemBox = itemPool.Dequeue();

                // if the item box was null, go through the list again.
                if (itemBox == null)
                    continue;

                // sets values if it isn't null.
                itemBox.ResetDespawnCountdown();
                itemBox.gameObject.SetActive(true);
                itemBox.RandomizeItem(); // randomize item
            }
            while (itemBox == null && itemPool.Count != 0); // while a preset item hasn't been found.

            // no items were found in the list.
            if (itemBox == null)
                itemBox = CreateItem();

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

    // clears out all items in the pool.
    public void ClearAllItemsInPool()
    {
        itemPool.Clear();
    }

    // destroy all items in the pool.
    public void DestroyAllItemsInPool()
    {
        while(itemPool.Count != 0)
        {
            FieldItem item = itemPool.Dequeue(); // removes item

            // destroys the item.
            if (item != null)
                Object.Destroy(item.gameObject);
        }

        itemPool.Clear();
    }
}
