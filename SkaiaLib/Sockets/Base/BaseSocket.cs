
// -----------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Base socket implementation.
// -----------------------------------------

using Skaia.Logging;
using Skaia.Surrogates;
using System;
using System.Net;
using System.Net.Sockets;

namespace Skaia.Sockets
{
    public abstract class BaseSocket
    {
        public abstract Socket Socket { get; protected set; }
        protected abstract byte[] RecvBuffer { get; }
        protected abstract EndPoint LocalEndpoint { get; set; }
        protected abstract SafeQueue<Packet> InQueue { get; }
        protected abstract SafeQueue<Packet> OutQueue { get; }

        public abstract bool Poll(int timeout);
        public abstract int Receive(byte[] buffer, int length, out EndPoint sender);
        public abstract int Send(byte[] data, int length, EndPoint receiver);
        public abstract void BindSocket(EndPoint localEndpoint);
        public abstract void Loop();

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
