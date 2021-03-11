using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace NetworkLibrary
{
    class UdpServerX
    {
        // name of server
        private string serverName = "";

        // default buffer size
        private int defaultBufferSize = 512;

        // out buffer to clients
        private byte[] outBuffer;

        // list of in buffers
        private List<byte[]> inBuffers = new List<byte[]>();

        // ip address of server
        private IPAddress ip;

        // private IPEndPoint client = null;
        private Socket server_socket = null;

        // remote clients
        private List<EndPoint> remoteClients = new List<EndPoint>();

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

        // gets the server name
        public string GetServerName()
        {
            return serverName;
        }

        // get the default buffer size
        public int GetDefaultBufferSize()
        {
            return defaultBufferSize;
        }

        // sets the default buffer size
        public void SetDefaultBufferSize(int newSize)
        {
            if (newSize < 0)
                return;

            defaultBufferSize = newSize;
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
        
        // ADDERS AND REMOVERS FOR CLIENTS

        // Adding and Removing Clients
        public EndPoint GetRemoteClient(int index)
        {
            // index out of bounds
            if(index < 0 || index >= remoteClients.Count)
            {
                return null;
            }

            return remoteClients[index];

        }

        // adds a remote client with the default buffer size
        public byte[] AddRemoteClient()
        {
            return AddRemoteClient(defaultBufferSize);
        }

        // adds a remote client with a buffer size
        // if the buffer size is negative, then the default size is set.
        public byte[] AddRemoteClient(int bufferSize)
        {
            EndPoint newRemote = new IPEndPoint(IPAddress.Any, 0); // any available port
            remoteClients.Add(newRemote);

            // buffer size is negative
            if (bufferSize < 0)
                bufferSize = defaultBufferSize;

            inBuffers.Add(new byte[bufferSize]);

            return inBuffers[inBuffers.Count - 1];

        }

        // adds a remote client with a buffer
        // this returns the buffer that was just added
        public byte[] AddRemoteClient(int bufferSize, byte[] buffer)
        {
            EndPoint newRemote = new IPEndPoint(IPAddress.Any, 0); // any available port
            remoteClients.Add(newRemote);

            // buffer size is negative
            if (bufferSize < 0)
                bufferSize = defaultBufferSize;

            // setting to new buffer
            inBuffers.Add(null);
            inBuffers[inBuffers.Count - 1] = buffer;

            return buffer;

        }

        // removes a remote client and returns its buffer.
        public byte[] RemoteRemoteClient(int index)
        {
            // index out of bounds
            if (index < 0 || index >= remoteClients.Count)
            {
                return null;
            }

            // removes buffer and remote client
            remoteClients.RemoveAt(index);

            byte[] buffer = inBuffers[index];
            inBuffers.RemoveAt(index);

            return buffer;
        }

        // deletes the remote client
        public void DeleteRemoteClient(int index)
        {
            // index out of bounds
            if (index < 0 || index >= remoteClients.Count)
            {
                return;
            }

            // remote client (garbage collector will delete this)
            remoteClients.RemoveAt(index);


            // gets the buffer
            byte[] buffer = inBuffers[index];
            inBuffers.RemoveAt(index);

            // deletes the buffer data
            Array.Clear(buffer, 0, buffer.Length);
        }

       
        // SETTER AND GETTER FOR BUFFER DATA
        // gets the send buffer data
        public byte[] GetSendBufferData()
        {
            return outBuffer;
        }

        // sets the receive buffer data
        public void SetSendBufferData(byte[] data, bool deleteOldData = false)
        {
            if (outBuffer != null && deleteOldData) // out buffer exists
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
        public void SetReceiveBufferData(int index, byte[] data, bool deleteOldData = false)
        {
            if (index >= 0 && index < inBuffers.Count)
            {
                if (inBuffers[index] != null && deleteOldData) // in buffer exists
                    Array.Clear(inBuffers[index], 0, inBuffers[index].Length);

                inBuffers[index] = data;
            }
        }

        // gets the ip address as a string.
        public string GetIPAddress()
        {
            if (ip != null) // references ip object
                return ip.ToString();
            else // if the ip object has not been made yet, reference the string.
                return ipAddress;
        }

        // sets the IP address
        public void SetIPAdress(string ipAdd)
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
                outBuffer = new byte[defaultBufferSize];


            // setting data values
            for(int i =  0; i < inBuffers.Count; i++)
            {
                if (inBuffers[i] == null)
                    inBuffers[i] = new byte[defaultBufferSize];
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

            serverName = host.HostName; // server name
            Console.WriteLine("Server name: {0} IP: {1}", host.HostName, ip);

            // using the same port that was used last class.
            IPEndPoint localEP = new IPEndPoint(ip, port);

            // last class the family was entered, but you can get from the ip directly the Address family.
            server_socket = new Socket(ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            // 0 for any available port.
            // client = new IPEndPoint(IPAddress.Any, 0); // 0 for any available port.

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

                // brings in data from all buffers
                for(int i = 0; i < inBuffers.Count; i++)
                {
                    // gets the remote client
                    EndPoint ep = remoteClients[i];

                    // receives the data
                    rec = server_socket.ReceiveFrom(inBuffers[i], ref ep);
                }

                // send out data
                for(int i = 0; i < remoteClients.Count; i++)
                {
                    server_socket.SendTo(outBuffer, remoteClients[i]);
                }
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
