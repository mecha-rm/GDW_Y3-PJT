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
    ///     - [0 - 3] - Identification Number (4 bytes per int)
    ///     - [4 - 15] - Position (x, y, z) (4 bytes per float, 12 total)
    ///     - [16 - 27] - Scale (x, y, z) (4 bytes per float, 12 total)
    ///     - [28 - 43] - Rotation (x, y, z, w) (4 bytes per float, 16 total)
    ///     - [44 - 47] - Score (4 bytes per int, 4 bytes total)
    /// </summary>
    /// 

    /// <summary>
    /// CLINET DATA SENT TO SERE SEND DATA
    /// </summary>


    // TODO: create interval

    // gets the game manager
    public GameplayManager gameManager;

    // the item spawner.
    public ItemSpawner itemSpawner;

    // the timer being used.
    public TimerObject timer;

    // TODO: use to correct the amount of time
    // private float timeDiff = 0.0F;

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

    // sets whether to block on sockets or not for both the server and client.
    public bool blocking = true;

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

        // if the item spawenr has not been set, search for it.
        if (itemSpawner == null)
            itemSpawner = FindObjectOfType<ItemSpawner>();


        // if server hasn't been set.
        if(server == null && isMaster)
        {
            server = FindObjectOfType<UdpServerX>();

            if (server == null)
                server = new UdpServerX();
        }

        // server is set, so add endpoints.
        if(server != null)
        {
            // set blocking sockets value
            server.SetBlockingSockets(blocking);

            // adds remote clients
            for (int i = 0; i < serverEndpoints; i++)
                server.AddEndPoint(serverBufferSize);
        }
        

        // if the client hasn't been set.
        if (client == null && !isMaster)
        {
            client = FindObjectOfType<UdpClient>();

            if (client == null)
                client = new UdpClient();

        }

        // set blocking sockets value
        if(client != null)
            client.SetBlockingSockets(blocking);

        // if the server or client should be run on start.
        // note that if either one is set to 'true' by default, they will override this.
        if (runHostOnStart)
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

    // checks to see if this is the master.
    public bool IsMaster()
    {
        return isMaster;
    }

    // sets value of master (if 'true', it's a server, if 'false', it's a client).
    public void SetMaster(bool value)
    {
        isMaster = value;
    }

    // gets the ip address of the active entity.
    public string GetIPAddress()
    {
        if(isMaster && server != null)
            return server.GetIPAddress();
        else if (!isMaster && client != null)
            return client.GetIPAddress();

        return "";
    }

    // sets the ip address
    // if 'setBoth' is set to 'true', the values of both are set (if applicable)
    public void SetIPAddress(string newIp, bool setBoth)
    {
        if ((isMaster || setBoth) && server != null)
            server.SetIPAddress(newIp);
        else if ((!isMaster || setBoth) && client != null)
            client.SetIPAddress(newIp);
    }

    // gets the port
    public int GetPort()
    {
        return server.GetPort();
    }

    // sets the port
    public void SetPort(int newPort)
    {
        server.SetPort(newPort);
    }

    // runs the server (or the client)
    public void RunHost()
    {
        // run appropriate host and set sockets for blocking (or not blocking).
        if (isMaster && server != null)
        {
            server.SetBlockingSockets(blocking);
            server.RunServer();
        }   
        else if (!isMaster && client != null)
        {
            client.SetBlockingSockets(blocking);
            client.RunClient();
        }
            

        dataComm = true;
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


        // removes the local player
        List<RemotePlayer> otherPlayers = new List<RemotePlayer>(players);
        otherPlayers.Remove(localPlayer); // removes the local player.

        // list of data that hasn't been matched.
        List<RemotePlayer.RemotePlayerData> unmatchedData = new List<RemotePlayer.RemotePlayerData>();

        // the list of found id values.
        List<int> foundIds = new List<int>();

        // gets the amount of endpoints
        int epAmount = server.server.GetEndPointCount();
        // int playerCount = otherPlayers.Count; // amount of players
        int playerCount = gameManager.playerCount; // amount of players

        // TODO: maybe check something with player count?

        // goes through each endpoint to get the data. There is a max of 3.
        for (int i = 0; i < epAmount; i++)
        {
            // remote player object.
            // RemotePlayer rp;
            // 
            // // bounds check for getting other player
            // if (i < otherPlayers.Count)
            //     rp = otherPlayers[i];
            // else
            //     break;



            // if the player is not equal to null.
            // if (rp.player != null)
            // {
            //     // if the player is controllable, then it should be ignored.
            // 
            //     if (rp.player.controllablePlayer)
            //         continue;
            // }

            // original
            // this didn't fix the problem, making restore original?
            // gets the proper data index and sends it to the player.
            // TODO: models keep flickering because the endpoint they match up with keeps getting changed.
            byte[] data = server.server.GetReceiveBufferData(i);
            // rp.ApplyData(data); // original

            // converted data.
            RemotePlayer.RemotePlayerData rpd = RemotePlayer.BytesToRemotePlayerData(data);

            // if the scale is 0, that means that no data was received.
            // you know this because under no circumstance should the scale EVER be zero.
            // this fix didn't stop the models swapping positions constantly.
            if(rpd.scale == Vector3.zero)
            {
                // since no data was received, skip.
                continue;
            }

            // if the id exists in the list, then it's that data has already been used.
            // as such, the data should be ignored.
            if(foundIds.Contains(rpd.idNumber))
            {
                continue;
            }

            // checks to see if the data was matched with anything.
            bool matched = false;

            // goes through list to find which player to apply the data to.
            for (int j = 0; j < otherPlayers.Count; j++)
            {
                // if the id numbers match, apply the data.
                if (otherPlayers[j].idNumber == rpd.idNumber)
                {
                    otherPlayers[j].ApplyData(rpd); // applies data
                    otherPlayers.RemoveAt(j); // removes matched player
                    matched = true; // data matched.

                    // adds the id to the list so that you know the data hasn't been set twice.
                    foundIds.Add(rpd.idNumber);
                    break;
                }
            }

            // saves data if it did not find a match.
            if (matched == false)
                unmatchedData.Add(rpd);
        }

        // unmatched data is applied to any remaining objects.
        if (unmatchedData.Count != 0 && otherPlayers.Count != 0)
        {
            // applies data based on placement in list.
            do
            {
                // Debug.Log("In Unmatched Data");

                // this should match the id number, meaning that it shouldn't be matched here ever again.
                otherPlayers[0].ApplyData(unmatchedData[0]);
                unmatchedData.RemoveAt(0);
                otherPlayers.RemoveAt(0);
            }
            while (unmatchedData.Count != 0 && otherPlayers.Count != 0);

            // // applies data based on placement in list.
            // for (int i = 0; i < otherPlayers.Count && i < unmatchedData.Count; i++)
            // {
            //     // this should match the id number, meaning that it shouldn't be matched here ever again.
            //     otherPlayers[i].ApplyData(unmatchedData[i]);
            //     unmatchedData.RemoveAt(i);
            // }
        }
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

        // gets all items currently on the field.
        FieldItem[] spawnedItems = FindObjectsOfType<FieldItem>(false);

        // Timer
        {
            // timer data
            byte[] data;
            
            // if the timer has been set, get the current time.
            if(timer != null)
                data = BitConverter.GetBytes(timer.GetCurrentTimeValue());
            else // no timer
                data = BitConverter.GetBytes(-1);

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
            byte[] data = BitConverter.GetBytes(spawnedItems.Length); // number of items
            Buffer.BlockCopy(data, 0, sendData, index, data.Length);
            index += data.Length;
        }

        // Players
        // getting player data
        // this sends the data of all four players.
        // // On the client side, the controlled player ignores the data of the player with the same number. 
        for (int i = 0; i < players.Count; i++)
        {
            byte[] pData = players[i].GetData();
            pData.CopyTo(sendData, index);
            index += pData.Length;
        }


        // Item Boxes
        {
            // copies the transformation data into the array (type, position, and rotation).
            for(int i = 0; i < spawnedItems.Length; i++)
            {
                byte[] iData = spawnedItems[i].GetData();
                iData.CopyTo(sendData, index);
                index += iData.Length;
            }
        }

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

        // list of other players
        List<RemotePlayer> otherPlayers = new List<RemotePlayer>(players);
        otherPlayers.Remove(localPlayer); // remove local player from list.
        
        // list of unmatched data. This should always be of size 1 since the controlled player getsi gnored..
        List<RemotePlayer.RemotePlayerData> unmatchedData = new List<RemotePlayer.RemotePlayerData>();

        // the list of found id values.
        List<int> foundIds = new List<int>();

        // values
        float time = -1.0F;
        int plyrCount = -1;
        int itemCount = -1;

        // Timer
        {
            time = BitConverter.ToSingle(recData, index);

            // TODO: check to see if the difference is great enough to make a change.
            // sets time.
            timer.SetCurrentTimeValue(time);
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
            // if (i >= otherPlayers.Count)
            //     continue;

            // gets data section
            byte[] pData = new byte[RemotePlayer.DATA_SIZE];
            Buffer.BlockCopy(recData, index, pData, 0, RemotePlayer.DATA_SIZE);
            index += pData.Length; // move onto next slot.

            // otherPlayers[i].ApplyData(pData);

            // converts data first before finding who it belongs to.
            RemotePlayer.RemotePlayerData rpd = RemotePlayer.BytesToRemotePlayerData(pData);

            // if the scale is 0, that means that no data was received.
            // you know this because under no circumstance should the scale EVER be zero.
            // TODO: this fix didn't stop the models swapping positions constantly.
            if (rpd.scale == Vector3.zero)
            {
                // since no data was received, skip.
                continue;
            }

            // if the id exists in the list, then it's that data has already been used.
            // as such, the data should be ignored.
            if (foundIds.Contains(rpd.idNumber))
            {
                continue;
            }

            // if the id number for the provided data is the same as that for the local player it is ignored. 
            // The list otherPlayers will always be one less than plyrCount due to the local player already being removed.
            if (rpd.idNumber == localPlayer.idNumber)
                continue;

            // checks to see if the data matched.
            bool matched = false;

            // goes through list to find which player to put it on.
            // goes through all player data passed.
            for(int j = 0; j < otherPlayers.Count; j++)
            {
                // if the id numbers match, apply the data.
                if (otherPlayers[j].idNumber == rpd.idNumber)
                {
                    otherPlayers[j].ApplyData(rpd); // applies data
                    otherPlayers.RemoveAt(j); // removes matched player
                    matched = true; // data matched.

                    // adds the id to the list so that you know the data hasn't been set twice.
                    foundIds.Add(rpd.idNumber);

                    break; 
                }
            }

            // saves data if it did not find a match.
            if (matched == false)
                unmatchedData.Add(rpd);


            // applies the data.
            // otherPlayers[i].ApplyData(pData);

        }


        // unmatched data is applied to any remaining objects.
        if(unmatchedData.Count != 0 && otherPlayers.Count != 0)
        {
            // applies data based on placement in list.
            do
            {
                // this should match the id number, meaning that it shouldn't be matched here ever again.
                otherPlayers[0].ApplyData(unmatchedData[0]);
                unmatchedData.RemoveAt(0);
                otherPlayers.RemoveAt(0);
            }
            while (unmatchedData.Count != 0 && otherPlayers.Count != 0);

            // // applies data based on placement in list.
            // for(int i = 0; i < otherPlayers.Count && i < unmatchedData.Count; i++)
            // {
            //     otherPlayers[i].ApplyData(unmatchedData[i]);
            // }
        }

        // Item Box Data
        // TODO: turn off item spawner for client so that it doesn't make new items.
        if(itemSpawner != null) // item spawner must be available for this to work.
        {
            // disables the spawner
            // itemSpawner.spawnerEnabled = false;

            FieldItem[] activeItems = FindObjectsOfType<FieldItem>(false);
            List<FieldItem.FieldItemData> recItems = new List<FieldItem.FieldItemData>();
            int arrIndex = 0;

            // gets all item transformation data.
            for (int i = 0; i < itemCount; i++)
            {
                // gets data section
                byte[] iData = new byte[FieldItem.DATA_SIZE];
                Buffer.BlockCopy(recData, index, iData, 0, FieldItem.DATA_SIZE);
                recItems.Add(FieldItem.BytesToFieldItemData(iData)); // adds data to list.

                index += iData.Length; // move onto next slot.
            }

            // while the array index is less than the amount of active items and received items.
            while(arrIndex < activeItems.Length && arrIndex < recItems.Count)
            {
                activeItems[arrIndex].ApplyData(recItems[arrIndex]);
                arrIndex++; // increment.
            }

            // gets the instance
            ItemManager im = ItemManager.GetInstance();

            // more items on the field than there should be.
            if (arrIndex < activeItems.Length)
            {
                // returns the item, and increments it.
                while(arrIndex < activeItems.Length)
                {
                    im.ReturnItem(activeItems[arrIndex]);
                    arrIndex++;
                }
            }
            // less items on the field than there should be.
            else if(arrIndex < recItems.Count)
            {
                // gets a new item, and adds it in.
                while (arrIndex < recItems.Count)
                {
                    FieldItem fieldItem = im.GetItem();
                    fieldItem.ApplyData(recItems[arrIndex]);
                    arrIndex++;
                }

            }
        }
    }

    // shuts down theh ost.
    public void ShutdownHost()
    {
        if (isMaster && server != null)
            server.ShutdownServer();
        else if (!isMaster && client != null)
            client.ShutdownClient();
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
