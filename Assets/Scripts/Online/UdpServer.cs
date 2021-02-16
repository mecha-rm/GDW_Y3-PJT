using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for sockets
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

// UDP Server
public class UdpServer : MonoBehaviour
{
    private static byte[] buffer = new byte[512];
    private static IPEndPoint client = null;

    private static Socket server_socket = null;
    private static EndPoint remoteClient = null;

    // receive timeout and send timeout times
    // these determine how long the server should wait for a client to connect.
    // if not set, the server will wait forever for lack of a better word. 
    public int receiveTimeout = 3;
    public int sendTimeout = 3;

    // if 'true', messages are printed to the console.
    public bool printMessages = true;

    // Start is called before the first frame update
    void Start()
    {
        RunServer();
    }

    // runs the server
    void RunServer()
    {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ip = host.AddressList[1]; // get IP address from list
        
        // the IP address can be manually entered using IPAddress.Parse(), which the line of code below shows.
        // the IP Address can be found by typing "ipconfig" into the Command Line.
        // IPAddress ip = IPAddress.Parse("000.000.0.000"); // manual entry 

        // prints messages to console
        if(printMessages)
            Console.WriteLine("Server name: {0} IP: {1}", host.HostName, ip);

        // TODO: maybe make the port a variable
        IPEndPoint localEP = new IPEndPoint(ip, 11111);

        // creates the server socket - gets the IP directly from the address family.
        server_socket = new Socket(ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

        // 0 for any available port.
        client = new IPEndPoint(IPAddress.Any, 0); // 0 for any available port.
        remoteClient = (EndPoint)client;

        // binds server and EP
        try
        {
            // the server listens and provides a service.
            server_socket.Bind(localEP);

            if(printMessages)
                Console.WriteLine("Waiting for data...");

            // sets timeouts for sending and receiving data
            server_socket.ReceiveTimeout = receiveTimeout;
            server_socket.SendTimeout = sendTimeout;

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            // TCP we use "Receive", and for UDP we use "ReceiveFrom" (possibly written incorrectly)
            // 'ref' means passing by reference.
            // waits to receive something from the client, trhowing an exception if nothing is given.
            int rec = server_socket.ReceiveFrom(buffer, ref remoteClient);

            // writes data
            Console.WriteLine("Received: {0} from Client: {1}", Encoding.ASCII.GetString(buffer, 0, rec), remoteClient.ToString());

            // use BitConverter to convert bytes to other data types
            // e.g. BitConverter.ToSingle(buffer, 0);
        }
        catch (Exception e)
        {
            // it also goes here if the client did not respond in time.
            Console.WriteLine(e.ToString());
        }
    }

    // shuts down the server
    // OnDestroy is called when the object is deleted.
    private void OnDestroy()
    {
        // TODO: this might throw an error for some reason.
        // closing server
        if (server_socket != null)
        {
            server_socket.Shutdown(SocketShutdown.Both);
            server_socket.Close();
        }

    }
}
