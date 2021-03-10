using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLibrary
{
    // client interface
    class ClientInterface
    {
        // gets the server
        private static GDW_Y3_Client.UdpClient client = new GDW_Y3_Client.UdpClient();

        // gets the buffer size.
        public static int GetBufferSize()
        {
            return client.GetSendBufferSize();
        }

        // sets the buffer size.
        public static void SetBufferSize(int size)
        {
            client.SetSendBufferSize(size);
        }

        // gets the buffer data
        public static byte[] GetBufferData()
        {
            return client.GetSendBufferData();
        }

        // gets the ip address as a string
        public static String GetIPAddress()
        {
            return client.GetIPAddress();
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
