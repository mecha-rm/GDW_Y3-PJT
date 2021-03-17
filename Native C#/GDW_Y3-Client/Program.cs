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
                    Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
                }
            }

            client.ShutdownClient();
        }

        // tests tcp client
        static void TcpClientTest(bool twoWay)
        {
            NetworkLibrary.TcpClient client = new NetworkLibrary.TcpClient();

            // most recent string provided.
            string recentString = "";

            // if 'true', repeat messages are printed.
            bool repeatMessages = true;

            // NOTE: the client sending data only does not work
            // I don't know if I'll bother to fix it though.
            // Also, setting up delays or non-blocking sockets before the client is running causes it to crash.


            // communication mode is send
            if (twoWay)
                client.SetCommunicationMode(NetworkLibrary.TcpClient.mode.both);
            else
                client.SetCommunicationMode(NetworkLibrary.TcpClient.mode.send); // send by default

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

        static void Main(string[] args)
        {
            // test mode
            int testMode = 2;

            switch (testMode)
            {
                default:
                case 0: // server (1 way)
                case 1:
                    UdpClientTest(true);
                    break;

                case 2: // server (4 way)
                    TcpClientTest(true);
                    break;
            }

        }
    }
}
