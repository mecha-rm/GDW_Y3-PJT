using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace GDW_Y3_Server
{
    class UdpServerX
    {
        // server variables
        // out buffer to clients
        private byte[] outBuffer;

        private List<byte[]> inBuffers = new List<byte[]>(1);


        // ip address of server
        private IPAddress ip;

        // private IPEndPoint client = null;
        private Socket server_socket = null;

        // remote clients
        private List<EndPoint> remoteClients = new List<EndPoint>(1);

        // the ip address for the object.
        string ipAddress = "";

        // port
        private int port = 11111;

        /// <summary>
        /// an exception is thrown if the program is set to either use non-blockng sockets, or timeout variables not set to 0.
        /// this happens because the program is expecting to get data, but since it's not waiting, it moves onto the next line of code.
        /// things still work, but the sent data will be harder to read due to messages constantly printing.
        /// </summary>

        // if 'true', sockets are being blocked.
        // it errors out if set to false by default when there is no connection being made.
        private bool blockingSockets = true;

        // timeout variables
        // these error out if set to 0 by default when there is no connection being made.
        private int receiveTimeout = 0, sendTimeout = 0;

        // checks to see if the server is running
        private bool running = false;


        // constructor
        public UdpServerX()
        {

        }


        // SEND DATA //
        // returns the buffer size.
        public int GetSendBufferSize()
        {
            if (outBuffer != null)
                return outBuffer.Length;
            else
                return 0;
        }

        // sets the buffer size for the server
        public void SetSendBufferSize(int size)
        {
            // the size value is invalid
            if (size < 0)
                return;

            // resizing array
            if (outBuffer != null)
                Array.Resize(ref outBuffer, size);
            else
                outBuffer = new byte[size];
        }


        // RECEIVE DATA //
        // gets size of buffer at provided index
        public int GetReceiveBufferSize(int index)
        {
            // validity check
            if (index < 0 || index >= inBuffers.Count)
                return -1;
            else
                return inBuffers[index].Length;
        }
        

        // sets the buffer size for the server
        public void SetReceiveBufferSize(int index, int size)
        {
            // validity check
            if (index >= 0 && index < inBuffers.Count)
            {
                // the size value is invalid
                if (size < 0)
                    return;

                // resizing array
                if (inBuffers[index] != null)
                {
                    byte[] data = inBuffers[index];
                    Array.Resize(ref data, size);
                    inBuffers[index] = data;
                }  
                else
                {
                    inBuffers[index] = new byte[size];
                }
                    
            }
        }

       
        // SETTER AND GETTER FOR BUFFER DATA
        // gets the send buffer data
        public byte[] GetSendBufferData()
        {
            return outBuffer;
        }

        // sets the receive buffer data
        public void SetSendBufferData(byte[] data)
        {
            if (outBuffer != null) // out buffer exists
                Array.Clear(outBuffer, 0, outBuffer.Length);

            outBuffer = data;
        }

        // gets receive buffer data
        public byte[] GetReceiveBufferData(int index)
        {
            // validity check
            if (index < 0 || index >= inBuffers.Count)
                return null;
            else
                return inBuffers[index];
        }

        // sets the receive buffer data
        public void SetReceiveBufferData(int index, byte[] data)
        {
            if (index >= 0 && index < inBuffers.Count)
            {
                if (inBuffers[index] != null) // in buffer exists
                    Array.Clear(inBuffers[index], 0, inBuffers[index].Length);

                inBuffers[index] = data;
            }
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
            ipAddress = ipAdd;
        }

        // gets the port number
        public int GetPort()
        {
            return port;
        }

        // sets the port number
        // this cannot be changed while a server is running
        public void SetPort(int newPort)
        {
            if (!running) // server is not running
            {
                port = newPort;
            }
            else // server is running
            {
                Console.WriteLine("Port number cannot be changed while the server is running.");
            }
        }

        // checks to see if the server is blocking sockets
        public bool IsBlockingSockets()
        {
            // if the server socket exists, that variable is referenced.
            // if not, the class variable is used.
            if (server_socket != null)
                return server_socket.Blocking;
            else
                return blockingSockets;
        }

        // sets the blocking sockets variable
        public void SetBlockingSockets(bool blocking)
        {
            // sets variable
            blockingSockets = blocking;

            if (server_socket != null)
                server_socket.Blocking = blockingSockets;
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
            if (server_socket != null)
                server_socket.SendTimeout = sendTimeout;
        }

        // gets the receiver timeout.
        public int GetReceiveTimeout()
        {
            return receiveTimeout;
        }

        // sets the receiver timeout.
        public void SetReceiveTimeout(int newRt)
        {
            receiveTimeout = newRt;

            // if the server socket has been generated.
            if (server_socket != null)
                server_socket.ReceiveTimeout = receiveTimeout;
        }

        // returns 'true' if the server is running
        public bool IsRunning()
        {
            if (server_socket != null)
            {
                return running;
            }

            return false;
        }

        // returns 'true' if the server is running
        public bool IsConnected()
        {
            if (server_socket != null)
            {
                return server_socket.Connected;
            }

            return false;
        }

        // runs the server project
        public void RunServer()
        {
            // buffers have not been generated
            // sending out data
            if (outBuffer == null)
                outBuffer = new byte[512];


            // setting data values
            for(int i =  0; i < inBuffers.Count; i++)
            {
                if (inBuffers[i] == null)
                    inBuffers[i] = new byte[512];
            }    


            // buffer = new byte[512];
            IPHostEntry host;

            // if the ip address has not already been set.
            if (ipAddress == "")
            {
                host = Dns.GetHostEntry(Dns.GetHostName());
                ip = host.AddressList[1]; // get IP address from list
            }
            else
            {
                host = Dns.GetHostEntry(Dns.GetHostName());
                ip = IPAddress.Parse(ipAddress);
            }

            // IPAddress ip = IPAddress.Parse("192.168.2.144"); // manually enter IP address (default).


            Console.WriteLine("Server name: {0} IP: {1}", host.HostName, ip);

            // using the same port that was used last class.
            IPEndPoint localEP = new IPEndPoint(ip, port);

            // last class the family was entered, but you can get from the ip directly the Address family.
            server_socket = new Socket(ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            // 0 for any available port.
            // client = new IPEndPoint(IPAddress.Any, 0); // 0 for any available port.

            // TODO: add remote client array

            if (remoteClients.Count == 0)
                remoteClients.Add(null);

            // Add remote client list
            // remoteClient1 = (EndPoint)(new IPEndPoint(IPAddress.Any, 0));
            // remoteClient2 = (EndPoint)(new IPEndPoint(IPAddress.Any, 0));
            // remoteClient3 = (EndPoint)(new IPEndPoint(IPAddress.Any, 0));
            // remoteClient4 = (EndPoint)(new IPEndPoint(IPAddress.Any, 0));

            // 
            try
            {
                // the server listens and provides a service.
                server_socket.Bind(localEP);

                // server prepared
                Console.WriteLine("Waiting for, and prepared to send data...");

                // sets timeout variables.
                server_socket.ReceiveTimeout = receiveTimeout;
                server_socket.SendTimeout = sendTimeout;

                // non-blocking if false (recommended)
                server_socket.Blocking = blockingSockets;

                // the server is running
                running = true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        // updates the server to listen for a message from the client.
        // this gets called each frame by the program using the plugin.
        public void Update()
        {
            try
            {
                // receives data
                int rec;



                // gets data from all clients
                // rec = server_socket.ReceiveFrom(inBuffer1, ref remoteClient1);
                // rec = server_socket.ReceiveFrom(inBuffer2, ref remoteClient2);
                // rec = server_socket.ReceiveFrom(inBuffer3, ref remoteClient3);
                // rec = server_socket.ReceiveFrom(inBuffer4, ref remoteClient4);
                // 
                // 
                // // Console.WriteLine("Received: {0} from Client: {1}", Encoding.ASCII.GetString(inBuffer, 0, rec), remoteClient1.ToString());
                // 
                // // sends data
                // server_socket.SendTo(outBuffer, remoteClient1);
                // server_socket.SendTo(outBuffer, remoteClient2);
                // server_socket.SendTo(outBuffer, remoteClient3);
                // server_socket.SendTo(outBuffer, remoteClient4);

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
                // Console.WriteLine(e.ToString());
                Console.WriteLine(e.ToString() + " - Client Response Failed");
            }
        }

        // shuts down the server.
        public void ShutdownServer()
        {
            // the server socket has not been generated.
            if (server_socket != null)
            {
                server_socket.Shutdown(SocketShutdown.Both);
                server_socket.Close();
                running = false;

                Console.WriteLine("Server Shutdown");
            }

        }

        // destructor
        ~UdpServerX()
        {
            ShutdownServer();
        }
    }
}
