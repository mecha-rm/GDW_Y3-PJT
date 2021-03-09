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
            server.RunServer();

            // Console.WriteLine(server.GetIPAddress());

            // while loop for updates
            while(server.IsRunning())
            {
                server.Update();
                byte[] data = server.GetBufferData();
                Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
            }
        
            server.ShutdownServer();
        
        }
    }
}
