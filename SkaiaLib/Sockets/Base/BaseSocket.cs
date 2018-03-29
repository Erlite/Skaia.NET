
// -----------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Base socket implementation.
// -----------------------------------------

using SkaiaLib.Logging;
using SkaiaLib.Surrogates;
using System;
using System.Net;
using System.Net.Sockets;

namespace SkaiaLib.Sockets
{
    public abstract class BaseSocket
    {
        private EndPoint recvEndpoint;
        protected abstract byte[] RecvBuffer { get; }
        protected abstract EndPoint LocalEndpoint { get; set; }
        protected abstract SafeQueue<Packet> InQueue { get; }
        protected abstract SafeQueue<Packet> OutQueue { get; }
        protected abstract Socket Socket { get; set; }

        public abstract void BindSocket(EndPoint localEndpoint);

        /// <summary>
        /// Poll the socket to see if anything is waiting to be received.
        /// </summary>
        /// <param name="timeout"> The amount of time to wait for a response. 1 for instant check. </param>
        /// <returns> True if something is waiting to be received. </returns>
        public virtual bool Poll(int timeout)
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
        /// Receive awaiting data from the socket.
        /// </summary>
        /// <param name="buffer"> The buffer to copy the data onto. </param>
        /// <param name="length"> The amount of data to copy. </param>
        /// <param name="sender"> The data's sender. </param>
        /// <returns> The amount of data copied. -1 if nothing. </returns>
        public virtual int Receive(byte[] buffer, int length, out EndPoint sender)
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
        /// Send data to the socket.
        /// </summary>
        /// <param name="data"> The data to send. </param>
        /// <param name="length"> The amount of data to send. Set to data length to send everything. </param>
        /// <param name="sender"> The data's receiver. </param>
        /// <returns></returns>
        public virtual int Send(byte[] data, int length, EndPoint receiver)
        {
            return Socket.SendTo(data, 0, length, SocketFlags.None, receiver);
        }

        /// <summary>
        /// To be called once, and in a new thread to avoid blocking.
        /// Handles receiving/sending packets.
        /// </summary>
        public virtual void Loop()
        {
            while (true)
            {
                // If something has been received on the socket.
                if (Poll(1))
                {
                    // Read the data
                    int recvBytes = Receive(RecvBuffer, RecvBuffer.Length, out EndPoint sender);

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
                    socket.Send(sendPckt.Data, sendPckt.Data.Length, sendPckt.Endpoint);
                }
            }
        }

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
