using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// socket communication
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class UdpClient : MonoBehaviour
{
    // client
    public NetworkLibrary.UdpClient client;

    // sets whether to block on sockets or not.
    // this sets the value at the instantiation (if applicable), at SetBlockingSockets(), and at RunClient().
    public bool blocking = true;

    // if 'true', the server starts running once the program starts.
    public bool runOnStart = false;

    // if 'true', messages are printed to the console.
    public bool printMessages = true;

    // TODO: change this to an independent variable.

    // called so that the server is initialized as early as possible.
    // this wasn't initialized in time during testing due to initialization order.
    private void Awake()
    {
        // creates client if it's not set.
        if (client == null)
        {
            client = new NetworkLibrary.UdpClient();

            // two-way communication
            client.SetCommunicationMode(NetworkLibrary.UdpClient.mode.both);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // sets blocking value.
        client.SetBlockingSockets(blocking);

        // runs the client.
        if (runOnStart)
            RunClient();
    }

    // gets the ip address
    public string GetIPAddress()
    {
        return client.GetIPAddress();
    }

    // sets the ip address
    public void SetIPAddress(string newIp)
    {
        client.SetIPAdress(newIp);
    }

    // gets the port
    public int GetPort()
    {
        return client.GetPort();
    }

    // sets the port
    public void SetPort(int newPort)
    {
        client.SetPort(newPort);
    }

    // is blocking sockets
    public bool IsBlockingSockets()
    {
        blocking = client.IsBlockingSockets();
        return blocking;
    }

    // set blocking sockets
    public void SetBlockingSockets(bool blocking)
    {
        this.blocking = blocking; // sets blocking variable.
        client.SetBlockingSockets(blocking);
    }

    // runs the client
    public void RunClient()
    {
        if(!client.IsRunning())
        {
            client.SetBlockingSockets(blocking); // updated value
            client.RunClient();
        }
            
    }

    // shuts down the client.
    public void ShutdownClient()
    {
        client.ShutdownClient();
    }

    // Update is called once per frame
    void Update()
    {
        client.Update();
    }

    // shutting down server
    // OnDestroy is called when the object is deleted.
    private void OnDestroy()
    {
        // shuts down the client
        if (client.IsRunning())
            client.ShutdownClient();
    }
}
