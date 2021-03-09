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
            UdpClient client = new UdpClient();
            client.RunClient();

            while(client.IsRunning())
            {

            }

            Encoding.ASCII.GetString(buffer, 0, rec);
        }
    }
}
