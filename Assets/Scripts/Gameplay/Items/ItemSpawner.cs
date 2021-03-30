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

    // if 'true', the spawner operates.
    // use this to disable the spawner on other systems.
    public bool spawnerEnabled = true;

    // spawn time (in seconds), and countdown to when to spawn another object.
    public float spawnTime = 20.0F;
    public float spawnCountdown = 0.0F;

    // the minimum and maximum of the spawn area
    public Vector3 spawnAreaMin = new Vector3(-50.0F, -50.0F, -50.0F);
    public Vector3 spawnAreaMax = new Vector3(50.0F, 50.0F, 50.0F);

    // if 'true', items will automatically despawn after a certain period of time.
    // note that changing this mid-game will have no effect on the items already available.
    public bool autoItemDespawn = true;

    // if an item box reaches the death space, it should despawn
    public DeathSpace deathSpace;

    // Start is called before the first frame update
    void Start()
    {
        // gets the item manager instance and sets the item count for the spawner.
        itemManager = ItemManager.GetInstance();
        itemManager.SetItemCount(itemCount);

        // sets the spawn time
        // spawnCountdown = spawnTime;
    }

    // gets the item count
    public int GetItemCount()
    {
        // the item count is matched up with the manager.
        itemCount = itemManager.GetItemPoolCount();
        return itemCount;
    }

    // sets the new item count
    public void SetItemCount(int newCount)
    {
        // changes the item count
        itemManager.SetItemCount(newCount);
    }

    // Update is called once per frame
    void Update()
    {
        // checks to see if the spawner is enabled.
        if(spawnerEnabled)
        {
            // countdown to spawn another item.
            spawnCountdown -= Time.deltaTime;

            // another itme should be spawned.
            if (spawnCountdown <= 0.0F)
            {
                spawnCountdown = 0.0F;

                // if there are items in the pool, then something can be spawned.
                if (!itemManager.ItemPoolIsEmpty())
                {
                    spawnCountdown = spawnTime;

                    // randomizes the position
                    FieldItem newItem = itemManager.GetItem();
                    newItem.RandomizePosition(spawnAreaMin, spawnAreaMax);
                    newItem.useDespawnTimer = autoItemDespawn;
                }
            }
        } 
    }
}
