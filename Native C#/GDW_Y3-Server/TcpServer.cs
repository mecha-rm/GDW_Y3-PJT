/*
 * Name:
 * Date:
 * Description:
 * References:
 *  - https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example
 *  - https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.socket.beginreceive?view=net-5.0
 */

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NetworkLibrary
{
    // this is made static in the DSN work. Note that this is an asynchronous socket server.
    // this isn't static since only one instance of the plugin can exist at a time.
    // this is a UDP server
    public class TcpServer
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
        // private IPEndPoint client = null;
        private Socket server_socket = null;
        private Socket client_socket = null;
        private IPEndPoint remoteClient = null;

        public ManualResetEvent resetEvent = new ManualResetEvent(false);

        // the ip address for the object.
        string ipAddress = "";

        // port
        private int port = 11111;

        // backlog for server.
        private int backlog = 16;

        // if 'true', asyncronous sockets are used.
        // private bool asyncSockets = true;

        // checks to see if the server is running
        private bool running = false;

        // constructor
        public TcpServer()
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
        public void SetReceiveBufferData(byte[] data, bool deleteOldData = false)
        {
            if (inBuffer != null && deleteOldData) // in buffer exists
                Array.Clear(inBuffer, 0, inBuffer.Length);

            inBuffer = data;
        }

        // clears the send buffer of its data, replacing it with an array of size 0 if applicable.
        // if the buffer hasn't been set, nothing happens.
        public void ClearSendBuffer(bool setSize0 = true)
        {
            if (outBuffer == null)
                return;

            Array.Clear(outBuffer, 0, outBuffer.Length);

            if (setSize0)
                outBuffer = new byte[0];
        }

        // clears the send buffer of its data, replacing it with an array of size 0 if applicable.
        // if the buffer hasn't been set, nothing happens.
        public void ClearReceiveBuffer(bool setSize0 = true)
        {
            if (inBuffer == null)
                return;

            Array.Clear(inBuffer, 0, inBuffer.Length);

            if (setSize0)
                inBuffer = new byte[0];
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

        // gets hte listen backlog
        public int GetListenBacklog()
        {
            return backlog;
        }


        // sets the listen backlog number
        // this only sets the value
        public void SetListenBacklog(int newBacklog)
        {
            if (running)
                backlog = newBacklog;
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

        // accept callback
        public void AcceptCallback(IAsyncResult ar)
        {
            // main thread should continue 
            resetEvent.Set();

            // Get the socket that handles the client request.  
            Socket asyncState = (Socket)ar.AsyncState;
            client_socket = asyncState.EndAccept(ar);
        }

        // read callback
        public void ReceiveCallback(IAsyncResult ar)
        {
            // the local socket
            Socket localSocket = (Socket)ar.AsyncState;

            int rec = localSocket.EndReceive(ar);

            // data received
            // if(rec > 0)
            // {
            //     // guide had a check to see if all data has been received.
            // 
            //     localSocket.BeginSend(outBuffer, 0, outBuffer.Length, 0,
            // new AsyncCallback(SendCallback), localSocket);
            // 
            // }
        }

        // send callback
        public void SendCallback(IAsyncResult ar)
        {
            try
            {
                // gets socket from async  
                Socket localSocket = (Socket)ar.AsyncState;

                // send data to remote device, getting how many bytes was sent.  
                int rec = localSocket.EndSend(ar);

                // is this necessary?
                // localSocket.Shutdown(SocketShutdown.Both);
                // localSocket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
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
                // client_socket = server_socket.Accept();
                // remoteClient = (IPEndPoint)client_socket.RemoteEndPoint;

                // gets the client socket
                // client_socket = server_socket.Accept();


                Console.WriteLine("Server setup complete. Waiting for connection...");

                // non signal state
                resetEvent.Reset(); // threads now block

                server_socket.BeginAccept(new AsyncCallback(AcceptCallback), server_socket);

                // wait until a client connects to continue.
                resetEvent.WaitOne();

                Console.WriteLine("Connection Made");

                // switch (commMode)
                // {
                //     case mode.both:
                //         Console.WriteLine("Waiting for, and prepared to send data...");
                //         break;
                // 
                //     case mode.send:
                //         Console.WriteLine("Ready to send data...");
                //         break;
                // 
                //     case mode.receive:
                //         Console.WriteLine("Waiting for data...");
                //         break;
                // }

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

                // original, always received.
                // localSocket.BeginReceive(inBuffer, 0, inBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), localSocket);

                // if the communication mode is set to either be two way or receive only
                if (commMode == mode.both || commMode == mode.receive)
                {
                    client_socket.BeginReceive(inBuffer, 0, inBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), client_socket);
                }

                // if the communication mode is set to either be two way or send only
                if (commMode == mode.both || commMode == mode.send)
                {
                    client_socket.BeginSend(outBuffer, 0, outBuffer.Length, 0, new AsyncCallback(SendCallback), client_socket);
                }

                // // receives the data if connected
                // // if((commMode == mode.both || commMode == mode.receive) && server_socket.Connected)
                // // {
                // //    int rec = server_socket.ReceiveFrom(inBuffer, ref remoteClient);
                // // 
                // //     Console.WriteLine("Received: {0} from Client: {1}", Encoding.ASCII.GetString(inBuffer, 0, rec), remoteClient.ToString());
                // // }
                // 
                // // NOTE: if put in the if statement, this doesn't work for some reason.
                // int rec = client_socket.Receive(inBuffer);
                // 
                // // Console.WriteLine("Received: {0} from Client: {1}", Encoding.ASCII.GetString(inBuffer, 0, rec), remoteClient.ToString());
                // 
                // // sends the data
                // if (commMode == mode.both || commMode == mode.send)
                // {
                //     // client socket has not been established yet.
                //     // if(client_socket == null)
                //     // {
                //     //     client_socket = server_socket.Accept();
                //     // }
                //     //     
                //     // if(client_socket != null)
                //     //     client_socket.Send(outBuffer);
                // 
                //     client_socket.Send(outBuffer);
                // }

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
            if (client_socket != null)
            {
                client_socket.Shutdown(SocketShutdown.Both);
                client_socket.Close();
                running = false;

                Console.WriteLine("Server Shutdown");
            }

        }

        // destructor
        ~TcpServer()
        {
            ShutdownServer();
        }
    }
}
