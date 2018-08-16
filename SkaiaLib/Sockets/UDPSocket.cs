
// ----------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Socket implementation to use UDP over IPv4.
// ----------------------------------------------------

using Skaia.Logging;
using Skaia.Surrogates;
using Skaia.Utils;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Skaia.Core;
using Skaia.Events;

namespace Skaia.Sockets
{
    public sealed class UDPSocket : NetSocket
    {
        private EndPoint recvEndpoint;
        private ConcurrentQueue<Packet> _inQueue = new ConcurrentQueue<Packet>();
        private ConcurrentQueue<Packet> _outQueue = new ConcurrentQueue<Packet>();
        private Socket _socket;

        public override Socket Socket { get { return _socket; } protected set { _socket = value; } }

        protected override Action<byte[], SEndPoint> RawDataReceived { get; set; }
        protected override EndPoint LocalEndpoint { get; set; }
        protected override byte[] RecvBuffer { get; } = new byte[4096];
        protected override ConcurrentQueue<Packet> InQueue { get { return _inQueue; } }
        protected override ConcurrentQueue<Packet> OutQueue { get { return _outQueue; } }

        /// <summary>
        /// Bind the socket to the local endpoint.
        /// </summary>
        /// <param name="localEndpoint"></param>
        public override void BindSocket(EndPoint localEndpoint)
        {
            // Create a new IPv4 UDP Socket.
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
            {
                Blocking = true
            };

            // Bind the local endpoint to the socket.
            Socket.Bind(localEndpoint);
            LocalEndpoint = Socket.LocalEndPoint;

            SkaiaLogger.LogMessage(MessageType.Info, $"Bound socket to {localEndpoint.ToString()}");
            // Set the connection reset.

            RawDataReceived = new Action<byte[], SEndPoint>(DispatchRawPacket);
            NetUtils.SetConnReset(Socket);
        }

        /// <summary>
        /// Send data to the socket.
        /// </summary>
        /// <param name="data"> The data to send. </param>
        /// <param name="length"> The amount of data to send. Set to data length to send everything. </param>
        /// <param name="receiver"> The data's receiver. </param>
        /// <returns></returns>
        public override int Send(byte[] data, int length, EndPoint receiver)
        {
            return Socket.SendTo(data, 0, length, SocketFlags.None, receiver);
        }

        /// <summary>
        /// Receive data on the socket. This will block the calling method until it finds something to receive.
        /// </summary>
        /// <param name="buffer"> The buffer to copy the data onto. </param>
        /// <param name="length"> The amount of data to copy. </param>
        /// <param name="sender"> The data's sender. </param>
        /// <returns> The data received or null if nothing. </returns>
        public override byte[] Receive(byte[] buffer, int length, out EndPoint sender)
        {
            if (!Socket.IsBound)
                throw new InvalidOperationException();

            sender = null;

            try
            {
                // Wait indefinitely to receive something on the socket.
                if (Socket.Poll(-1, SelectMode.SelectRead))
                {
                    // Get the length of the packet received.
                    int pcktLength = Socket.ReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref recvEndpoint);

                    // If for some odd reason it's empty, we'll just discard it and return an empty byte array.
                    if (pcktLength > 0)
                    {
                        byte[] data = new byte[pcktLength];
                        Buffer.BlockCopy(buffer, 0, data, 0, data.Length);
                        sender = recvEndpoint;
                        return data;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // This probably happens if the socket isn't bound.
                SkaiaLogger.LogMessage(MessageType.Error, "Failed to poll and receive on socket.", ex);
                sender = null;
            }

            return null;
        }


        /// <summary>
        /// Handles receiving packets. Should be called in a new Task to avoid blocking the main thread.
        /// </summary>
        public override void ReceiveLoop()
        {
            while (true)
            {
                // Wait to receive something on the socket.
                EndPoint sender = null;
                byte[] data = Receive(RecvBuffer, RecvBuffer.Length, out sender);

                // Invoke the data received event.
                IPEndPoint endPoint = sender as IPEndPoint;
                RawDataReceived?.Invoke(data, SEndPoint.Create(endPoint.Address, endPoint.Port));
            }
        }


        public override void SendLoop()
        {
            throw new NotImplementedException();
        }

        private void DispatchRawPacket(byte[] packet, SEndPoint sender)
        {
            // TODO: Implement support for multiple netevents in a single packet.
            // Convert the two byte identifier to a ushort.
            ushort id = BitConverter.ToUInt16(packet.SubArray(0, 2), 0);

            // Try to find the type in the registered types dictionary.
            Type packetType;
            if (TypeManager.TryGetTypeFromID(id, out packetType))
            {
                NetworkEvent netEvent = new NetworkEvent(packet);
            }
        }
    }
}