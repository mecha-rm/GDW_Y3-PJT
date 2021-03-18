﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDW_Y3_Server
{
    class Program
    {
        // main server test
        static void UdpServerTest(bool twoWay)
        {
            NetworkLibrary.UdpServer server = new NetworkLibrary.UdpServer();


            // NOTE: the server sending data only does not work
            // I don't know if I'll bother to fix it though.
            // Also, setting up delays or non-blocking sockets before the server is running causes it to crash.

            // two way mode
            if (twoWay)
                server.SetCommunicationMode(NetworkLibrary.UdpServer.mode.both);
            else
                server.SetCommunicationMode(NetworkLibrary.UdpServer.mode.receive); // receive by default


            server.SetBlockingSockets(false);

            // runs the server
            server.RunServer();

            // Console.WriteLine(server.GetIPAddress());

            // while loop for updates
            while (server.IsRunning())
            {
                // if doing two way 
                if (twoWay)
                {
                    Console.WriteLine("Enter Message: ");
                    string str = Console.ReadLine();
                    byte[] sendData = Encoding.ASCII.GetBytes(str);
                    server.SetSendBufferData(sendData);
                }


                server.Update();

                byte[] data = server.GetReceiveBufferData();

                if (data.Length > 0)
                    Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
            }

            server.ShutdownServer();
        }

        // 1 to 4 Server Test
        static void UdpServer4Test()
        {
            NetworkLibrary.UdpServer4 server4 = new NetworkLibrary.UdpServer4();

            // runs the serve
            server4.RunServer();

            // Console.WriteLine(server.GetIPAddress());

            // while loop for updates
            while (server4.IsRunning())
            {
                Console.WriteLine("Enter Message: ");
                string str = Console.ReadLine();
                byte[] sendData = Encoding.ASCII.GetBytes(str);
                server4.SetSendBufferData(sendData);

                server4.SetBlockingSockets(false);
                server4.Update();
                
                
                // server4.SetReceiveTimeout(1);
                // server4.SetSendTimeout(1);

                byte[] data; 
                
                // buffer 1
                data = server4.GetReceiveBuffer1Data();

                if (data != null && data.Length > 0)
                    Console.WriteLine("Buffer 1: " + Encoding.ASCII.GetString(data, 0, data.Length));

                // buffer 2
                data = server4.GetReceiveBuffer2Data();

                if (data != null && data.Length > 0)
                    Console.WriteLine("Buffer 2: " + Encoding.ASCII.GetString(data, 0, data.Length));

                // buffer 3
                data = server4.GetReceiveBuffer3Data();

                if (data != null && data.Length > 0)
                    Console.WriteLine("Buffer 3: " + Encoding.ASCII.GetString(data, 0, data.Length));

                // buffer 4
                data = server4.GetReceiveBuffer4Data();

                if (data != null && data.Length > 0)
                    Console.WriteLine("Buffer 4: " + Encoding.ASCII.GetString(data, 0, data.Length));
            }

            server4.ShutdownServer();
        }

        // public void ServerXTest() {}
        static void TcpServerSyncTest(bool twoWay)
        {
            NetworkLibrary.TcpServerSync server = new NetworkLibrary.TcpServerSync();

            // last received string
            string recentString = "";

            // if 'true', repeat messages are printed.
            bool repeatMessages = true;

            // two way mode
            if (twoWay)
                server.SetCommunicationMode(NetworkLibrary.TcpServerSync.mode.both);
            else
                server.SetCommunicationMode(NetworkLibrary.TcpServerSync.mode.receive); // receive by default

            // blocking sockets
            server.SetBlockingSockets(true);
            server.ignoreError10035 = true;

            // runs the server
            server.RunServer();

            // while loop for updates
            while (server.IsRunning())
            {
                // if doing two way 
                if (twoWay)
                {
                    Console.WriteLine("Enter Message: ");
                    string str = Console.ReadLine();
                    byte[] sendData = Encoding.ASCII.GetBytes(str);
                    server.SetSendBufferData(sendData);
                }

                server.Update();

                byte[] data = server.GetReceiveBufferData();
                string receivedString = Encoding.ASCII.GetString(data, 0, data.Length);

                // new string has been provided.
                if (data.Length > 0 && (receivedString != recentString || repeatMessages))
                {
                    recentString = receivedString;
                    Console.WriteLine(receivedString);
                }

                // Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
            }

            server.ShutdownServer();
        }


        // testing out the TCP Server
        static void TcpServerAsyncTest(bool twoWay)
        {
            NetworkLibrary.TcpServerAsync server = new NetworkLibrary.TcpServerAsync();

            // last received string
            string recentString = "";

            // if 'true', repeat messages are printed.
            bool repeatMessages = true;

            // two way mode
            if (twoWay)
                server.SetCommunicationMode(NetworkLibrary.TcpServerAsync.mode.both);
            else
                server.SetCommunicationMode(NetworkLibrary.TcpServerAsync.mode.receive); // receive by default

            // runs the server
            server.RunServer();

            // while loop for updates
            while (server.IsRunning())
            {
                // if doing two way 
                if (twoWay)
                {
                    Console.WriteLine("Enter Message: ");
                    string str = Console.ReadLine();
                    byte[] sendData = Encoding.ASCII.GetBytes(str);
                    server.SetSendBufferData(sendData);
                }

                server.Update();

                byte[] data = server.GetReceiveBufferData();
                string receivedString = Encoding.ASCII.GetString(data, 0, data.Length);

                // new string has been provided.
                if (data.Length > 0 && (receivedString != recentString || repeatMessages))
                {
                    recentString = receivedString;
                    Console.WriteLine(receivedString);
                }

                // Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
            }

            server.ShutdownServer();
        }

        // uncomment if making DLL
        static void Main(string[] args)
        {
            // test mode
            int testMode = 4;

            switch(testMode)
            {
                default:
                case 0: // server (1 way)
                case 1:
                    UdpServerTest(true);
                    break;

                case 2: // server (4 way)
                    UdpServer4Test();
                    break;

                case 3:
                    TcpServerSyncTest(true);
                    break;

                case 4:
                    TcpServerAsyncTest(true);
                    break;
            }
        }
    }
}
