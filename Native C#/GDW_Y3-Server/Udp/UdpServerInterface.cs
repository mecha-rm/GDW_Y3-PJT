using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace NetworkLibrary
{
    public static class UdpServerInterface
    {
        // gets the server
        private static UdpServer server = new UdpServer();


        // gets the server name
        public static string GetServerName()
        {
            return server.GetServerName();
        }

        // gets the communication mode
        public static UdpServer.mode GetCommunicationMode()
        {
            return server.GetCommunicationMode();
        }

        // sets the communication mode
        public static void SetCommunicationMode(UdpServer.mode newMode)
        {
            server.SetCommunicationMode(newMode);
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

        // returns the buffer size.
        public static int GetReceiveBufferSize()
        {
            return server.GetReceiveBufferSize();
        }

        // sets the buffer size for the server
        public static void SetReceiveBufferSize(int size)
        {
            server.SetReceiveBufferSize(size);
        }

        // gets the send buffer data
        public static byte[] GetSendBufferData()
        {
            return server.GetSendBufferData();
        }

        // sets the receive buffer data
        public static void SetSendBufferData(byte[] data, bool deleteOldData = false)
        {
            server.SetSendBufferData(data,deleteOldData);
        }

        // gets the receive buffer data
        public static byte[] GetReceiveBufferData()
        {
            return server.GetReceiveBufferData();
        }

        // sets the receive buffer data
        public static void SetReceiveBufferData(byte[] data, bool deleteOldData = false)
        {
            server.SetReceiveBufferData(data, deleteOldData);
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
