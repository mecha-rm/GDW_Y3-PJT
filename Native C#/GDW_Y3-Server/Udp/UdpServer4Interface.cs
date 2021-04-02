using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLibrary
{
    public static class UdpServer4Interface
    {
        // gets the server
        private static UdpServer4 server = new UdpServer4();

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
        public static int GetReceiveBuffer1Size()
        {
            return server.GetReceiveBuffer1Size();
        }

        // sets the buffer size for the server
        public static void SetReceive2BufferSize(int size)
        {
            server.SetReceiveBuffer2Size(size);
        }

        // gets the size of the second buffer
        public static int GetReceiveBuffer2Size()
        {
            return server.GetReceiveBuffer2Size();
        }

        // sets the buffer size for the server
        public static void SetReceiveBuffer2Size(int size)
        {
            server.SetReceiveBuffer2Size(size);
        }

        // gets the size of the third buffer
        public static int GetReceiveBuffer3Size()
        {
            return server.GetReceiveBuffer3Size();
        }

        // sets the buffer size for the server
        public static void SetReceiveBuffer3Size(int size)
        {
            server.SetReceiveBuffer3Size(size);
        }

        // get receive buffer 4
        public static int GetReceiveBuffer4Size()
        {
            return server.GetReceiveBuffer4Size();
        }

        // sets the buffer 4 size for the server
        public static void SetReceiveBuffer4Size(int size)
        {
            server.SetReceiveBuffer4Size(size);
        }

        // gets the send buffer data
        public static byte[] GetSendBufferData()
        {
            return server.GetSendBufferData();
        }

        // sets the receive buffer data
        // if 'deleteOldData' is set to 'true', the old data is deleted.
        public static void SetSendBufferData(byte[] data, bool deleteOldData = false)
        {
            server.SetSendBufferData(data, deleteOldData);
        }

        // gets the receive buffer 1 data
        public static byte[] GetReceiveBuffer1Data()
        {
            return server.GetReceiveBuffer1Data();
        }

        // sets the receive buffer data
        public static void SetReceiveBuffer1Data(byte[] data, bool deleteOldData = false)
        {
            server.SetReceiveBuffer1Data(data, deleteOldData);
        }

        // gets the receive buffer 2 data
        public static byte[] GetReceiveBuffer2Data()
        {
            return server.GetReceiveBuffer2Data();
        }

        // sets the receive buffer data
        public static void SetReceiveBuffer2Data(byte[] data, bool deleteOldData = false)
        {
            server.SetReceiveBuffer2Data(data, deleteOldData);
        }

        // gets the receive buffer 3 data
        public static byte[] GetReceiveBuffer3Data()
        {
            return server.GetReceiveBuffer3Data();
        }

        // sets the receive buffer data
        public static void SetReceiveBuffer3Data(byte[] data, bool deleteOldData = false)
        {
            server.SetReceiveBuffer3Data(data, deleteOldData);
        }

        // gets the receive buffer 4 data
        public static byte[] GetReceiveBuffer4Data()
        {
            return server.GetReceiveBuffer4Data();
        }

        // sets the receive buffer data
        public static void SetReceiveBuffer4Data(byte[] data, bool deleteOldData = false)
        {
            server.SetReceiveBuffer4Data(data, deleteOldData);
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
