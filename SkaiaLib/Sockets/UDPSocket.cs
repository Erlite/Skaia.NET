
// ----------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Socket implementation to use UDP over IPv4.
// ----------------------------------------------------

using SkaiaLib.Logging;
using SkaiaLib.Utils;
using System;
using System.Net;
using System.Net.Sockets;
using SkaiaLib.Surrogates;

namespace SkaiaLib.Sockets
{
    public class UDPSocket : BaseSocket
    {
        private SafeQueue<Packet> inQueue;
        private SafeQueue<Packet> outQueue;

        public Socket BaseSocket { get { return Socket; } }

        protected sealed override Socket Socket { get; set; }
        protected sealed override EndPoint LocalEndpoint { get; set; }
        protected override byte[] RecvBuffer { get; } = new byte[4096];
        protected override SafeQueue<Packet> InQueue { get { return inQueue; } }
        protected override SafeQueue<Packet> OutQueue { get { return outQueue; } }

        public sealed override void BindSocket(EndPoint localEndpoint)
        {
            // Create a new IPv4 UDP Socket.
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
            {
                Blocking = false
            };

            // Bind the local endpoint to the socket.
            Socket.Bind(localEndpoint);
            LocalEndpoint = Socket.LocalEndPoint;

            SkaiaLogger.LogMessage(MessageType.Info, $"Bound socket to {localEndpoint.ToString()}");
            // Set the connection reset.
            NetUtils.SetConnReset(Socket);
        }
    }
}