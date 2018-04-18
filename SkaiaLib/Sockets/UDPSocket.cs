
// ----------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Socket implementation to use UDP over IPv4.
// ----------------------------------------------------

using Skaia.Logging;
using Skaia.Utils;
using System;
using System.Net;
using System.Net.Sockets;
using Skaia.Surrogates;

namespace Skaia.Sockets
{
    public sealed class UDPSocket : BaseSocket
    {
        private EndPoint recvEndpoint;
        private SafeQueue<Packet> inQueue = new SafeQueue<Packet>();
        private SafeQueue<Packet> outQueue = new SafeQueue<Packet>();
        private Socket socket;

        public override Socket Socket { get { return socket; } protected set { socket = value; } }

        protected sealed override EndPoint LocalEndpoint { get; set; }
        protected override byte[] RecvBuffer { get; } = new byte[4096];
        protected override SafeQueue<Packet> InQueue { get { return inQueue; } }
        protected override SafeQueue<Packet> OutQueue { get { return outQueue; } }

        /// <summary>
        /// Bind the socket to the local endpoint.
        /// </summary>
        /// <param name="localEndpoint"></param>
        public override void BindSocket(EndPoint localEndpoint)
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

        /// <summary>
        /// Poll the socket to see if anything is waiting to be received.
        /// </summary>
        /// <param name="timeout"> The amount of time to wait for a response. 1 for instant check. </param>
        /// <returns> True if something is waiting to be received. </returns>
        public override bool Poll(int timeout)
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

        /// <summary>
        /// Send data to the socket.
        /// </summary>
        /// <param name="data"> The data to send. </param>
        /// <param name="length"> The amount of data to send. Set to data length to send everything. </param>
        /// <param name="sender"> The data's receiver. </param>
        /// <returns></returns>
        public override int Send(byte[] data, int length, EndPoint receiver)
        {
            return Socket.SendTo(data, 0, length, SocketFlags.None, receiver);
        }

        /// <summary>
        /// Receive awaiting data from the socket.
        /// </summary>
        /// <param name="buffer"> The buffer to copy the data onto. </param>
        /// <param name="length"> The amount of data to copy. </param>
        /// <param name="sender"> The data's sender. </param>
        /// <returns> The amount of data copied. -1 if nothing. </returns>
        public override int Receive(byte[] buffer, int length, out EndPoint sender)
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


        /// <summary>
        /// To be called once, and in a new thread to avoid blocking.
        /// Handles receiving/sending packets.
        /// </summary>
        public override void Loop()
        {
            while (true)
            {
                // If something has been received on the socket.
                if (Poll(1))
                {
                    // Read the data
                    int recvBytes = Receive(RecvBuffer, RecvBuffer.Length, out EndPoint sender);

                    // If there's more than 0 bytes copy the data into the buffer and enqueue.
                    if (recvBytes > 0)
                    {
                        byte[] data = new byte[recvBytes];
                        Buffer.BlockCopy(RecvBuffer, 0, data, 0, recvBytes);
                        Packet packet = new Packet { Data = data, Endpoint = sender };
                        EnqueueReceivedPacket(packet);
                    }
                }

                // Send queued data
                while (DequeueTransmitPacketQueue(out Packet sendPckt))
                {
                    Send(sendPckt.Data, sendPckt.Data.Length, sendPckt.Endpoint);
                }
            }
        }
    }
}