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

        // gets the buffer size.
        public static int GetBufferSize()
        {
            return server.GetBufferSize();
        }

        // sets the buffer size.
        public static void SetBufferSize(int size)
        {
            server.SetBufferSize(size);
        }

        // gets the buffer data
        public static byte[] GetBufferData()
        {
            return server.GetBufferData();
        }

        // gets the ip address as a string
        public static String GetIPAddress()
        {
            return server.GetIPAddress();
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
        public static int GetReceiverTimeout()
        {
            return server.GetReceiverTimeout();
        }

        // sets the receiver timeout.
        public static void SetReceiverTimeout(int newRt)
        {
            server.SetReceiverTimeout(newRt);
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
