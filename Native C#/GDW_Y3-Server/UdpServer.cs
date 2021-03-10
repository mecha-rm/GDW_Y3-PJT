using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace GDW_Y3_Server
{
    // this is made static in the DSN work.
    // this isn't static since only one instance of the plugin can exist at a time.
    // this is a UDP server
    public class UdpServer
    {
        // enum for mode
        public enum mode { both, send, receive };

        // communication mode
        public mode commMode = mode.receive;

        // server variables
        private byte[] outBuffer;
        private byte[] inBuffer;
        private IPAddress ip;

        private IPEndPoint client = null;
        private Socket server_socket = null;
        private EndPoint remoteClient = null;

        // the ip address for the object.
        string ipAddress = "";

        // port
        // private int port;

        // if 'true', sockets are being blocked.
        // it errors out if set to false by default when there is no connection being made.
        private bool blockingSockets = true;

        // timeout variables
        // these error out if set to 0 by default when there is no connection being made.
        private int receiveTimeout = 0, sendTimeout = 0;

        // checks to see if the server is running
        private bool running = false;

        // the client socket for two-way communication
        Socket client_socket = null;

        // constructor
        public UdpServer()
        {

        }

        // gets the communication mode
        // public mode GetCommunicationMode()
        // {
        //     return commMode;
        // }
        // 
        // // sets the communication mode
        // public void SetCommunicationMode(mode newMode)
        // {
        //     commMode = newMode;
        // }

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
            if(size > 0)
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
            if (size > 0)
                inBuffer = new byte[size];
        }

        // gets the send buffer data
        public byte[] GetSendBufferData()
        {
            return outBuffer;
        }

        // gets the receive buffer data
        public byte[] GetReceiveBufferData()
        {
            return inBuffer;
        }

        // sets the receive buffer data
        public void SetReceiveBufferData(byte[] data)
        {
            inBuffer = data;
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
        public int GetReceiverTimeout()
        {
            return receiveTimeout;
        }

        // sets the receiver timeout.
        public void SetReceiverTimeout(int newRt)
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

            // reading in data
            if (inBuffer == null)
                inBuffer = new byte[512];

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
            IPEndPoint localEP = new IPEndPoint(ip, 11111);

            // last class the family was entered, but you can get from the ip directly the Address family.
            server_socket = new Socket(ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            // 0 for any available port.
            client = new IPEndPoint(IPAddress.Any, 0); // 0 for any available port.
            remoteClient = (EndPoint)client;

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
            try
            {
                // the variable for sending and reading data
                int rec;

                // receives the data
                if(commMode == mode.both || commMode == mode.receive)
                {
                    rec = server_socket.ReceiveFrom(inBuffer, ref remoteClient);

                    Console.WriteLine("Received: {0} from Client: {1}", Encoding.ASCII.GetString(inBuffer, 0, rec), remoteClient.ToString());
                }

                // sends the data
                if (commMode == mode.both || commMode == mode.send)
                {
                    client_socket.Send(outBuffer);
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
            if(server_socket != null)
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
