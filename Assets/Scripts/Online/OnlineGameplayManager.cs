using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnlineGameplayManager : MonoBehaviour
{
    // The Master (server) handles the time, and item calculation
    // The clients only handle their own score tallies and player controls.

    // TODO: needed data:
    // - Player Transformations
    // - Item Positions and Rotations
    // - Time
    // - 

    // TODO: transfer over item boxes

    /// <summary>
    /// Data that is sent out to each client.
    /// 
    /// * Order:
    ///     - Time (float) - 4 bytes
    ///     - Number of Players (Int) - 4 Bytes
    ///     - Number of Active Item Boxes (Int) - 4 bytes
    ///     - Players (Number, Transformation, Score) - 47 Bytes each
    ///     - Items (Type, Position, Orientation) - 
    ///     - 
    ///     
    /// </summary>

    /// <summary>
    /// SERVER SEND DATA
    /// Keep in mind that a float is 4 bytes long.
    /// * [0 - 47] - Player
    ///     - [0 - 3] - Player Number (4 bytes per int)
    ///     - [4 - 15] - Position (x, y, z) (4 bytes per float, 12 total)
    ///     - [16 - 27] - Scale (x, y, z) (4 bytes per float, 12 total)
    ///     - [28 - 43] - Rotation (x, y, z, w) (4 bytes per float, 16 total)
    ///     - [44 - 47] - Score (4 bytes per int, 4 bytes total)
    /// </summary>
    /// 

    /// <summary>
    /// CLINET DATA SENT TO SERE SEND DATA
    /// </summary>


    // gets the game manager
    public GameplayManager gameManager;

    // if 'true', this player is the host.
    public bool master;

    // server
    public UdpServerX server;

    // client
    public UdpClient client;

    // player data to send over to other players.
    public List<RemotePlayer> players = new List<RemotePlayer>();

    // the number of endpoints enabled.
    // this only applies upon activation.
    // this gets set when the server is run.
    public int serverEndpoints = 1;

    // if 'true', data is communicated.
    public bool dataComm = true;


    // Start is called before the first frame update
    void Start()
    {
        // finds gameplay manager if not set.
        if (gameManager == null)
            gameManager = FindObjectOfType<GameplayManager>();

        // if server hasn't been set.
        if(server == null)
        {
            server = FindObjectOfType<UdpServerX>();

            if (server == null)
                server = new UdpServerX();

            // adds remote clients
            for (int i = 0; i < serverEndpoints; i++)
                server.server.AddRemoteClient(server.bufferSize);
        }

        // if the client hasn't been set.
        if (client == null)
        {
            client = FindObjectOfType<UdpClient>();

            if (client == null)
                client = new UdpClient();
        }

        // if the list hasn't had anything put into it.
        if (players.Count == 0)
        {
            RemotePlayer[] arr = FindObjectsOfType<RemotePlayer>();

            // adds players to list.
            foreach (RemotePlayer rp in arr)
                players.Add(rp);
        }
            

    }

    // runs the server (or the client)
    public void Run()
    {
        // runs the server or the client based on the expectation.
        // TODO: implement stop to prevent unending attempt to connect with TCP.
        if (master)
        {
            // for(int i = 0; i < numOfEndpoints; i++)
            //     server.add

            server.RunServer();
        }
        else
        {
            client.RunClient();
        }
            
    }

    // gets the data from client
    void GetDataFromClients()
    {
        // Step 1: Get Info From Players
        // Step 2: Apply Data
        // Step 3: Send Off Data

        // byte[] data = ;

        // NOTE: only the player data gets received.
        // NOTE: figure out how to track if players get items
        // would be easier just to have each side generate their own items to be honest.

        // goes trhough each player and provides the data
        for(int i = 0; i < gameManager.playerCount; i++)
        {

            // TODO: get proper section of data.
            byte[] data = server.server.GetReceiveBufferData(i);

            // sends data to remote player object.
            if (i < players.Count)
                players[i].ApplyData(data);
        }

        // TODO: set up item boxes
    }

    // sends the data to the clients
    void SendDataToClients()
    {
        // Time (float) - 4 bytes
        // Number of Players (Int) - 4 Bytes
        // Number of Active Item Boxes (Int) - 4 bytes
        // Players
        // Items
        
        
        // data to be sent out to clients.
        byte[] sendData = new byte[server.bufferSize];
        int index = 0;

        // Timer
        {
            // TODO: replace with actual timer.
            byte[] data = BitConverter.GetBytes(0);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // Number of Players
        {
            byte[] data = BitConverter.GetBytes(players.Count);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // Number of Item Boxes
        {
            // TODO: replace with number of item boxes.
            byte[] data = BitConverter.GetBytes(0);
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // Players
        // getting player data
        for (int i = 0; i < players.Count; i++)
        {
            byte[] pData = players[i].GetData();
            pData.CopyTo(sendData, index);
            index += pData.Length;
        }

        // TODO: figure out how to track other players getting items

        // Item Boxes
        // {
        //     // TODO: get item manager.
        //     FieldItem[] items = FindObjectsOfType<FieldItem>();
        // 
        //     for(int i = 0; i < items.Length; i++)
        //     {
        //         byte[] iData = items[i].GetData();
        //         iData.CopyTo(sendData, index);
        //         index += iData.Length;
        //     }
        // }

        // sets the send buffer data
        server.server.SetSendBufferData(sendData);
    }

    // Update is called once per frame
    void Update()
    {
        // gets the data from the clients.
        GetDataFromClients();

        // sends the data from the clients.
        SendDataToClients();
    }
}
