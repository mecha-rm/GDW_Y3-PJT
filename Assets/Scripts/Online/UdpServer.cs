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
public class UdpServer : MonoBehaviour
{
    // if 'true', the server starts running once the program starts.
    bool runOnStart = true;

    // if 'true', messages are printed to the console.
    public bool printMessages = true;

    // Start is called before the first frame update
    void Start()
    {
        // runs the server.
        if (runOnStart)
            RunServer();
    }
    
    // call to start runnnig the server
    public void RunServer()
    {
        // if the server isn't already running.
        if(!NetworkLibrary.UdpServerXInterface.IsRunning())
            NetworkLibrary.UdpServerXInterface.RunServer();
    }

    // Update is called once per frame
    void Update()
    {
        // updates the server
        NetworkLibrary.UdpServerXInterface.Update();
    }


    // shuts down the server
    // OnDestroy is called when the object is deleted.
    private void OnDestroy()
    {
        // shuts down the server if this script is destroyed.
        NetworkLibrary.UdpServerXInterface.ShutdownServer();
    }
}
