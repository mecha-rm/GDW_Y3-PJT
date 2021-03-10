using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace NetworkLibrary
{
    public class ServerInterface
    {
        // gets the server
        private static GDW_Y3_Server.UdpServer server = new GDW_Y3_Server.UdpServer();

        // gets the communication mode
        public GDW_Y3_Server.UdpServer.mode GetCommunicationMode()
        {
            return server.GetCommunicationMode();
        }

        // sets the communication mode
        public void SetCommunicationMode(GDW_Y3_Server.UdpServer.mode newMode)
        {
            server.SetCommunicationMode(newMode);
        }

        // returns the buffer size.
        public int GetSendBufferSize()
        {
            return server.GetSendBufferSize();
        }

        // sets the buffer size for the server
        public void SetSendBufferSize(int size)
        {
            server.SetSendBufferSize(size);
        }

        // returns the buffer size.
        public int GetReceiveBufferSize()
        {
            return server.GetReceiveBufferSize();
        }

        // sets the buffer size for the server
        public void SetReceiveBufferSize(int size)
        {
            server.SetReceiveBufferSize(size);
        }

        // gets the send buffer data
        public byte[] GetSendBufferData()
        {
            return server.GetSendBufferData();
        }

        // sets the receive buffer data
        public void SetSendBufferData(byte[] data)
        {
            server.SetSendBufferData(data);
        }

        // gets the receive buffer data
        public byte[] GetReceiveBufferData()
        {
            return server.GetReceiveBufferData();
        }

        // gets the ip address as a string
        public static String GetIPAddress()
        {
            return server.GetIPAddress();
        }

        // sets the IP address
        public void SetIPAdress(String ipAdd)
        {
            server.SetIPAdress(ipAdd);
        }

        // gets the port number
        public int GetPort()
        {
            return server.GetPort();
        }

        // sets the port number
        // this cannot be changed while a server is running
        public void SetPort(int newPort)
        {
            server.SetPort(newPort);
        }

        // checks to see if the server is blocking sockets
        public bool IsBlockingSockets()
        {
            return server.IsBlockingSockets();
        }

        // sets the blocking sockets variable
        public void SetBlockingSockets(bool blocking)
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
