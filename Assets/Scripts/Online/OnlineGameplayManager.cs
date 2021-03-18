using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineGameplayManager : MonoBehaviour
{
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
    /// 

    // if 'true', this player is the host.
    public bool host;

    // server
    public UdpServer server;

    // client
    public UdpClient client;

    // player data to send over to other players.
    List<RemotePlayer> players = new List<RemotePlayer>();


    // Start is called before the first frame update
    void Start()
    {
        // if server hasn't been set.
        if(server == null)
        {
            server = FindObjectOfType<UdpServer>();

            if (server == null)
                server = new UdpServer();
        }

        // if the client hasn't been set.
        if (client == null)
        {
            client = FindObjectOfType<UdpClient>();

            if (client == null)
                client = new UdpClient();
        }
    }

    // runs the server (or the client)
    public void Run()
    {
        // runs the server or the client based on the expectation.
        // TODO: implement stop to prevent unending attempt to connect with TCP.
        if (host)
            server.RunServer();
        else
            client.RunClient();
    }

    void GetDataFromClients()
    {

    }

    void SendDataToClients()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
