using System;
using System.Net;
using System.Net.Sockets;

namespace SkaiaLib.Base
{
    public abstract class BaseSocket
    {
        protected abstract Socket Socket { get; set; }
        protected abstract EndPoint LocalEndpoint { get; set; }

        public abstract void BindSocket(EndPoint localEndpoint);
        public abstract bool Poll(int timeout);
        public abstract int Receive(byte[] buffer, int length, out EndPoint sender);
        public abstract int Send(byte[] buffer, int length, EndPoint sender);

        /// <summary>
        /// This is a sneaky way to get your own IP. It will connect to Google's DNS server at 8.8.8.8 and get the socket's local endpoint.
        /// This also ensures that you use the PC's preferred endpoint.
        /// </summary>
        /// <seealso cref="https://stackoverflow.com/a/27376368"/>
        /// <returns>Your local IP address to use as a socket bound endpoint.</returns>
        // TODO: Change this to a more robust solution.
        protected IPAddress GetLocalAddress()
        {
            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    return endPoint.Address;
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("SkaiaLib [FATAL] => Couldn't find local IP. Defaulting to 127.0.0.1 | Ex: " + ex.StackTrace);
                return IPAddress.Parse("127.0.0.1");
            }
        }

        /// <summary>
        /// Thanks to the network freelancer, according to him everything fails without this.
        /// </summary>
        /// <param name="s"></param>
        protected void SetConnReset(Socket s)
        {
            try
            {
                const uint IOC_IN = 0x80000000;
                const uint IOC_VENDOR = 0x18000000;
                uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                s.IOControl((int)SIO_UDP_CONNRESET, new Byte[] { Convert.ToByte(false) }, null);
            }
            catch { }
        }
    }
}
