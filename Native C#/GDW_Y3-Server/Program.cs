using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDW_Y3_Server
{
    class Program
    {
        // uncomment if making DLL
        static void Main(string[] args)
        {
            UdpServer server = new UdpServer();
            bool twoWay = true; // two-way connection


            // NOTE: the server sending data only does not work
            // I don't know if I'll bother to fix it though.
            // Also, setting up delays or non-blocking sockets before the server is running causes it to crash.


            // two way mode
            if(twoWay)
                server.SetCommunicationMode(UdpServer.mode.both);
            else
                server.SetCommunicationMode(UdpServer.mode.receive); // receive by default


            // runs the serve
            server.RunServer();

            // Console.WriteLine(server.GetIPAddress());


            // while loop for updates
            while(server.IsRunning())
            {
                // if doing two way 
                if(twoWay)
                {
                    Console.WriteLine("Enter Message: ");
                    string str = Console.ReadLine();
                    byte[] sendData = Encoding.ASCII.GetBytes(str); ;
                    server.SetSendBufferData(sendData);
                }
                
                server.Update();

                byte[] data = server.GetReceiveBufferData();

                if(data.Length > 0)
                    Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
            }
        
            server.ShutdownServer();
        
        }
    }
}
