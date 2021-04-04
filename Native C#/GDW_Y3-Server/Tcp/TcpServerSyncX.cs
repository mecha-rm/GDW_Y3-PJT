using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace NetworkLibrary
{
    // tcp with multiple sockets, optimized.
    class TcpServerSyncX : Server
    {
        // name of server
        private string serverName = "";

        // default buffer size
        private int defaultBufferSize = 512;

        // server variables
        private byte[] outBuffer;

        // list of in buffers
        private List<byte[]> inBuffers = new List<byte[]>();


        // server variables
        private IPAddress ip;
        private Socket server_socket = null;
        // private Socket client_socket = null;

        // client sockets and remote clients.
        private List<Socket> client_sockets = new List<Socket>();
        private List<IPEndPoint> remoteClients = new List<IPEndPoint>();

        // private IPEndPoint remoteClient = null;

        // the ip address for the object.
        string ipAddress = "";

        // port
        private int port = 11111;

        // the backlog
        private int backlog = 16;

        // if 'true', sockets are being blocked.
        // it errors out if set to false by default when there is no connection being made.
        private bool blockingSockets = true;

        // Error 10035 is a non-blocking socket error
        // if this variable is set to 'true', exceptions of this number do not print.
        public bool ignoreError10035 = false;

        // timeout variables
        // these error out if set to 0 by default when there is no connection being made.
        private int receiveTimeout = 0, sendTimeout = 0;

        // delay on acceptions in microseconds
        // a -1 value indicates to wait indefinitely (same as called Accept() like nomal).
        public int acceptTimeout = 1000000; // 1 second

        // checks to see if the server is running
        private bool running = false;


        // constructor
        public TcpServerSyncX()
        {

        }

        // gets the server name
        public string GetServerName()
        {
            return serverName;
        }

        // BUFFER RELATED // 
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
        // get number of endpoints available (in MICROSECONDS)
        public int GetAcceptTimeout()
        {
            return acceptTimeout;
        }

        // sets the accept timeout (in MICROSECONDS)
        public void SetAcceptTimeout(int microseconds)
        {
            acceptTimeout = microseconds;
        }

        // gets the accept timeout in seconds
        public int GetAcceptTimeoutInSeconds()
        {
            // 1 second = 1,000,000 microseconds
            return acceptTimeout / 1000000;
        }

        // sets the accept timeout in seconds
        public void SetAcceptTimeoutInSeconds(int seconds)
        {
            acceptTimeout = seconds * 1000000;
        }


        // gets the endpoint count.
        public int GetEndPointCount()
        {
            return remoteClients.Count;
        }

        // Adding and Removing Clients
        public EndPoint GetEndPoint(int index)
        {
            // index out of bounds
            if (index < 0 || index >= remoteClients.Count)
            {
                return null;
            }

            return remoteClients[index];

        }

        // adds a remote client with the default buffer size
        public byte[] AddEndPoint()
        {
            return AddEndPoint(defaultBufferSize);
        }

        // adds a remote client with a buffer size
        // if the buffer size is negative, then the default size is set.
        public byte[] AddEndPoint(int bufferSize)
        {
            // server socket not set.
            if (server_socket == null)
                Console.WriteLine("Server has not been run.");

            try
            {
                List<Socket> sockets = new List<Socket>();
                Socket newSocket;
                IPEndPoint endPoint;
                byte[] buffer;

                // socket list.
                // sets to be blocking so that Accept can function.
                server_socket.Blocking = true;
                sockets.Add(server_socket);

                // only sockets with connection requests remain.
                // delay
                Socket.Select(sockets, null, null, acceptTimeout);

                // nothing to connect to.
                if(sockets.Count == 0)
                {
                    Console.WriteLine("No client connected in time. Endpoint creation failure.");
                    server_socket.Blocking = blockingSockets;
                    return null;
                }

                // accepts the request.
                newSocket = sockets[0].Accept();
                newSocket.Blocking = blockingSockets;
                endPoint = (IPEndPoint)newSocket.RemoteEndPoint;                

                // returning server socket settings.
                server_socket = sockets[0];
                server_socket.Blocking = blockingSockets;

                // client socket settings
                // non-blocking if false (recommended)
                newSocket.ReceiveTimeout = receiveTimeout;
                newSocket.SendTimeout = sendTimeout;
                newSocket.Blocking = blockingSockets;

                // creating the buffer
                // buffer size is negative
                if (bufferSize < 0)
                    bufferSize = defaultBufferSize;

                // create buffer data
                buffer = new byte[bufferSize];

                // adds content to lists
                client_sockets.Add(newSocket);
                remoteClients.Add(endPoint);
                inBuffers.Add(buffer);

                Console.WriteLine("Endpoint Created Succesfully");

                // returns the buffer
                return buffer;
            }
            catch (ArgumentNullException ane) // null
            {
                Console.WriteLine("ArgumentNullException: {0}", ane.ToString());
            }
            catch (SocketException se) // socket
            {
                Console.WriteLine("SocketException: {0}", se.ToString());
            }
            catch (Exception e) // generic
            {
                Console.WriteLine("SocketException: {0}", e.ToString());
            }

            // exception encountered.
            return null;
        }


        // removes a remote client and returns its buffer.
        public byte[] RemoveEndPoint(int index)
        {
            // index out of bounds
            if (index < 0 || index >= remoteClients.Count)
            {
                return null;
            }

            // removes buffer and remote client
            client_sockets.RemoveAt(index);
            remoteClients.RemoveAt(index);

            byte[] buffer = inBuffers[index];
            inBuffers.RemoveAt(index);

            return buffer;
        }

        // deletes the remote client
        public void DeleteEndPoint(int index)
        {
            // gets the buffer via index.
            byte[] buffer = RemoveEndPoint(index);

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

        // TODO: add 'AddSendBufferData' function?

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

        // gets backlog value for listening
        public int GetBacklog()
        {
            return backlog;
        }

        // sets backlog value.
        public void SetBacklog(int bl)
        {
            backlog = bl;
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
        public override void RunServer()
        {
            // buffers have not been generated
            // sending out data
            if (outBuffer == null)
                outBuffer = new byte[defaultBufferSize];


            // setting data values
            for (int i = 0; i < inBuffers.Count; i++)
            {
                if (inBuffers[i] == null)
                    inBuffers[i] = new byte[defaultBufferSize];
            }

            // TODO: move host out of if statement for ipaddress in server files.
            // buffer = new byte[512];
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            // if the ip address has not already been set.
            if (ipAddress == "")
            {
                ip = host.AddressList[1]; // get IP address from list
            }
            else
            {
                ip = IPAddress.Parse(ipAddress);
            }

            // IPAddress ip = IPAddress.Parse("192.168.2.144"); // manually enter IP address (default).

            serverName = host.HostName; // server name
            Console.WriteLine("Server name: {0} IP: {1}", host.HostName, ip);

            // using the same port that was used last class.
            IPEndPoint localEP = new IPEndPoint(ip, port);

            // last class the family was entered, but you can get from the ip directly the Address family.
            server_socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // 
            try
            {
                // the server listens and provides a service.
                server_socket.Bind(localEP);

                // listening on ip:port
                server_socket.Listen(backlog); // backlog

                // 0 for any available port.
                // client = new IPEndPoint(IPAddress.Any, 0); // 0 for any available port.
                // remoteClient = (EndPoint)client;

                // server - put before 
                // sets timeout variables.
                server_socket.ReceiveTimeout = receiveTimeout;
                server_socket.SendTimeout = sendTimeout;

                // non-blocking if false (recommended)
                server_socket.Blocking = blockingSockets;

                // connections setup
                Console.WriteLine("Open to add connections...");


                // prints different message based on selected mode.
                Console.WriteLine("Prepared to send and receive data.");

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
        public override void Update()
        {
            // checks to see if the server is running.
            if (!running)
            {
                Console.WriteLine("The server has not been started. Call RunServer().");
                return;
            }

            // RECEIVE DATA //
            // goes through all sockets
            for(int i = 0; i < client_sockets.Count; i++)
            {
                try
                {
                    // gets values
                    Socket cs = client_sockets[i];
                    IPEndPoint ep = remoteClients[i];
                    byte[] buffer = inBuffers[i];

                    // if the endpoint isn't connected, don't do anything.
                    if (!cs.Connected)
                    {
                        Console.WriteLine("Endpoint is not connected.");
                    }

                    // if there's no data to get.
                    if (cs.Available == 0)
                        continue;

                    // receive data
                    int rec = cs.Receive(buffer);

                    // sets back values
                    client_sockets[i] = cs;
                    remoteClients[i] = ep;
                    inBuffers[i] = buffer;
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException: {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    // Error: WSAEWOULDBLOCK - 10035
                    // this error is expected if you are using non-blocking sockets.
                    if (!ignoreError10035 || (ignoreError10035 && se.ErrorCode != 10035))
                        Console.WriteLine("SocketException: {0}", se.ToString());
                }
                catch (Exception e)
                {
                    // Console.WriteLine(e.ToString());
                    Console.WriteLine("Exception: {0}", e.ToString());
                }
            }


            // SEND DATA //
            // goes through all sockets
            for (int i = 0; i < client_sockets.Count; i++)
            {
                try
                {
                    // gets values
                    Socket cs = client_sockets[i];
                    IPEndPoint ep = remoteClients[i];
                    byte[] buffer = inBuffers[i];

                    // receive data
                    cs.Send(buffer);

                    // sets back values
                    client_sockets[i] = cs;
                    remoteClients[i] = ep;
                    inBuffers[i] = buffer;

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException: {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    // Error: WSAEWOULDBLOCK - 10035
                    // this error is expected if you are using non-blocking sockets.
                    if (!ignoreError10035 || (ignoreError10035 && se.ErrorCode != 10035))
                        Console.WriteLine("SocketException: {0}", se.ToString());
                }
                catch (Exception e)
                {
                    // Console.WriteLine(e.ToString());
                    Console.WriteLine("Exception: {0}", e.ToString());
                }
            }

        }

        // shuts down the server.
        public override void ShutdownServer()
        {
            // used to see if the server was ever actually started.
            if (!running)
            {
                Console.WriteLine("The server is not currently running.");
                return;
            }


            // the server socket has not been generated.
            foreach(Socket cs in client_sockets)
            {
                cs.Shutdown(SocketShutdown.Both);
                cs.Close();
            }

            running = false;

            // shuts down server.
            Console.WriteLine("Server Shutdown");

        }

        // destructor
        ~TcpServerSyncX()
        {
            ShutdownServer();
        }
    }
}
