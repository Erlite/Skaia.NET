
// ----------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Socket implementation to use UDP over IPv4.
// ----------------------------------------------------

using SkaiaLib.Base;
using SkaiaLib.Logging;
using System;
using System.Net;
using System.Net.Sockets;

namespace SkaiaLib.Sockets
{
    public class UDPSocket : BaseSocket
    {
        public Socket BaseSocket { get { return Socket; } }

        protected sealed override Socket Socket { get; set; }
        protected sealed override EndPoint LocalEndpoint { get; set; }

        private EndPoint recvEndpoint;

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
            recvEndpoint = (EndPoint)new IPEndPoint(IPAddress.Any, 0);

            SkaiaLogger.LogMessage(MessageType.Info, $"Bound socket to {localEndpoint.ToString()}");
            // Set the connection reset.
            SetConnReset(Socket);
        }

        public sealed override bool Poll(int timeout)
        {
            try
            {
                return Socket.Poll(timeout, SelectMode.SelectRead);
            }
            catch (Exception ex)
            {
                SkaiaLogger.LogMessage(MessageType.Error, "Failed to poll on socket", ex);
                return false;
            }
        }

        public sealed override int Receive(byte[] buffer, int length, out EndPoint sender)
        {
            int recvBytes = Socket.ReceiveFrom(buffer, 0, length, SocketFlags.None, ref recvEndpoint);

            if (recvBytes > 0)
            {
                sender = recvEndpoint;
                return recvBytes;
            }

            sender = null;
            return -1;
        }

        public sealed override int Send(byte[] buffer, int length, EndPoint receiver)
        {
            return Socket.SendTo(buffer, 0, length, SocketFlags.None, receiver);
        }
    }
}