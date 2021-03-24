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

    // if 'true', the server starts running once the program starts.
    bool runOnStart = true;

    // if 'true', messages are printed to the console.
    public bool printMessages = true;

    // TODO: change this to an independent variable.

    // Start is called before the first frame update
    void Start()
    {
        if (client == null)
            client = new NetworkLibrary.UdpClient();

        // runs the client.
        if (runOnStart)
            RunClient();
    }

    // runs the client
    public void RunClient()
    {
        client.RunClient();
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
        client.ShutdownClient();
    }
}
