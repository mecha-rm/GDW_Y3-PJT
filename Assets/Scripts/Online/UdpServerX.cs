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

    // if 'true', the server starts running once the program starts.
    public bool runOnStart = false;

    // if 'true', messages are printed to the console.
    public bool printMessages = true;


    // Start is called before the first frame update
    void Start()
    {
        if(server == null)
            server = new NetworkLibrary.UdpServerX();

        // runs the server.
        if (runOnStart)
            RunServer();
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

    // call to start runnnig the server
    public void RunServer()
    {
        // if the server isn't already running.
        if(!server.IsRunning())
            server.RunServer();
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
