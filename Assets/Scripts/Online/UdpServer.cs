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
    // server variables
    private static byte[] buffer = new byte[512];
    private static IPEndPoint client = null;

    // server and remote client
    private static Socket server_socket = null;
    private static EndPoint remoteClient = null;

    // the server port numbers
    private int server_port = 11111;

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
        // IPAddress ip = host.AddressList[1]; // get IP address from user's system
        IPAddress ip = IPAddress.Parse("127.0.0.1"); // manually enter IP address (new, unique server IP)

        // writes the message
        if (printMessages)
        {
            Console.WriteLine("Server name: {0} IP: {1}", host.HostName, ip);
            // Debug.LogAssertion("Server name: " + host.HostName + " IP: " + ip);
        }

        // using the same port that was used last class.
        IPEndPoint localEP = new IPEndPoint(ip, server_port);

        // last class the family was entered, but you can get from the ip directly the Address family.
        server_socket = new Socket(ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

        // 0 for any available port.
        client = new IPEndPoint(IPAddress.Any, 0); // 0 for any available port.
        remoteClient = (EndPoint)client;

        // binds server and EP
        try
        {
            // the server listens and provides a service.
            server_socket.Bind(localEP);

            // Console.WriteLine("Waiting for data...");
            Debug.LogAssertion("Waiting for Data...");

            // sets timeout variables.
            server_socket.ReceiveTimeout = receiveTimeout;
            server_socket.SendTimeout = sendTimeout;

            // non-blocking
            server_socket.Blocking = false;

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    // gets the buffer size in bytes
    public int GetBufferSize()
    {
        return buffer.Length;
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
            if(printMessages)
            {
                Console.WriteLine("Received: {0} from Client: {1}", Encoding.ASCII.GetString(buffer, 0, rec), remoteClient.ToString());
                // Debug.LogAssertion("Received: " + Encoding.ASCII.GetString(buffer, 0, rec) + " from Client: " + remoteClient.ToString());
            }
            

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
