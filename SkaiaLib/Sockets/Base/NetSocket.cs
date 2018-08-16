
// -----------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Base socket implementation.
// -----------------------------------------

using Skaia.Logging;
using Skaia.Surrogates;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Skaia.Sockets
{
    /// <summary>
    /// The lowest level of sockets, can be used to create your own socket behaviour for the NetworkManager.
    /// </summary>
    public abstract class NetSocket
    {
        private EndPoint _recvEndpoint;

        public abstract Socket Socket { get; protected set; }

        // TODO: Change this to an ArrayPool.
        protected abstract byte[] RecvBuffer { get; }
        protected abstract Action<byte[], SEndPoint> RawDataReceived { get; set; }
        protected abstract EndPoint LocalEndpoint { get; set; }
        protected abstract ConcurrentQueue<Packet> InQueue { get; }
        protected abstract ConcurrentQueue<Packet> OutQueue { get; }

        public abstract byte[] Receive(byte[] buffer, int length, out EndPoint sender);
        public abstract int Send(byte[] data, int length, EndPoint receiver);
        public abstract void BindSocket(EndPoint localEndpoint);
        public abstract void SendLoop();
        public abstract void ReceiveLoop();

        /// <summary>
        /// Add a received packet to the queue.
        /// </summary>
        /// <param name="packet"> The packet to enqueue. </param>
        protected virtual void EnqueueReceivedPacket(Packet packet)
        {
            InQueue.Enqueue(packet);
        }


        /// <summary>
        /// Retrieve a packet from the received packets queue.
        /// </summary>
        /// <param name="packet"> The retrieved packet if any. </param>
        /// <returns> True on success. </returns>
        public virtual bool DequeueReceivedPacketQueue(out Packet packet)
        {
            return InQueue.TryDequeue(out packet);
        }

        /// <summary>
        /// Queue a packet for sending.
        /// </summary>
        /// <param name="packet"> Packet to queue. </param>
        public virtual void EnqueuePacketToSend(Packet packet)
        {
            OutQueue.Enqueue(packet);
        }

        /// <summary>
        /// Retrieve a packet from the packets to transmit queue.
        /// </summary>
        /// <param name="packet"> The retrieved packet if any. </param>
        /// <returns> True on success. </returns>
        protected virtual bool DequeueTransmitPacketQueue(out Packet packet)
        {
            return OutQueue.TryDequeue(out packet);
        }
    }
}
