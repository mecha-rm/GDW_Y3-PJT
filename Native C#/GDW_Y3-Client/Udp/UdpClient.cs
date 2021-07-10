using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkLibrary
{
    public class UdpClient : Client
    {
        // enum for mode
        public enum mode { both, send, receive };

        // communication mode
        public mode commMode = mode.both;

        // client variables
        private byte[] outBuffer; // sending data to server
        private byte[] inBuffer; // getting data from a server

        // default buffer size
        private int defaultBufferSize = 512;

        private IPAddress ip;

        private EndPoint remote = null;
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
        private bool blockingSockets = true;

        // if 'true', this error is not printed.
        public bool ignoreError10035 = false;

        // timeout variables
        private int receiveTimeout = 0, sendTimeout = 0;

        // checks to see if the server is running
        private bool running = false;

        // constructor
        public UdpClient()
        {

        }

        // gets the communication mode
        public mode GetCommunicationMode()
        {
            return commMode;
        }

        // sets the communication mode
        public void SetCommunicationMode(mode newMode)
        {
            commMode = newMode;
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
        public int GetReceiveTimeout()
        {
            return receiveTimeout;
        }

        // sets the receiver timeout.
        public void SetReceiveTimeout(int newRt)
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
                client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                // client prepared
                Console.WriteLine("Prepared to send and receive data from the server respectively...");

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
                    if (!ignoreError10035 || (ignoreError10035 && sexc.ErrorCode != 10035))
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
        public override void Update()
        {
            // checks to see if the client is running.
            if (!running)
            {
                Console.WriteLine("The client has not been started. Call RunClient().");
                return;
            }


            // SEND //
            try
            {
                // sends the data
                if (commMode == mode.both || commMode == mode.send)
                {
                    client_socket.SendTo(outBuffer, remote);
                }
            }
            catch (ArgumentNullException anexc)
            {
                Console.WriteLine("ArgumentNullException: {0}", anexc.ToString());
            }
            catch (SocketException sexc)
            {
                if (!ignoreError10035 || (ignoreError10035 && sexc.ErrorCode != 10035))
                    Console.WriteLine("SocketException: {0}", sexc.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception: {0}", e.ToString());
            }


            // RECEIVE //
            try
            {
                // receives the data
                if (commMode == mode.both || commMode == mode.receive)
                {
                    int rec = client_socket.ReceiveFrom(inBuffer, ref remote);

                    // Console.WriteLine("Received: {0} from Server: {1}", Encoding.ASCII.GetString(inBuffer, 0, rec), remote.ToString());
                }

            }
            catch (ArgumentNullException anexc)
            {
                Console.WriteLine("ArgumentNullException: {0}", anexc.ToString());
            }
            catch (SocketException sexc)
            {
                if (!ignoreError10035 || (ignoreError10035 && sexc.ErrorCode != 10035))
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
            if(!running)
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
        ~UdpClient()
        {
            ShutdownClient();
        }
    }
}
