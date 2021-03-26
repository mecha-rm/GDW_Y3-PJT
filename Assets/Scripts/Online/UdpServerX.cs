using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// for sockets
// using System;
// using System.Text;
// using System.Net;
// using System.Net.Sockets;

// UDP Server
public class UdpServerX : MonoBehaviour
{
    // server
    public NetworkLibrary.UdpServerX server;

    // sets whether to block on sockets or not.
    // this sets the value at the instantiation (if applicable), at SetBlockingSockets(), and at RunServer().
    public bool blocking = true;

    // if 'true', the server starts running once the program starts.
    public bool runOnStart = false;

    // if 'true', messages are printed to the console.
    public bool printMessages = true;

    // called so that the server is initialized as early as possible.
    // this wasn't initialized in time during testing due to initialization order.
    private void Awake()
    {
        // creates server if it's not set.
        if (server == null)
        {
            server = new NetworkLibrary.UdpServerX();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // sets blocking value.
        server.SetBlockingSockets(blocking);


        // runs the server.
        if (runOnStart)
            RunServer();
    }

    // gets the ip address
    public string GetIPAddress()
    {
        return server.GetIPAddress();
    }

    // sets the ip address
    public void SetIPAddress(string newIp)
    {
        server.SetIPAdress(newIp);
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

    // public NetworkLibrary.UdpServerXInterface GetServer()
    // {
    //     NetworkLibrary.UdpServerXInterface x = ;
    // }

    // gets the data
    // public byte[] GetData(int index)
    // {
    //     return server.GetReceiveBufferData(index);
    // }
    // 
    // // sets the data
    // public void SetData(int index, byte[] data)
    // {
    //     server.SetReceiveBufferData(index, data);
    // }

    // adds a remote client
    public void AddEndPoint()
    {
        server.AddEndPoint();
    }

    // adds a remote client
    public void AddEndPoint(int bufferSize)
    {
        server.AddEndPoint(bufferSize);
    }

    // is blocking sockets
    public bool IsBlockingSockets()
    {
        blocking = server.IsBlockingSockets();
        return blocking;
    }

    // set blocking sockets
    public void SetBlockingSockets(bool blocking)
    {
        this.blocking = blocking; // sets blocking variable.
        server.SetBlockingSockets(blocking);
    }

    // call to start runnnig the server
    public void RunServer()
    {
        // if the server isn't already running.
        if(!server.IsRunning())
        {
            server.SetBlockingSockets(blocking); // updated value
            server.RunServer();
        }
            
    }

    // shuts down the server.
    public void ShutdownServer()
    {
        server.ShutdownServer();
    }

    // Update is called once per frame
    void Update()
    {
        // updates the server
        if(server.IsRunning())
            server.Update();
    }


    // shuts down the server
    // OnDestroy is called when the object is deleted.
    private void OnDestroy()
    {
        // shuts down the server if this script is destroyed.
        server.ShutdownServer();
    }
}
