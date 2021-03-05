﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDW_Y3_Network
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpServer server = new UdpServer();
            server.RunServer();
            
            // while loop for updates
            while(server.IsRunning())
            {
                server.Update();
            }

            server.ShutdownServer();

        }
    }
}
