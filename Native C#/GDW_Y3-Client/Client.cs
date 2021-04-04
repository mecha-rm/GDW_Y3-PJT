// References:
// * https://docs.microsoft.com/en-us/dotnet/api/system.net.iphostentry.addresslist?view=net-5.0
// * https://stackoverflow.com/questions/6803073/get-local-ip-address
// * https://stackoverflow.com/questions/11411486/how-to-get-ipv4-and-ipv6-address-of-local-machine

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace NetworkLibrary
{
    // server class
    public abstract class Client
    {
        // gets local host for ipv4 (ip: 127.0.0.1)
        public static IPAddress LocalHostIPv4
        {
            get
            {
                return IPAddress.Loopback;
            }
        }

        // gets local host for ipv6 (ip: 0:0:0:0:0:0:0:1) ("::1" for short notation)
        public static IPAddress LocalHostIPv6
        {
            get
            {
                return IPAddress.IPv6Loopback;
            }
        }

        // gets any ip address value for ipv4 (ip: 0.0.0.0)
        public static IPAddress AnyIPv4
        {
            get
            {

                return IPAddress.Any;
            }
        }

        // gets any ip address value for ipv6 (ip: 0000:0000:0000:0000:0000:0000:0000:0000) ("::" for short notation) 
        public static IPAddress AnyIPv6
        {
            get
            {
                return IPAddress.IPv6Any;
            }
        }

        // gets the local ip address
        public static IPAddress GetLocalIPv4Address()
        {
            // host
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            // looks for ipv4
            foreach (IPAddress ip in host.AddressList)
            {
                // if the ip address has been found.
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }

            // not found.
            Console.WriteLine("IPv4 not found.");
            return null;
        }

        // gets local ip v4 address as a string.
        public static string GetLocalIPv4AddressAsString()
        {
            // gets ip address.
            IPAddress ip = GetLocalIPv4Address();

            // if the ip address was foun.
            if (ip == null)
                return null;
            else
                return ip.ToString();
        }

        // gets the local ip address
        public static IPAddress GetLocalIPv6Address()
        {
            // host
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            // looks for ipv4
            foreach (IPAddress ip in host.AddressList)
            {
                // if the ip address has been found.
                if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    return ip;
                }
            }

            // not found.
            Console.WriteLine("IPv6 not found.");
            return null;
        }

        // gets local ip v6 address as a string.
        public static string GetLocalIPv6AddressAsString()
        {
            // gets ip address.
            IPAddress ip = GetLocalIPv6Address();

            // if the ip address was found.
            if (ip == null)
                return null;
            else
                return ip.ToString();
        }

        // runs the server
        public abstract void RunClient();

        // updates the server
        public abstract void Update();

        // shuts down the server.
        public abstract void ShutdownClient();
    }
}

