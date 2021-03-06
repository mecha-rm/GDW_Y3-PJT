﻿/*
 * Team Outkasts
 * Description: client libraries
 * References:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDW_Y3_Client
{
    class Program
    {
        // udp client test
        static void UdpClientTest(bool twoWay)
        {
            NetworkLibrary.UdpClient client = new NetworkLibrary.UdpClient();

            // NOTE: the client sending data only does not work
            // I don't know if I'll bother to fix it though.
            // Also, setting up delays or non-blocking sockets before the client is running causes it to crash.


            // communication mode is send
            if (twoWay)
                client.SetCommunicationMode(NetworkLibrary.UdpClient.mode.both);
            else
                client.SetCommunicationMode(NetworkLibrary.UdpClient.mode.send); // send by default

            // set the sockets to block
            client.SetBlockingSockets(false);

            // ignore this error
            client.ignoreError10035 = true;

            // runs the client
            client.RunClient();

            // while the clent is running
            while (client.IsRunning())
            {
                Console.WriteLine("Enter Message: ");
                string str = Console.ReadLine();

                byte[] data = Encoding.ASCII.GetBytes(str);
                client.SetSendBufferData(data);

                client.Update();

                // if doing two way
                if (twoWay)
                {
                    data = client.GetReceiveBufferData();
                    Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
                }
            }

            client.ShutdownClient();
        }

        // tcp client test
        static void TcpClientSyncTest(bool twoWay)
        {
            NetworkLibrary.TcpClientSync client = new NetworkLibrary.TcpClientSync();

            // most recent string provided.
            string recentString = "";

            // if 'true', repeat messages are printed.
            bool repeatMessages = true;

            // NOTE: the client sending data only does not work
            // I don't know if I'll bother to fix it though.
            // Also, setting up delays or non-blocking sockets before the client is running causes it to crash.


            // communication mode is send
            if (twoWay)
                client.SetCommunicationMode(NetworkLibrary.TcpClientSync.mode.both);
            else
                client.SetCommunicationMode(NetworkLibrary.TcpClientSync.mode.send); // send by default

            client.SetBlockingSockets(false);

            // runs the client
            client.RunClient();

            // while the clent is running
            while (client.IsRunning())
            {
                Console.WriteLine("Enter Message: ");
                string str = Console.ReadLine();

                byte[] data = Encoding.ASCII.GetBytes(str);
                client.SetSendBufferData(data);

                client.Update();

                // if doing two way
                if (twoWay)
                {
                    data = client.GetReceiveBufferData();

                    string receivedString = Encoding.ASCII.GetString(data, 0, data.Length);

                    if (data.Length > 0 && (receivedString != recentString || repeatMessages))
                    {
                        recentString = receivedString;
                        Console.WriteLine(receivedString);
                    }

                    // Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
                }
            }

            client.ShutdownClient();
        }

        // tests tcp client
        static void TcpClientAsyncTest(bool twoWay)
        {
            NetworkLibrary.TcpClientAsync client = new NetworkLibrary.TcpClientAsync();

            // most recent string provided.
            string recentString = "";

            // if 'true', repeat messages are printed.
            bool repeatMessages = true;

            // NOTE: the client sending data only does not work
            // I don't know if I'll bother to fix it though.
            // Also, setting up delays or non-blocking sockets before the client is running causes it to crash.


            // communication mode is send
            if (twoWay)
                client.SetCommunicationMode(NetworkLibrary.TcpClientAsync.mode.both);
            else
                client.SetCommunicationMode(NetworkLibrary.TcpClientAsync.mode.send); // send by default

            // runs the client
            client.RunClient();

            // while the clent is running
            while (client.IsRunning())
            {
                Console.WriteLine("Enter Message: ");
                string str = Console.ReadLine();

                byte[] data = Encoding.ASCII.GetBytes(str);
                client.SetSendBufferData(data);

                client.Update();

                // if doing two way
                if (twoWay)
                {
                    data = client.GetReceiveBufferData();
                    
                    string receivedString = Encoding.ASCII.GetString(data, 0, data.Length);

                    if (data.Length > 0 && (receivedString != recentString || repeatMessages))
                    {
                        recentString = receivedString;
                        Console.WriteLine(receivedString);
                    }

                    // Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
                }
            }

            client.ShutdownClient();
        }

        // tcp client x test (with non-blocking)
        static void TcpClientSyncXTest()
        {
            NetworkLibrary.TcpClientSyncX client = new NetworkLibrary.TcpClientSyncX();

            // most recent string provided.
            string recentString = "";

            // if 'true', repeat messages are printed.
            bool repeatMessages = true;

            // NOTE: the client sending data only does not work
            // I don't know if I'll bother to fix it though.
            // Also, setting up delays or non-blocking sockets before the client is running causes it to crash.
            client.SetBlockingSockets(false);
            // client.connectTimeout = 5000000;
            client.SetConnectTimeoutInSeconds(2);

            // runs the client
            client.RunClient();

            // while the clent is running
            while (client.IsRunning())
            {
                Console.WriteLine("Enter Message: ");
                string str = Console.ReadLine();

                byte[] data = Encoding.ASCII.GetBytes(str);
                client.SetSendBufferData(data);

                // send and receive data
                client.Update();

                data = client.GetReceiveBufferData();

                // receives data
                string receivedString = Encoding.ASCII.GetString(data, 0, data.Length);

                if (data.Length > 0 && (receivedString != recentString || repeatMessages))
                {
                    recentString = receivedString;
                    Console.WriteLine(receivedString);
                }

                // Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
            }

            client.ShutdownClient();
        }

        // main function - uncomment if making DLL
        static void Main(string[] args)
        {
            // test mode
            int testMode = 1;

            switch (testMode)
            {
                default:
                case 0: // client (1 way)
                case 1:
                    UdpClientTest(true);
                    break;

                case 2: // tcp synchronous client
                    TcpClientSyncTest(true);
                    break;

                case 3: // tcp client
                    TcpClientAsyncTest(true);
                    break;
                
                case 4: // tcp syncronous client (non-blocking)
                    TcpClientSyncXTest();
                    break;
            }

        }
    }
}
