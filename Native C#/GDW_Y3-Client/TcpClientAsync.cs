/*
 * Name:
 * Date:
 * Description:
 * References:
 *  - https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-client-socket-example
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NetworkLibrary
{
    public class TcpClientAsync
    {
        // enum for mode
        public enum mode { both, send, receive };

        // communication mode
        public mode commMode = mode.send;

        // client variables
        private byte[] outBuffer; // sending data to server
        private byte[] inBuffer; // getting data from a server

        // default buffer size
        private int defaultBufferSize = 512;

        private IPAddress ip;

        private IPEndPoint remote = null;
        private Socket client_socket = null;

        // the ip address for the object.
        string ipAddress = "";

        // port
        private int port = 11111;

        // if 'true', asyncronous sockets are used.
        // private bool asyncSockets = true;

        // event resets
        private ManualResetEvent resetConnectEvent = new ManualResetEvent(false);
        private ManualResetEvent resetSendEvent = new ManualResetEvent(false);
        private ManualResetEvent resetReceiveEvent = new ManualResetEvent(false);

        // checks to see if the server is running
        private bool running = false;

        // constructor
        public TcpClientAsync()
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

        // connect callback
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket localSocket = (Socket)ar.AsyncState;

                // connection has been ended.
                localSocket.EndConnect(ar);

                resetConnectEvent.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // sends callback
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // gets socket
                Socket localSocket = (Socket)ar.AsyncState;

                // gets data
                int rec = localSocket.EndSend(ar);

                // all bytes have been sent.
                resetSendEvent.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // receive callback
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket.  
                Socket localSocket = (Socket)ar.AsyncState;

                // ends receiver 
                int rec = localSocket.EndReceive(ar);

                resetReceiveEvent.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // runs the client
        public void RunClient()
        {
            // setting out buffer if it has not been established.
            if (outBuffer == null)
                outBuffer = new byte[defaultBufferSize];

            // setting up the in buffer
            if (inBuffer == null)
                inBuffer = new byte[defaultBufferSize];

            try
            {
                // host
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

                // setting up the two end points
                remote = new IPEndPoint(ip, port);

                // generates socket
                client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // connects the tcp socket
                client_socket.BeginConnect(remote, new AsyncCallback(ConnectCallback), client_socket);
                resetConnectEvent.WaitOne();

                running = true;

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


        // updates the client
        public void Update()
        {
            try
            {
                // NOTE: this stalls the user for a resposne when they send the first time, but after that they don't have to wiat.
                // sends the data
                if (commMode == mode.both || commMode == mode.send)
                {
                    client_socket.BeginSend(outBuffer, 0, outBuffer.Length, 0, new AsyncCallback(SendCallback), client_socket);
                    resetSendEvent.WaitOne();
                }

                // receives the data
                if (commMode == mode.both || commMode == mode.receive)
                {
                    client_socket.BeginReceive(inBuffer, 0, inBuffer.Length, 0, new AsyncCallback(ReceiveCallback), client_socket);
                    resetReceiveEvent.WaitOne();
                    // Console.WriteLine("Received: {0} from Server: {1}", Encoding.ASCII.GetString(inBuffer, 0, rec), remote.ToString());
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
                Console.WriteLine("Unexpected exception: {0}", e.ToString());
            }
        }

        // shuts down the client
        public void ShutdownClient()
        {
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
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        // destructor
        ~TcpClientAsync()
        {
            ShutdownClient();
        }
    }
}
