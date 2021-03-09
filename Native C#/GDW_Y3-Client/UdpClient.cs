using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace GDW_Y3_Client
{
    public class UdpClient
    {
        // client variables
        private byte[] buffer;
        private IPAddress ip;

        private IPEndPoint remote = null;
        private Socket client_socket = null;

        // the ip address for the object.
        string ipAddress = "";

        // port
        // private int port;

        // if 'true', sockets are being blocked.
        private bool blockingSockets = false;

        // timeout variables
        private int receiveTimeout = 0, sendTimeout = 0;

        // checks to see if the server is running
        private bool running = false;

        // constructor
        public UdpClient()
        {

        }

        // returns the buffer size.
        public int GetBufferSize()
        {
            if (buffer != null)
                return buffer.Length;
            else
                return 0;
        }

        // sets the buffer size for the server
        public void SetBufferSize(int size)
        {
            if (size > 0)
                buffer = new byte[size];
        }

        // gets the buffer data
        public byte[] GetBufferData()
        {
            return buffer;
        }

        // sets the buffer data
        public void SetBufferData(byte[] data)
        {
            buffer = data;
        }

        // adds the buffer data
        public void AddBufferData(byte[] newData)
        {
            // creates the new array
            byte[] arr = new byte[buffer.Length + newData.Length];

            Buffer.BlockCopy(buffer, 0, arr, 0, buffer.Length);
            Buffer.BlockCopy(newData, 0, arr, buffer.Length, newData.Length);

            // sets array of data as new buffer
            Array.Clear(buffer, 0, buffer.Length);
            buffer = arr;
        }

        // gets the ip address as a string.
        public String GetIPAddress()
        {
            if (ip != null) // references ip object
                return ip.ToString();
            else // if the ip object has not been made yet, reference the string.
                return ipAddress;
        }

        // sets the IP address
        public void SetIPAdress(String ipAdd)
        {
            ip = IPAddress.Parse(ipAdd); // set server's ip address
            remote = new IPEndPoint(ip, 11111); // create remote with port
            ipAddress = ipAdd;
        }

        // checks to see if the server is blocking sockets
        public bool IsBlockingSockets()
        {
            // if the server socket exists, that variable is referenced.
            // if not, the class variable is used.
            if (client_socket != null)
                return client_socket.Blocking;
            else
                return blockingSockets;
        }

        // sets the blocking sockets variable
        public void SetBlockingSockets(bool blocking)
        {
            // sets variable
            blockingSockets = blocking;

            if (client_socket != null)
                client_socket.Blocking = blockingSockets;
        }

        // getter for send timeout
        public int GetSendTimeout()
        {
            return sendTimeout;
        }

        // setter for send timeout
        public void SetSendTimeout(int newSt)
        {
            sendTimeout = newSt;

            // if the server socket has been generated.
            if (client_socket != null)
                client_socket.SendTimeout = sendTimeout;
        }

        // gets the receiver timeout.
        public int GetReceiverTimeout()
        {
            return receiveTimeout;
        }

        // sets the receiver timeout.
        public void SetReceiverTimeout(int newRt)
        {
            receiveTimeout = newRt;

            // if the server socket has been generated.
            if (client_socket != null)
                client_socket.ReceiveTimeout = receiveTimeout;
        }

        // returns 'true' if the server is running
        public bool IsRunning()
        {
            if (client_socket != null)
            {
                return running;
            }

            return false;
        }

        // returns 'true' if the server is running
        public bool IsConnected()
        {
            if (client_socket != null)
            {
                return client_socket.Connected;
            }

            return false;
        }

        // runs the client
        public void RunClient()
        {
            if (buffer == null)
                buffer = new byte[512];

            try
            {
                // uses the localhost
                if (ipAddress == null || ipAddress == "")
                {
                    // ipAddress = "127.0.0.1"; // local host

                    // grab IP address of system
                    IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                    ipAddress = host.AddressList[1].ToString(); // get IP address from list
                }

                ip = IPAddress.Parse(ipAddress); // your server's public ip address.

                remote = new IPEndPoint(ip, 11111);
                client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                // sets timeout variables.
                client_socket.ReceiveTimeout = receiveTimeout;
                client_socket.SendTimeout = sendTimeout;

                // non-blocking socket for client
                client_socket.Blocking = blockingSockets;

                // the client is running
                running = true;

                try
                {
                    // Console.WriteLine("Connection Made");
                }
                catch (ArgumentNullException anexc)
                {
                    Console.WriteLine("ArgumentNullException: {0}", anexc.ToString());
                }
                catch (SocketException sexc)
                {
                    Console.WriteLine("SocketException: {0}", sexc.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception: {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
            }
        }


        // updates the client
        public void Update()
        {
            try
            {
                // sends the data
                client_socket.SendTo(buffer, remote);

            }
            catch (ArgumentNullException anexc)
            {
                Console.WriteLine("ArgumentNullException: {0}", anexc.ToString());
            }
            catch (SocketException sexc)
            {
                Console.WriteLine("SocketException: {0}", sexc.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception: {0}", e.ToString());
            }
        }

        // shuts down the client
        public void ShutdownClient()
        {
            // release the socket if it has been established.
            if(client_socket != null)
            {
                client_socket.Shutdown(SocketShutdown.Both);
                client_socket.Close();
                running = false;

                Console.WriteLine("Client Shutdown");
            }
            
        }

        // destructor
        ~UdpClient()
        {
            ShutdownClient();
        }
    }
}
