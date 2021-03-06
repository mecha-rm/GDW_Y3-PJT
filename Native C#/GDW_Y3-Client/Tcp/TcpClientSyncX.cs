﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Net.NetworkInformation;

namespace NetworkLibrary
{
    // client sync
    class TcpClientSyncX : Client
    {
        // client variables
        private byte[] outBuffer; // sending data to server
        private byte[] inBuffer; // getting data from a server

        // default buffer size
        private int defaultBufferSize = 512;

        // ip address
        private IPAddress ip;

        // remote
        private IPEndPoint remote = null;
        private Socket client_socket = null;

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
        // this should be left as 'true' on the client side. Only on the server side should this be set to false.
        // NOTE: setting 'blockingSockets' to false sets 'Connected' to false.
        private bool blockingSockets = true;

        // timeout variables
        private int receiveTimeout = 0, sendTimeout = 0;

        // delay on acceptions in microseconds
        // a -1 value indicates to wait indefinitely (same as called Accept() like nomal).
        public int connectTimeout = 1000000; // 1 second

        // if 'true', an attempt to connect is done when the client is run.
        public bool connectOnRun = true;

        // event reset
        // private ManualResetEvent resetConnectEvent = new ManualResetEvent(false);

        // checks to see if the server is running
        private bool running = false;

        // constructor
        public TcpClientSyncX()
        {

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

        // returns the buffer size.
        public int GetReceiveBufferSize()
        {
            if (inBuffer != null)
                return inBuffer.Length;
            else
                return 0;
        }

        // sets the buffer size for the server
        public void SetReceiveBufferSize(int size)
        {
            // the size value is invalid
            if (size < 0)
                return;

            // resizing array
            if (inBuffer != null)
                Array.Resize<byte>(ref inBuffer, size);
            else
                inBuffer = new byte[size];
        }

        // gets the send buffer data
        public byte[] GetSendBufferData()
        {
            return outBuffer;
        }


        // CONNECT FUNCTIONS

        // connect callback
        // private void ConnectCallback(IAsyncResult ar)
        // {
        //     try
        //     {
        //         Socket localSocket = (Socket)ar.AsyncState;
        // 
        //         // connection has been ended.
        //         localSocket.EndConnect(ar);
        // 
        //         resetConnectEvent.Set();
        // 
        //         client_socket = localSocket;
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e.ToString());
        //     }
        // }


        // connects to endpoint (requires 'Run' to be called).
        public bool Connect()
        {
            // run has not been called.
            if (client_socket == null || remote == null)
            {
                Console.WriteLine("Client has not been run.");
                return false;
            }


            // connects the tcp socket
            // client_socket.BeginConnect(remote, new AsyncCallback(ConnectCallback), client_socket);
            // resetConnectEvent.WaitOne();
            // 
            // return false;

            // ORIGINAL
            try
            {
                // list of sockets.
                // List<Socket> sockets = new List<Socket>();
                // sockets.Add(client_socket);
                // 
                // // checks for an endpoint to connect to.
                client_socket.Blocking = true;

                // client_socket.ReceiveTimeout = receiveTimeout;
                // client_socket.SendTimeout = sendTimeout;
                // Socket.Select(sockets, null, null, connectTimeout);

                // IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

                // nothing to connect to
                // if (sockets.Count == 0)
                // {
                //     Console.WriteLine("No available endpoint found. Connection failure.");
                //     client_socket.Blocking = blockingSockets;
                // 
                //     // connection failed.
                //     return false;
                // }

                // TcpClient tcpClient = new TcpClient();
                // tcpClient.Connect(ip.ToString(), port);
                // tcpClient.Close();

                // Ping ping = new Ping();
                // PingReply pingReply = ping.Send(ip.ToString());
                // 
                // // ping was successful.
                // if (pingReply.Status == IPStatus.Success)
                // {
                //     client_socket.Connect(remote);
                // }

                client_socket.Connect(remote);

                // if no exception was thrown

                // makes connection
                // sockets[0].Connect(remote); // connect
                // sockets[0].ConnectAsync(remote);
                // client_socket.Connect(remote);
                // client_socket.ConnectAsync(host.HostName, port);
                // client_socket.Connect(host.HostName, port);
                // client_socket.ConnectAsync(remote);
                // client_socket.ConnectAsync(remote);

                // no connection made.
                // if (!client_socket.Connected)
                // {
                //     Console.WriteLine("No endpoint found. Connection failed.");
                //     client_socket.Blocking = blockingSockets;
                //     return false;
                // }

                // save socket.
                // client_socket = sockets[0];

                // timeouts
                // client_socket.ReceiveTimeout = receiveTimeout;
                // client_socket.SendTimeout = sendTimeout;

                // non-blocking socket for client
                // client_socket.Blocking = blockingSockets;

                // connection successful
                Console.WriteLine("Connection made Succesfully");
                return true;

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException: {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException: {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception: {0}", e.ToString());
            }

            // returns false
            return false;
        }


        // sets the receive buffer data
        // if 'deleteOldData' is set to true, the original data is cleared out.
        public void SetSendBufferData(byte[] data, bool deleteOldData = false)
        {
            if (outBuffer != null && deleteOldData) // out buffer exists
                Array.Clear(outBuffer, 0, outBuffer.Length);

            outBuffer = data;
        }

        // TODO: add 'AddSendBufferData' function?

        // gets the receive buffer data
        public byte[] GetReceiveBufferData()
        {
            return inBuffer;
        }

        // sets the receive buffer data
        // if 'deleteOldData' is set to 'true', then the original data is not deleted.
        public void SetReceiveBufferData(byte[] data, bool deleteOldData = false)
        {
            if (inBuffer != null && deleteOldData) // in buffer exists
                Array.Clear(inBuffer, 0, inBuffer.Length);

            inBuffer = data;
        }

        // get number of endpoints available (in MICROSECONDS)
        public int GetConnectTimeout()
        {
            return connectTimeout;
        }

        // sets the accept timeout (in MICROSECONDS)
        public void SetConnectTimeout(int microseconds)
        {
            connectTimeout = microseconds;
        }

        // gets the accept timeout in seconds
        public int GetConnectTimeoutInSeconds()
        {
            // 1 second = 1,000,000 microseconds
            return connectTimeout / 1000000;
        }

        // sets the accept timeout in seconds
        public void SetConnectTimeoutInSeconds(int seconds)
        {
            connectTimeout = seconds * 1000000;
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
            remote = new IPEndPoint(ip, port); // create remote with port
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
        // this does not get set to 'true', even if the UDP server is connected.
        public bool IsConnected()
        {
            if (client_socket != null)
            {
                return client_socket.Connected;
            }

            return false;
        }

        // runs the client
        public override void RunClient()
        {
            // setting out buffer if it has not been established.
            if (outBuffer == null)
                outBuffer = new byte[defaultBufferSize];

            // setting up the in buffer
            if (inBuffer == null)
                inBuffer = new byte[defaultBufferSize];

            try
            {
                // if the ip address has not already been set.
                if (ipAddress == "")
                {
                    // looks for ipv4
                    Console.WriteLine("Acquiring IPv4");
                    ip = GetLocalIPv4Address();

                    if (ip == null) // ipv4 not found.
                    {
                        Console.WriteLine("IPv4 not found. Acquiring IPv6");
                        ip = GetLocalIPv6Address();

                        if (ip == null) // ipv6 not found.
                        {
                            Console.WriteLine("IPv4 and IPv6 not found. Setting to local host.");
                            ip = LocalHostIPv4;

                            // no local host ipv4, so get ipv6
                            if (ip == null)
                                ip = LocalHostIPv6;
                        }

                        // saving string
                        ipAddress = ip.ToString();
                    }
                    else
                    {
                        // saving to string
                        ipAddress = ip.ToString();
                    }
                }
                else
                {
                    // parses saved ip
                    ip = IPAddress.Parse(ipAddress);
                }


                remote = new IPEndPoint(ip, port);
                client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // client connects to remote
                // TODO: create timeout function for connection to socket.
                // Console.WriteLine("Searching for server with IP " + ipAddress + "...");
                // client_socket.Connect(remote);
                // 
                // // client prepared
                // Console.WriteLine("Client now prepared to send and receive data from the server respectively...");
                // 
                // // these must go AFTER 'Connect' is called, otherwise it crashes.
                // // sets timeout variables.
                // client_socket.ReceiveTimeout = receiveTimeout;
                // client_socket.SendTimeout = sendTimeout;
                // 
                // // non-blocking socket for client
                // client_socket.Blocking = blockingSockets;

                // attempts connection
                if(connectOnRun)
                {
                    Console.WriteLine("Attempting connection...");
                    Connect();
                }

                // the client is running
                running = true;

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException: {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException: {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception: {0}", e.ToString());
            }
        }


        // updates the client
        public override void Update()
        {
            // checks to see if the client is running.
            if (!running)
            {
                Console.WriteLine("The client has not been started. Call RunClient().");
                return;
            }

            // attempts to connect if no connection exists.
            if(!client_socket.Connected)
            {
                Console.WriteLine("No active connection. Attempting to connect.");
                Connect();
            }

            
            // SEND DATA
            try
            {
                // sends the data
                client_socket.Send(outBuffer);
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


            // RECEIVE DATA
            try
            {
                // receives data
                int rec;

                // if there's data available (non-blocking only).
                if ((!blockingSockets && client_socket.Available != 0) || blockingSockets)
                    rec = client_socket.Receive(inBuffer);
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
        public override void ShutdownClient()
        {
            // used to see if the client was ever actually started.
            if (!running)
            {
                Console.WriteLine("The client is not currently running.");
                return;
            }


            try
            {
                // release the socket if it has been established.
                if (client_socket != null)
                {
                    client_socket.Shutdown(SocketShutdown.Both);
                    client_socket.Close();
                    running = false;

                    Console.WriteLine("Client Shutdown");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        // destructor
        ~TcpClientSyncX()
        {
            ShutdownClient();
        }
    }
}
