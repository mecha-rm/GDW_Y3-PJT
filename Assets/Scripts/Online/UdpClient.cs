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
    // TODO: the size of the outBuffer should probably be changed.
    private static byte[] outBuffer = new byte[512];
    private static IPEndPoint remoteEP;
    private static Socket client_socket;

    // the server port numbers
    private int client_port = 11111;

    // if 'true', messages are printed to the console.
    public bool printMessages = true;

    // Start is called before the first frame update
    void Start()
    {
        RunClient();
    }

    // runs the client
    void RunClient()
    {
        // inserts the IP address
        // IPAddress ip = IPAddress.Parse("000.000.0.00"); // the ip address
        IPAddress ip = IPAddress.Parse("127.0.0.1"); // the ip address (your ip address)
        remoteEP = new IPEndPoint(ip, client_port);

        client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // try
        try
        {
            outBuffer = Encoding.ASCII.GetBytes("Testing Connection... INFR3396U");
            client_socket.SendTo(outBuffer, remoteEP);
        }
        catch (Exception e)
        {
            // it also goes here if the client did not respond in time.
            if (printMessages)
            {
                Console.WriteLine(e.ToString());
                // Debug.LogError(e.ToString());
            }
        }

    }

    // gets the size of the buffer
    public int GetBufferSize()
    {
        return outBuffer.Length;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            client_socket.SendTo(outBuffer, remoteEP);
        }
        catch(Exception e)
        {
            if(printMessages)
            {
                Console.WriteLine(e.ToString());
                // Debug.LogError(e.ToString());
            }
        }
    }

    // shutting down server
    // OnDestroy is called when the object is deleted.
    private void OnDestroy()
    {
        // closing server
        if (client_socket != null)
        {
            client_socket.Shutdown(SocketShutdown.Both);
            client_socket.Close();
        }
    }
}
