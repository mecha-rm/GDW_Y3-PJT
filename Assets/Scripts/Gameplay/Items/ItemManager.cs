using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the item manager, which is a singleton
public class ItemManager
{
    // instance of singleton
    private static ItemManager instance = null;

    // list of items in the pool
    List<FieldItem> items = new List<FieldItem>();

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

    // sets the total amount of items that are available.
    public void SetItemCount(int itemCount)
    {
        // there are more items that should be available
        if(items.Count > itemCount)
        {
            do
            {
                // removes the item from the list
                FieldItem item = items[items.Count - 1];
                items.RemoveAt(items.Count - 1);
                Object.Destroy(item);
            }
            while (items.Count > itemCount);
        }
        // there are less items than should be available
        else if(items.Count < itemCount)
        {
            do
            {
                // generates an item and adds it to the list.
                // TODO: check for component being on prefab?
                GameObject itemBox = GameObject.Instantiate((GameObject)Resources.Load(itemPrefab));
                FieldItem fieldItem = itemBox.GetComponent<FieldItem>();

                // missing component
                if (fieldItem == null)
                    fieldItem = itemBox.AddComponent<FieldItem>();

                fieldItem.RandomizeItem();
                items.Add(fieldItem);
            }
            while (items.Count < itemCount);
        }

        for(int i = 0; i < itemCount; i++)
        {

        }
    }
}
