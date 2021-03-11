using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDW_Y3_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworkLibrary.UdpClient client = new NetworkLibrary.UdpClient();
            bool twoWay = true; // two-way connection


            // NOTE: the client sending data only does not work
            // I don't know if I'll bother to fix it though.
            // Also, setting up delays or non-blocking sockets before the client is running causes it to crash.


            // communication mode is send
            if (twoWay)
                client.SetCommunicationMode(NetworkLibrary.UdpClient.mode.both);
            else
                client.SetCommunicationMode(NetworkLibrary.UdpClient.mode.send); // send by default

            // runs the client
            client.RunClient();

            // while the clent is running
            while(client.IsRunning())
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
    }
}
