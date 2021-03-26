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
    public bool isMaster = true;

    // server
    public UdpServerX server;

    // client
    public UdpClient client;

    // player data to send over to other players.
    // the controlled player should be in this list.
    public List<RemotePlayer> players = new List<RemotePlayer>();

    // local controlled player.
    private RemotePlayer localPlayer = null;

    // the number of endpoints enabled.
    // this only applies upon activation.
    // this gets set when the server is run.
    public int serverEndpoints = 1;

    // run server and client on start.
    public bool runHostOnStart = false;

    // if 'true', data is communicated.
    public bool dataComm = true;

    // buffer size for clients and servers
    public int serverBufferSize = 512;
    public int clientBufferSize = 512;

    // Start is called before the first frame update
    void Start()
    {
        // finds gameplay manager if not set.
        if (gameManager == null)
            gameManager = FindObjectOfType<GameplayManager>();


        // if server hasn't been set.
        if(server == null && isMaster)
        {
            server = FindObjectOfType<UdpServerX>();

            if (server == null)
                server = new UdpServerX();

            // adds remote clients
            for (int i = 0; i < serverEndpoints; i++)
                server.server.AddEndPoint(serverBufferSize);
        }

        // if the client hasn't been set.
        if (client == null && !isMaster)
        {
            client = FindObjectOfType<UdpClient>();

            if (client == null)
                client = new UdpClient();

        }

        // if the server or client should be run on start.
        // note that if either one is set to 'true' by default, they will override this.
        if(runHostOnStart)
        {
            if (isMaster)
                server.RunServer();
            else
                client.RunClient();
        }

        // if the list hasn't had anything put into it.
        if (players.Count == 0)
        {
            RemotePlayer[] arr = FindObjectsOfType<RemotePlayer>();

            // adds players to list.
            foreach (RemotePlayer rp in arr)
                players.Add(rp);
        }

        // finds controlled player
        foreach(RemotePlayer rp in players)
        {
            if(rp.player.controllablePlayer)
            {
                localPlayer = rp;
                break;
            }    
        }
            

    }

    // runs the server (or the client)
    public void Run()
    {
        // runs the server or the client based on the expectation.
        // TODO: implement stop to prevent unending attempt to connect with TCP.
        if (isMaster)
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

    // SERVER -> CLIENT (MASTER) //

    // gets the data from client
    void ReceiveDataFromClients()
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
        byte[] sendData = new byte[serverBufferSize];
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


    // CLIENT -> SERVER (NOT MASTER) //
    // sends the client's data to the server.
    void SendDataToServer()
    {
        // gets player position
        byte[] sendData = new byte[clientBufferSize];
        int index = 0;

        if (localPlayer == null)
        {
            // searches for local player
            for (int i = 0; i < players.Count; i++)
            {
                PlayerObject p = players[i].player;

                // local player found
                if (p.controllablePlayer)
                {
                    localPlayer = players[i];
                    break;
                }

            }
        }

        // local player is equal to null, so there's nothing to send.
        if (localPlayer == null)
            return;

        // copies data
        byte[] data = localPlayer.GetData();

        Buffer.BlockCopy(data, 0, sendData, index, data.Length);
        index += data.Length;

        client.client.SetSendBufferData(sendData);

    }

    // receives data from the server.
    void ReceiveDataFromServer()
    {
        // Time (float) - 4 bytes
        // Number of Players (Int) - 4 Bytes
        // Number of Active Item Boxes (Int) - 4 bytes
        // Players
        // Items

        // data to be sent out to clients.
        byte[] recData = client.client.GetReceiveBufferData();
        int index = 0;

        // values
        float time = -1.0F;
        int plyrCount = -1;
        int itemCount = -1;

        // Timer
        {
            time = BitConverter.ToSingle(recData, index);
            // TODO: update timer
            index += sizeof(float);

        }

        // Number of Players
        {
            plyrCount = BitConverter.ToInt32(recData, index);
            // TODO: check to see if number of players changed?
            index += sizeof(int);
        }

        // Number of Item Boxes
        {
            itemCount = BitConverter.ToInt32(recData, index);
            // TODO: adjust item box amount
            index += sizeof(int);
        }

        // Players - getting player data
        for (int i = 0; i < plyrCount; i++)
        {
            // the player count is different.
            if (i > players.Count)
                continue;

            // if this is the local player, ignore it.
            if (localPlayer == players[i])
                continue;

            // gets data section
            byte[] pData = new byte[RemotePlayer.DATA_SIZE];
            Buffer.BlockCopy(recData, index, pData, 0, RemotePlayer.DATA_SIZE);
            index += pData.Length;

            // applies the data.
            players[i].ApplyData(pData);
        }

        // TODO: apply item box data.

    }

    // runs host
    public void RunHost()
    {
        if (isMaster && server != null)
            server.RunServer();
        else if (!isMaster && client != null)
            client.RunClient();

        dataComm = true;
    }

    // Update is called once per frame
    void Update()
    {
        // if data should be communicated.
        if(dataComm)
        {
            if (isMaster && server.server.IsRunning()) // this player is the server
            {
                // gets the data from the clients.
                ReceiveDataFromClients();

                // sends the data from the clients.
                SendDataToClients();
            }
            else if(!isMaster && client.client.IsRunning()) // this player is the client.
            {
                // sends the data to the server
                SendDataToServer();

                // receives data from the server.
                ReceiveDataFromServer();
            }
        }

        
    }
}
