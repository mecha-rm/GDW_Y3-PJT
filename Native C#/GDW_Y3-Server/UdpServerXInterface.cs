using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace NetworkLibrary
{
    // server interface
    public static class UdpServerXInterface
    {
        // gets the server
        private static UdpServerX server = new UdpServerX();

        // gets the server name
        public static string GetServerName()
        {
            return server.GetServerName();
        }

        // get the default buffer size
        public static int GetDefaultBufferSize()
        {
            return server.GetDefaultBufferSize();
        }

        // sets the default buffer size
        public static void SetDefaultBufferSize(int newSize)
        {
            server.SetDefaultBufferSize(newSize);
        }

        // SEND DATA //
        // returns the buffer size.
        public static int GetSendBufferSize()
        {
            return server.GetSendBufferSize();
        }

        // sets the buffer size for the server
        public static void SetSendBufferSize(int size)
        {
            server.SetSendBufferSize(size);
        }


        // RECEIVE DATA //
        // gets size of buffer at provided index
        public static int GetReceiveBufferSize(int index)
        {
            return server.GetReceiveBufferSize(index);
        }


        // sets the buffer size for the server
        public static void SetReceiveBufferSize(int index, int size)
        {
            server.SetReceiveBufferSize(index, size);
        }

        // ADDERS AND REMOVERS FOR CLIENTS

        // Adding and Removing Clients
        public static EndPoint GetRemoteClient(int index)
        {
            return server.GetEndPoint(index);
        }

        // adds a remote client with the default buffer size
        public static byte[] AddRemoteClient()
        {
            return server.AddEndPoint();
        }

        // adds a remote client with a buffer size
        // if the buffer size is negative, then the default size is set.
        public static byte[] AddRemoteClient(int bufferSize)
        {
            return server.AddEndPoint(bufferSize);
        }

        // adds a remote client with a buffer
        // this returns the buffer that was just added
        public static byte[] AddRemoteClient(int bufferSize, byte[] buffer)
        {
            return server.AddEndPoint(bufferSize, buffer);
        }

        // removes a remote client and returns its buffer.
        public static byte[] RemoteRemoteClient(int index)
        {
            return server.RemoteEndPoint(index);
        }

        // deletes the remote client
        public static void DeleteRemoteClient(int index)
        {
            server.DeleteEndPoint(index);
        }


        // SETTER AND GETTER FOR BUFFER DATA
        // gets the send buffer data
        public static byte[] GetSendBufferData()
        {
            return server.GetSendBufferData();
        }

        // sets the receive buffer data
        public static void SetSendBufferData(byte[] data, bool deleteOldData = false)
        {
            server.SetSendBufferData(data, deleteOldData);
        }

        // gets receive buffer data
        public static byte[] GetReceiveBufferData(int index)
        {
            return server.GetReceiveBufferData(index);
        }

        // sets the receive buffer data
        public static void SetReceiveBufferData(int index, byte[] data, bool deleteOldData = false)
        {
            server.SetReceiveBufferData(index, data, deleteOldData);
        }

        // gets the ip address as a string
        public static string GetIPAddress()
        {
            return server.GetIPAddress();
        }

        // sets the IP address
        public static void SetIPAdress(string ipAdd)
        {
            server.SetIPAdress(ipAdd);
        }

        // gets the port number
        public static int GetPort()
        {
            return server.GetPort();
        }

        // sets the port number
        // this cannot be changed while a server is running
        public static void SetPort(int newPort)
        {
            server.SetPort(newPort);
        }

        // checks to see if the server is blocking sockets
        public static bool IsBlockingSockets()
        {
            return server.IsBlockingSockets();
        }

        // sets the blocking sockets variable
        public static void SetBlockingSockets(bool blocking)
        {
            server.SetBlockingSockets(blocking);
        }

        // getter for the send timeout
        public static int GetSendTimeout()
        {
            return server.GetSendTimeout();
        }

        // setter for send tiemout
        public static void SetSendTimeout(int newSt)
        {
            server.SetSendTimeout(newSt);
        }

        // gets the receiver timeout.
        public static int GetReceiveTimeout()
        {
            return server.GetReceiveTimeout();
        }

        // sets the receiver timeout.
        public static void SetReceiveTimeout(int newRt)
        {
            server.SetReceiveTimeout(newRt);
        }

        // checks to see if the server is running
        public static bool IsRunning()
        {
            return server.IsRunning();
        }

        // checks to see if the server is connected.
        public static bool IsConnected()
        {
            return server.IsConnected();
        }

        // run the server project.
        public static void RunServer()
        {
            server.RunServer();
        }

        // updates server
        public static void Update()
        {
            server.Update();
        }

        // shuts down the server.
        public static void ShutdownServer()
        {
            server.ShutdownServer();
        }
    }
}
