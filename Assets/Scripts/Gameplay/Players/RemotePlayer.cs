using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// remote player script for online component
public class RemotePlayer : MonoBehaviour
{
    // struct for remote player data.
    public struct RemotePlayerData
    {
        public int idNumber;
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
        public float playerScore;
    }

    // FORMAT:
    /// <summary>
    /// Format for reading data from the server.
    /// Keep in mind that a float is 4 bytes long.
    /// * [0 - 47] - Player
    ///     - [0 - 3] - Identification Number (4 bytes per int)
    ///     - [4 - 15] - Position (x, y, z) (4 bytes per float, 12 total)
    ///     - [16 - 27] - Scale (x, y, z) (4 bytes per float, 12 total)
    ///     - [28 - 43] - Rotation (x, y, z, w) (4 bytes per float, 16 total)
    ///     - [44 - 47] - Score (4 bytes per int, 4 bytes total)
    /// </summary>

    // player the remote player is attached to.
    public PlayerObject player;

    // the identification number for updating the remote player
    // this should be setup and provided by the server. This should stay the same for the whole round.
    // id numbers should be random, and only be made positive.
    public int idNumber = -1;

    // randomize the id number on start.
    public bool randomizeIdOnStart = true;

    // size of data for sending player information over the internet.
    public const int DATA_SIZE = 48;

    // Start is called before the first frame update
    void Start()
    {
        // player not set
        if (player == null)
            player = GetComponent<PlayerObject>();

        // randomizes the id number for the remote player that goes from 1 to the max value.
        if (randomizeIdOnStart)
            RandomizeIdNumber();
    }

    // randomizes the ID number.
    public void RandomizeIdNumber()
    {
        idNumber = UnityEngine.Random.Range(1, int.MaxValue);
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

        // Identification Number
        {
            byte[] data = BitConverter.GetBytes(idNumber);
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

        // Identification Number
        {
            idNumber = BitConverter.ToInt32(data, index);
            index += sizeof(int);
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

    // applies remote player data
    public void ApplyData(RemotePlayerData rpd)
    {
        // player number should not be overriddden
        idNumber = rpd.idNumber;
        player.transform.position = rpd.position;
        player.transform.localScale = rpd.scale;
        player.transform.rotation = rpd.rotation;
        player.playerScore = rpd.playerScore;
    }

    // converts remote player data to bytes
    public static byte[] RemotePlayerDataToBytes(RemotePlayerData rpd)
    {
        // size of content
        byte[] sendData = new byte[DATA_SIZE];

        // index of content
        int index = 0;

        // Identification Number
        {
            byte[] data = BitConverter.GetBytes(rpd.idNumber);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }


        // Player Position
        {
            byte[] data;

            // x position
            data = BitConverter.GetBytes(rpd.position.x);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // y position
            data = BitConverter.GetBytes(rpd.position.y);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // z position
            data = BitConverter.GetBytes(rpd.position.z);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // Player Scale
        {
            byte[] data;

            // x scale
            data = BitConverter.GetBytes(rpd.scale.x);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // y scale
            data = BitConverter.GetBytes(rpd.scale.y);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // z scale
            data = BitConverter.GetBytes(rpd.scale.z);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // Player Rotation
        {
            byte[] data;

            // x rotation
            data = BitConverter.GetBytes(rpd.rotation.x);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // y rotation
            data = BitConverter.GetBytes(rpd.rotation.y);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // z rotation
            data = BitConverter.GetBytes(rpd.rotation.z);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

            // w value
            data = BitConverter.GetBytes(rpd.rotation.w);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // Player Score
        {
            byte[] data;

            // player score
            data = BitConverter.GetBytes(rpd.playerScore);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;

        }

        return sendData;
    }

    // converts byte data to remote player data
    public static RemotePlayerData BytesToRemotePlayerData(byte[] data)
    {
        // remote player data
        RemotePlayerData rpd = new RemotePlayerData();
        
        // index
        int index = 0;

        // no data sent.
        // TODO: should check for enough data being available.
        if (data == null || data.Length == 0)
            return rpd;

        // ID Number
        {
            rpd.idNumber = BitConverter.ToInt32(data, index);
            index += sizeof(int);
        }

        // Player Position
        {
            // getting position values
            // x value
            rpd.position.x = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            // y value
            rpd.position.y = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            // z value
            rpd.position.z = BitConverter.ToSingle(data, index);
            index += sizeof(float);
        }

        // Player Scale
        {
            // getting position values
            // x value
            rpd.scale.x = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            // y value
            rpd.scale.y = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            // z value
            rpd.scale.z = BitConverter.ToSingle(data, index);
            index += sizeof(float);
        }

        // Player Rotation
        {
            // getting position values
            // x value
            rpd.rotation.x = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            // y value
            rpd.rotation.y = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            // z value
            rpd.rotation.z = BitConverter.ToSingle(data, index);
            index += sizeof(float);

            // w value
            rpd.rotation.w = BitConverter.ToSingle(data, index);
            index += sizeof(float);
        }

        // Player Score
        {
            rpd.playerScore = BitConverter.ToSingle(data, index);
            index += sizeof(float);
        }

        return rpd;
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
