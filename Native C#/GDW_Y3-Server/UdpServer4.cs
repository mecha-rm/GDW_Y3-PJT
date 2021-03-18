using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace NetworkLibrary
{
    // server with 4 clients
    class UdpServer4
    {
        // name of server
        private string serverName = "";

        // default buffer size
        private int defaultBufferSize = 512;

        // server variables
        // out buffer to clients
        private byte[] outBuffer;

        // in buffer from all clients
        private byte[] inBuffer1;
        private byte[] inBuffer2;
        private byte[] inBuffer3;
        private byte[] inBuffer4;


        // ip address of server
        private IPAddress ip;

        // private IPEndPoint client = null;
        private Socket server_socket = null;

        // remote clients
        private EndPoint remoteClient1 = null;
        private EndPoint remoteClient2 = null;
        private EndPoint remoteClient3 = null;
        private EndPoint remoteClient4 = null;

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

        // Error 10035 is a non-blocking socket error
        // if this variable is set to 'true', exceptions of this number do not print.
        public bool ignoreError10035 = false;

        // timeout variables
        // these error out if set to 0 by default when there is no connection being made.
        private int receiveTimeout = 0, sendTimeout = 0;

        // checks to see if the server is running
        private bool running = false;


        // constructor
        public UdpServer4()
        {

        }

        // gets the server name
        public string GetServerName()
        {
            return serverName;
        }

        // BUFFERE RELATED //
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
        // returns the buffer size.
        public int GetReceiveBuffer1Size()
        {
            if (inBuffer1 != null)
                return inBuffer1.Length;
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
            if (inBuffer1 != null)
                Array.Resize<byte>(ref inBuffer1, size);
            else
                inBuffer1 = new byte[size];
        }

        // gets the size of the second buffer
        public int GetReceiveBuffer2Size()
        {
            if (inBuffer1 != null)
                return inBuffer2.Length;
            else
                return 0;
        }

        // sets the buffer size for the server
        public void SetReceiveBuffer2Size(int size)
        {
            // the size value is invalid
            if (size < 0)
                return;

            // resizing array
            if (inBuffer2 != null)
                Array.Resize<byte>(ref inBuffer2, size);
            else
                inBuffer2 = new byte[size];
        }

        // gets the size of the third buffer
        public int GetReceiveBuffer3Size()
        {
            if (inBuffer3 != null)
                return inBuffer3.Length;
            else
                return 0;
        }

        // sets the buffer size for the server
        public void SetReceiveBuffer3Size(int size)
        {
            // the size value is invalid
            if (size < 0)
                return;

            // resizing array
            if (inBuffer3 != null)
                Array.Resize<byte>(ref inBuffer3, size);
            else
                inBuffer3 = new byte[size];
        }

        // get receive buffer 4
        public int GetReceiveBuffer4Size()
        {
            if (inBuffer4 != null)
                return inBuffer4.Length;
            else
                return 0;
        }

        // sets the buffer 4 size for the server
        public void SetReceiveBuffer4Size(int size)
        {
            // the size value is invalid
            if (size < 0)
                return;

            // resizing array
            if (inBuffer4 != null)
                Array.Resize<byte>(ref inBuffer4, size);
            else
                inBuffer4 = new byte[size];
        }

        // SETTER AND GETTER FOR BUFFER DATA
        // gets the send buffer data
        public byte[] GetSendBufferData()
        {
            return outBuffer;
        }
   
        // sets the receive buffer data
        // if 'deleteOldData' is set to 'true', the old data is deleted.
        public void SetSendBufferData(byte[] data, bool deleteOldData = false)
        {
            if (outBuffer != null && deleteOldData) // out buffer exists
                Array.Clear(outBuffer, 0, outBuffer.Length);

            outBuffer = data;
        }

        // gets the receive buffer 1 data
        public byte[] GetReceiveBuffer1Data()
        {
            return inBuffer1;
        }

        // sets the receive buffer data
        public void SetReceiveBuffer1Data(byte[] data, bool deleteOldData = false)
        {
            if (inBuffer1 != null && deleteOldData) // in buffer exists
                Array.Clear(inBuffer1, 0, inBuffer1.Length);

            inBuffer1 = data;
        }

        // gets the receive buffer 2 data
        public byte[] GetReceiveBuffer2Data()
        {
            return inBuffer2;
        }

        // sets the receive buffer data
        public void SetReceiveBuffer2Data(byte[] data, bool deleteOldData = false)
        {
            if (inBuffer2 != null && deleteOldData) // in buffer exists
                Array.Clear(inBuffer2, 0, inBuffer2.Length);

            inBuffer2 = data;
        }

        // gets the receive buffer 3 data
        public byte[] GetReceiveBuffer3Data()
        {
            return inBuffer3;
        }

        // sets the receive buffer data
        public void SetReceiveBuffer3Data(byte[] data, bool deleteOldData = false)
        {
            if (inBuffer3 != null && deleteOldData) // in buffer exists
                Array.Clear(inBuffer3, 0, inBuffer3.Length);

            inBuffer3 = data;
        }

        // gets the receive buffer 4 data
        public byte[] GetReceiveBuffer4Data()
        {
            return inBuffer4;
        }

        // sets the receive buffer data
        public void SetReceiveBuffer4Data(byte[] data, bool deleteOldData = false)
        {
            if (inBuffer4 != null && deleteOldData) // in buffer exists
                Array.Clear(inBuffer4, 0, inBuffer4.Length);

            inBuffer4 = data;
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

            // reading in data from buffer 1
            if (inBuffer1 == null)
                inBuffer1 = new byte[defaultBufferSize];

            // reading in data from buffer 2
            if (inBuffer2 == null)
                inBuffer2 = new byte[defaultBufferSize];

            // reading in data from buffer 3
            if (inBuffer3 == null)
                inBuffer3 = new byte[defaultBufferSize];

            // reading in data from buffer 4
            if (inBuffer4 == null)
                inBuffer4 = new byte[defaultBufferSize];

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
            remoteClient1 = (EndPoint)(new IPEndPoint(IPAddress.Any, 0));
            remoteClient2 = (EndPoint)(new IPEndPoint(IPAddress.Any, 0));
            remoteClient3 = (EndPoint)(new IPEndPoint(IPAddress.Any, 0));
            remoteClient4 = (EndPoint)(new IPEndPoint(IPAddress.Any, 0));

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
                rec = server_socket.ReceiveFrom(inBuffer1, ref remoteClient1);
                rec = server_socket.ReceiveFrom(inBuffer2, ref remoteClient2);
                rec = server_socket.ReceiveFrom(inBuffer3, ref remoteClient3);
                rec = server_socket.ReceiveFrom(inBuffer4, ref remoteClient4);


                // Console.WriteLine("Received: {0} from Client: {1}", Encoding.ASCII.GetString(inBuffer, 0, rec), remoteClient1.ToString());

                // sends data
                server_socket.SendTo(outBuffer, remoteClient1);
                server_socket.SendTo(outBuffer, remoteClient2);
                server_socket.SendTo(outBuffer, remoteClient3);
                server_socket.SendTo(outBuffer, remoteClient4);

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException: {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                if (!ignoreError10035 || (ignoreError10035 && se.ErrorCode != 10035))
                    Console.WriteLine("SocketException: {0}", se.ToString());
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
        ~UdpServer4()
        {
            ShutdownServer();
        }
    }
}
