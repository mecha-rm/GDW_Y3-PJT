using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spawns items
public class ItemSpawner : MonoBehaviour
{
    // the item manager
    private ItemManager itemManager;

    // spawns item into the scene.
    public int itemCount = 10; // the 10 items

    // if an item box reaches the death space, it should despawn
    public DeathSpace deathSpace;

    // Start is called before the first frame update
    void Start()
    {
        // gets the item manager instance
        itemManager = ItemManager.GetInstance();
        itemManager.SetItemCount(itemCount);
    }

    // gets the item count
    public int GetItemCount()
    {
        // the item count is matched up with the manager.
        itemCount = itemManager.GetItemCount();
        return itemCount;
    }

    // sets the new item count
    public void SetItemCount(int newCount)
    {
        itemManager.SetItemCount(newCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
