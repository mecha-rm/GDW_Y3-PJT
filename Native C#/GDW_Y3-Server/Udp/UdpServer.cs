using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace NetworkLibrary
{
    // this is made static in the DSN work.
    // this isn't static since only one instance of the plugin can exist at a time.
    // this is a UDP server
    public class UdpServer
    {
        // enum for mode
        public enum mode { both, send, receive };

        // name of server
        private string serverName = "";

        // communication mode
        public mode commMode = mode.both;

        // default buffer size
        private int defaultBufferSize = 512;

        // server variables
        private byte[] outBuffer;
        private byte[] inBuffer;


        // server variables
        private IPAddress ip;
        private IPEndPoint client = null;
        private Socket server_socket = null;
        private EndPoint remoteClient = null;

        // the ip address for the object.
        string ipAddress = "";

        // port
        private int port = 11111;

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
        public UdpServer()
        {

        }

        // gets the server name
        public string GetServerName()
        {
            return serverName;
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
        public void SetSendBufferData(byte[] data, bool deleteOldData = false)
        {
            if(outBuffer != null && deleteOldData) // out buffer exists
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
            if(!running) // server is not running
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
            if(server_socket != null)
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

            // reading in data
            if (inBuffer == null)
                inBuffer = new byte[defaultBufferSize];

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
            client = new IPEndPoint(ip, port); // 0 for any available port.
            remoteClient = client;

            // 
            try
            {
                // the server listens and provides a service.
                server_socket.Bind(localEP);

                // for two-way communication
                // server_socket.Listen(16); // backlog

                // gets the client socket
                // client_socket = server_socket.Accept();

                // prints different message based on selected mode.
                
                switch(commMode)
                {
                    case mode.both:
                        Console.WriteLine("Waiting for, and prepared to send data...");
                        break;

                    case mode.send:
                        Console.WriteLine("Ready to send data...");
                        break;

                    case mode.receive:
                        Console.WriteLine("Waiting for data...");
                        break;
                }
                

                // sets timeout variables.
                server_socket.ReceiveTimeout = receiveTimeout;
                server_socket.SendTimeout = sendTimeout;

                // non-blocking if false (recommended)
                server_socket.Blocking = blockingSockets;

                // the server is running
                running = true;

                // moved to update loop
                // // added in while loop 
                // while (true)
                // {
                //     // TCP we use "Receive", and for UDP we use "ReceiveFrom" (you probably wrote this wrong)
                //     // 'ref' means passing by reference.
                //     int rec = server.ReceiveFrom(buffer, ref remoteClient);
                // 
                //     Console.WriteLine("Received: {0} from Client: {1}", Encoding.ASCII.GetString(buffer, 0, rec), remoteClient.ToString());
                // 
                //     // would give you a float
                //     // the client has to convert from float to byte using... BitConverter.GetBytes();
                //     // BitConverter.ToSingle(buffer, 0);
                // }
                // 
                // // closing server
                // server.Shutdown(SocketShutdown.Both);
                // server.Close();

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
            // checks to see if the server is running.
            if (!running)
            {
                Console.WriteLine("The server has not been started. Call RunServer().");
                return;
            }
                

            // RECEIVE
            try
            {
                // receives the data if connected
                // if((commMode == mode.both || commMode == mode.receive) && server_socket.Connected)
                // {
                //    int rec = server_socket.ReceiveFrom(inBuffer, ref remoteClient);
                // 
                //     Console.WriteLine("Received: {0} from Client: {1}", Encoding.ASCII.GetString(inBuffer, 0, rec), remoteClient.ToString());
                // }

                // NOTE: if put in the if statement, this doesn't work for some reason.
                int rec = server_socket.ReceiveFrom(inBuffer, ref remoteClient);

                // Console.WriteLine("Received: {0} from Client: {1}", Encoding.ASCII.GetString(inBuffer, 0, rec), remoteClient.ToString());

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


            // SEND
            try
            {
                // sends the data
                if (commMode == mode.both || commMode == mode.send)
                {
                    // client socket has not been established yet.
                    // if(client_socket == null)
                    // {
                    //     client_socket = server_socket.Accept();
                    // }
                    //     
                    // if(client_socket != null)
                    //     client_socket.Send(outBuffer);

                    server_socket.SendTo(outBuffer, remoteClient);
                }

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
            // used to see if the server was ever actually started.
            if (!running)
            {
                Console.WriteLine("The server is not currently running.");
                return;
            }


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
        ~UdpServer()
        {
            ShutdownServer();
        }
    }
}
