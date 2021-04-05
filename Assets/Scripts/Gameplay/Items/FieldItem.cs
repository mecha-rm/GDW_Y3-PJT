using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// TODO: add item number variable so that you know which items have been turned on/off.
// script for items that are on the field and available for pickup
public class FieldItem : MonoBehaviour
{
    // struct for field item data.
    public struct FieldItemData
    {
        public int itemSet;
        public Vector3 position;
        public Quaternion rotation;
    }

    // enum for available items
    // TODO: add more item types
    public enum itemType
    {
        none, speedUp, jumpUp
    }

    // number of items available
    private const int ITEM_COUNT = 2;

    // the number of the item
    public itemType itemSet = 0;

    // if 'true', the field item despawns after a certain amount of time.
    public bool useDespawnTimer = true;

    // timer for despawning the item (in seconds).
    public static float MaxDespawnTime = 50.0F;
    public float despawnCountdown = 0.0F;

    // the death space of the field item.
    public DeathSpace deathSpace;

    // data size (32 bytes)
    // Type - 1 Int (4 Bytes)
    // Position - 3 Floats (12 Bytes)
    // Rotation - 4 Bytes (16 Bytes)
    public const int DATA_SIZE = 32;

    // Start is called before the first frame update
    void Start()
    {
        despawnCountdown = MaxDespawnTime;

        // gets the deaths pace
        deathSpace = FindObjectOfType<DeathSpace>();
    }

    // starts the countdown over.
    public void ResetDespawnCountdown()
    {
        // resets the despawn countdown
        despawnCountdown = MaxDespawnTime;
    }

    // gets the total amount of items
    public int GetTotalItemAmount()
    {
        return ITEM_COUNT;
    }

    // randomizes the item number of the 
    public itemType RandomizeItem()
    {
        // TODO: have adjustable probability rates.

        // randomizes the item
        itemSet = (itemType)UnityEngine.Random.Range(1, ITEM_COUNT + 1);

        return itemSet;
    }

    // randomizes the field item's position using a minimum (inclusive) and maximum (inclusive)
    public void RandomizePosition(Vector3 min, Vector3 max)
    {
        // creates a new position.
        transform.position = new Vector3(
            UnityEngine.Random.Range(min.x, max.x),
            UnityEngine.Random.Range(min.y, max.y),
            UnityEngine.Random.Range(min.z, max.z)
        );
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
                // checks to see if the player already has a speed item attached.
                SpeedUpItem spdItem = player.gameObject.GetComponent<SpeedUpItem>();

                // if the player already has a speed item, the countdown for it is reset.
                // if they didn't have a speed item, they are given one.
                if (spdItem == null)
                    genItem = player.gameObject.AddComponent<SpeedUpItem>();
                else
                    spdItem.ResetCountdown();

                break;
            case itemType.jumpUp: // jump
                // checks to see if the player already has a jump item attached.
                JumpUpItem jumpItem = player.gameObject.GetComponent<JumpUpItem>();

                // if the player already has a jump item, the countdown for it is reset.
                // if they didn't have a jump item, they are given one.
                if (jumpItem == null)
                    genItem = player.gameObject.AddComponent<JumpUpItem>();
                else
                    jumpItem.ResetCountdown();
                
                break;
        }

        // activates the effect.
        if (genItem != null)
            genItem.ActivateEffect(player);

        return genItem;
    }

    // gets the data for this field item (type, position, and roation)
    public byte[] GetData()
    {
        // Bytes: 32 Total
        // Type - 1 Int (4 Bytes)
        // Position - 3 Floats (12 Bytes)
        // Rotation - 4 Bytes (16 Bytes)

        // size of content
        byte[] sendData = new byte[DATA_SIZE];

        // index of content
        int index = 0;

        // Item Number
        {
            byte[] data = BitConverter.GetBytes((int)itemSet);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }


        // Item Position
        {
            byte[] data;

            // x position
            data = BitConverter.GetBytes(transform.position.x);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // y position
            data = BitConverter.GetBytes(transform.position.y);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // z position
            data = BitConverter.GetBytes(transform.position.z);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // Item Rotation
        {
            byte[] data;

            // x rotation
            data = BitConverter.GetBytes(transform.rotation.x);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // y rotation
            data = BitConverter.GetBytes(transform.rotation.y);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // z rotation
            data = BitConverter.GetBytes(transform.rotation.z);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // w value
            data = BitConverter.GetBytes(transform.rotation.w);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        return sendData;
    }

    // applys the data for this field item (type, position, and roation)
    public void ApplyData(byte[] data)
    {
        // index of content
        int index = 0;

        // Item Set
        {
            int type = BitConverter.ToInt32(data, index);
            itemSet = (itemType)type;
            index += sizeof(int); // move onto next value
        }

        // Item Position
        {
            // gets the player position
            Vector3 newPos = new Vector3();

            // getting position values
            newPos.x = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            newPos.y = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            newPos.z = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            transform.position = newPos;
        }
  

        // Item Rotation
        {
            // gets the item rotation
            Quaternion newRot = new Quaternion();

            // getting position values
            newRot.x = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            newRot.y = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            newRot.z = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            newRot.w = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            transform.rotation = newRot;
        }
    }


    // applys the data for this field item (type, position, and rotation)
    public void ApplyData(FieldItemData fid)
    {
        // copies data
        itemSet = (itemType)fid.itemSet;
        transform.position = fid.position;
        transform.rotation = fid.rotation;
    }

    // converts remote player data to bytes
    public static byte[] FieldItemDataToBytes(FieldItemData fid)
    {
        // Bytes: 32 Total
        // Type - 1 Int (4 Bytes)
        // Position - 3 Floats (12 Bytes)
        // Rotation - 4 Bytes (16 Bytes)

        // size of content
        byte[] sendData = new byte[DATA_SIZE];

        // index of content
        int index = 0;

        // Item Number
        {
            byte[] data = BitConverter.GetBytes(fid.itemSet);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }


        // Item Position
        {
            byte[] data;

            // x position
            data = BitConverter.GetBytes(fid.position.x);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // y position
            data = BitConverter.GetBytes(fid.position.y);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // z position
            data = BitConverter.GetBytes(fid.position.z);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // Item Rotation
        {
            byte[] data;

            // x rotation
            data = BitConverter.GetBytes(fid.rotation.x);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // y rotation
            data = BitConverter.GetBytes(fid.rotation.y);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // z rotation
            data = BitConverter.GetBytes(fid.rotation.z);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // w value
            data = BitConverter.GetBytes(fid.rotation.w);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        return sendData;
    }

    // converts remote player data to bytes
    public static FieldItemData BytesToFieldItemData(byte[] data)
    {
        // field item
        FieldItemData fid = new FieldItemData();

        // index of content
        int index = 0;

        // no data sent.
        // TODO: should check for enough data being available.
        if (data == null || data.Length == 0)
            return fid;

        // Item Set
        {
            fid.itemSet = BitConverter.ToInt32(data, index);
            index += sizeof(int); // move onto next value
        }

        // Item Position
        {
            // getting position values
            // x value
            fid.position.x = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            // y value
            fid.position.y = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            // z value
            fid.position.z = BitConverter.ToSingle(data, index);
            index += sizeof(float);
        }


        // Item Rotation
        {
            // getting position values
            fid.rotation.x = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            fid.rotation.y = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            fid.rotation.z = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            fid.rotation.w = BitConverter.ToSingle(data, index);
            index += sizeof(float);
        }

        return fid;
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

        // returns self to the pool
        ItemManager.GetInstance().ReturnItem(this);

        // deactivates object (this happens in the return item section)
        // gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // if there is a death space
        if(deathSpace != null)
        {
            // if the item box falls into the death space, it is returned to the object pool.
            if(deathSpace.InDeathSpace(transform.position) == true)
            {
                ItemManager.GetInstance().ReturnItem(this);
            }
        }
        // maybe check for the death space if it isn't set?

        // counts down to when this item should despawn
        if(useDespawnTimer)
        {
            despawnCountdown -= Time.deltaTime;

            // returns item to pool.
            if (despawnCountdown <= 0.0F)
            {
                despawnCountdown = 0.0F;
                ItemManager.GetInstance().ReturnItem(this);
            }
        }
    }
}
