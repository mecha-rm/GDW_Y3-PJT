using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// remote player script for online component
public class RemotePlayer : MonoBehaviour
{
    // FORMAT:
    // [0 - 10] - P1 Transform (PosX, PosY, P

    /// <summary>
    /// Format for writing data to the server.
    /// 
    /// </summary>

    /// <summary>
    /// Format for reading data from the server.
    /// Keep in mind that a float is 4 bytes long.
    /// * [0 - 47] - Player
    ///     - [0 - 3] - Player Number (4 bytes per int)
    ///     - [4 - 15] - Position (x, y, z) (4 bytes per float, 12 total)
    ///     - [16 - 27] - Scale (x, y, z) (4 bytes per float, 12 total)
    ///     - [28 - 43] - Rotation (x, y, z, w) (4 bytes per float, 16 total)
    ///     - [44 - 47] - Score (4 bytes per int, 4 bytes total)
    /// </summary>

    // player the remote player is attached to.
    public PlayerObject player;

    // size of data for sending player information over the internet.
    public const int DATA_SIZE = 48;

    // Start is called before the first frame update
    void Start()
    {
        // player not set
        if (player == null)
            GetComponent<PlayerObject>();
    }

    // gets the data size.
    public static int GetDataSize()
    {
        return DATA_SIZE;
    }

    // packs the player data and returns it.
    public byte[] GetData()
    {

        // size of content
        byte[] sendData = new byte[DATA_SIZE];

        // index of content
        int index = 0;

        // Player Number
        {
            byte[] data = BitConverter.GetBytes(player.playerNumber);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }
  

        // Player Position
        {
            byte[] data;

            // x position
            data = BitConverter.GetBytes(player.transform.position.x);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // y position
            data = BitConverter.GetBytes(player.transform.position.y);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // z position
            data = BitConverter.GetBytes(player.transform.position.z);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // Player Scale
        {
            byte[] data;

            // x scale
            data = BitConverter.GetBytes(player.transform.localScale.x);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // y scale
            data = BitConverter.GetBytes(player.transform.localScale.y);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // z scale
            data = BitConverter.GetBytes(player.transform.localScale.z);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // Player Rotation
        {
            byte[] data;

            // x rotation
            data = BitConverter.GetBytes(player.transform.rotation.x);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // y rotation
            data = BitConverter.GetBytes(player.transform.rotation.y);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // z rotation
            data = BitConverter.GetBytes(player.transform.rotation.z);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // w value
            data = BitConverter.GetBytes(player.transform.rotation.w);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // Player Score
        {
            byte[] data;

            // player score
            data = BitConverter.GetBytes(player.playerScore);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

        }

        return sendData;
    }

    // receive player data
    public void ApplyData(byte[] data)
    {
        // no data sent.
        // should account for not having enough data.
        if (data == null || data.Length == 0)
            return;

        // if the player has not been set.
        if (player == null)
            return;

        // index of content
        int index = 0;

        // Player Number (does not change)
        {
            //int pNum = BitConverter.ToInt32(data, index);
            //index += sizeof(int);

            index += sizeof(int); // skip player number
        }

        // Player Position
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

            player.transform.position = newPos;
        }

        // Player Scale
        {
            // gets the player scale
            Vector3 newScale = new Vector3();

            // getting position values
            newScale.x = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            newScale.y = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            newScale.z = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            player.transform.localScale = newScale;
        }

        // Player Rotation
        {
            // gets the player rotation
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

            player.transform.rotation = newRot;
        }

        // Player Score
        {
            float score = BitConverter.ToSingle(data, index);
            index += sizeof(float);
            player.playerScore = score;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // player object is set, and the server is running
        // if(player != null && NetworkLibrary.UdpServerXInterface.IsRunning())
        // {
        //     
        // 
        // 
        // }
    }
}
