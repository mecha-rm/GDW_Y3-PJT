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

    // sets the send timeout
    public int sendTimeout = 0;

    // sets the receive timeout
    public int receiveTimeout = 0;

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
        SetBlockingSockets(blocking);

        // set timeout values
        SetSendTimeout(sendTimeout);
        SetReceiveTimeout(receiveTimeout);


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

    // gets the send data
    public byte[] GetSendData()
    {
        return server.GetSendBufferData();
    }

    // sets the receive data
    public void SetSendData(byte[] data)
    {
        server.SetSendBufferData(data);
    }

    // gets the receive data
    public byte[] GetReceiveData(int index)
    {
        return server.GetReceiveBufferData(index);
    }
    
    // sets the receive data
    public void SetReceiveData(int index, byte[] data)
    {
        server.SetReceiveBufferData(index, data);
    }

    // gets number of endpoints
    public int GetEndPointCount()
    {
        return server.GetEndPointCount();
    }

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

    // gets the send timeout
    public int GetSendTimeout()
    {
        sendTimeout = server.GetSendTimeout();
        return sendTimeout;
    }

    // sets the send timeout
    public void SetSendTimeout(int sendTimeout)
    {
        // sets the send timeout
        this.sendTimeout = (sendTimeout >= 0) ? sendTimeout : 0;
        server.SetSendTimeout(this.sendTimeout);
    }

    // gets the receive timeout
    public int GetReceiveTimeout()
    {
        receiveTimeout = server.GetReceiveTimeout();
        return receiveTimeout;
    }

    // sets the receive timeout
    public void SetReceiveTimeout(int receiveTimeout)
    {
        // sets the receive timeout
        this.receiveTimeout = (receiveTimeout >= 0) ? receiveTimeout : 0;
        server.SetReceiveTimeout(this.receiveTimeout);
    }


    // returns 'true' if the server is running.
    public bool IsRunning()
    {
        return server.IsRunning();
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
        // sets blocking socket value
        SetBlockingSockets(blocking);

        // sets timeout values
        SetSendTimeout(sendTimeout);
        SetReceiveTimeout(receiveTimeout);

        // updates the server
        if (server.IsRunning())
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
