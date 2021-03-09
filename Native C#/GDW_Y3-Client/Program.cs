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

            // while the clent is running
            while(client.IsRunning())
            {
                Console.WriteLine("Enter Message: ");
                string str = Console.ReadLine();

                byte[] data = Encoding.ASCII.GetBytes(str);
                client.SetBufferData(data);

                client.Update();
            }

            client.ShutdownClient();
            
        }
    }
}
