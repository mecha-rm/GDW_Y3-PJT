using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLibrary
{
    // client interface
    public static class UdpClientInterface
    {
        // gets the server
        private static UdpClient client = new UdpClient();

        // gets the communication mode
        public static UdpClient.mode GetCommunicationMode()
        {
            return client.GetCommunicationMode();
        }

        // sets the communication mode
        public static void SetCommunicationMode(UdpClient.mode newMode)
        {
            client.SetCommunicationMode(newMode);
        }

        // get the default buffer size
        public static int GetDefaultBufferSize()
        {
            return client.GetDefaultBufferSize();
        }

        // sets the default buffer size
        public static void SetDefaultBufferSize(int newSize)
        {
            client.SetDefaultBufferSize(newSize);
        }

        // gets the buffer size.
        public static int GetSendBufferSize()
        {
            return client.GetSendBufferSize();
        }

        // sets the buffer size.
        public static void SetSendBufferSize(int size)
        {
            client.SetSendBufferSize(size);
        }

        // gets teh receive buffer size
        public static int GetReceiveBufferSize()
        {
            return client.GetReceiveBufferSize();
        }

        // sets the receive buffer size
        public static void SetReceiveBufferSize(int size)
        {
            client.SetReceiveBufferSize(size);
        }

        // gets the buffer data
        public static byte[] GetSendBufferData()
        {
            return client.GetSendBufferData();
        }

        // sets the receive buffer data
        public static void SetSendBufferData(byte[] data, bool deleteOldData = false)
        {
            client.SetSendBufferData(data, deleteOldData);
        }

        // gets the receive buffer data
        public static byte[] GetReceiveBufferData()
        {
            return client.GetReceiveBufferData();
        }

        // sets the receive buffer data
        // if 'deleteOldData' is set to 'true', then the original data is not deleted.
        public static void SetReceiveBufferData(byte[] data, bool deleteOldData = false)
        {
            client.SetReceiveBufferData(data, deleteOldData);
        }

        // gets the ip address as a string
        public static string GetIPAddress()
        {
            return client.GetIPAddress();
        }

        // sets the IP address
        public static void SetIPAdress(string ipAdd)
        {
            client.SetIPAdress(ipAdd);
        }

        // gets the port number
        public static int GetPort()
        {
            return client.GetPort();
        }

        // sets the port number
        // this cannot be changed while a server is running
        public static void SetPort(int newPort)
        {
            client.SetPort(newPort);
        }

        // checks to see if the server is blocking sockets
        public static bool IsBlockingSockets()
        {
            return client.IsBlockingSockets();
        }

        // sets the blocking sockets variable
        public static void SetBlockingSockets(bool blocking)
        {
            client.SetBlockingSockets(blocking);
        }

        // getter for the send timeout
        public static int GetSendTimeout()
        {
            return client.GetSendTimeout();
        }

        // setter for send tiemout
        public static void SetSendTimeout(int newSt)
        {
            client.SetSendTimeout(newSt);
        }

        // gets the receiver timeout.
        public static int GetReceiverTimeout()
        {
            return client.GetReceiverTimeout();
        }

        // sets the receiver timeout.
        public static void SetReceiverTimeout(int newRt)
        {
            client.SetReceiverTimeout(newRt);
        }

        // checks to see if the server is running
        public static bool IsRunning()
        {
            return client.IsRunning();
        }

        // checks to see if the server is connected.
        public static bool IsConnected()
        {
            return client.IsConnected();
        }

        // run the server project.
        public static void RunClient()
        {
            client.RunClient();
        }

        // updates server
        public static void Update()
        {
            client.Update();
        }

        // shuts down the server.
        public static void ShutdownClient()
        {
            client.ShutdownClient();
        }
    }
}
