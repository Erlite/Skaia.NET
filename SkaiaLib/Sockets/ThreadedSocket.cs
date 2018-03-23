using SkaiaLib.Surrogates;
using System;
using System.Net;
using System.Threading;

namespace SkaiaLib.Sockets
{
    /// <summary>
    /// Highest socket implementation for SkaiaLib.
    /// Sockets will lock a thread, don't want that happening so we run them in a new thread.
    /// </summary>
    public sealed class ThreadedSocket
    {
        Thread thread;
        UDPSocket socket;
        byte[] recvBuffer = new byte[4096];

        SafeQueue<Packet> inQueue = new SafeQueue<Packet>();
        SafeQueue<Packet> outQueue = new SafeQueue<Packet>();

        /// <summary>
        /// Start the socket in a new thread.
        /// </summary>
        /// <param name="endpoint"> The local endpoint. </param>
        public void Start(int port)
        {
            thread = new Thread(ThreadLoop)
            {
                IsBackground = true,
                Name = "Socket Thread"
            };

            thread.Start(port);
        }

        /// <summary>
        /// The socket loop which pulls received data and sends the queued data.
        /// </summary>
        /// <param name="endpoint"></param>
        void ThreadLoop(object port)
        {
            socket = new UDPSocket();
            socket.BindSocket(new IPEndPoint(socket.GetLocalIP(), (int)port));

            while (true)
            {
                // If something has been received on the socket.
                if (socket.Poll(1))
                {
                    EndPoint sender;
                    // Read the data
                    int recvBytes = socket.Receive(recvBuffer, recvBuffer.Length, out sender);

                    if (recvBytes > 0)
                    {
                        byte[] data = new byte[recvBytes];
                        Buffer.BlockCopy(recvBuffer, 0, data, 0, recvBytes);
                        Packet packet = new Packet { Data = data, Endpoint = sender };
                        EnqueueReceivedPacket(packet);
                    }

                    Packet sendPckt;
                    // Send queued data
                    while (DequeueTransmitPacketQueue(out sendPckt))
                    {
                        socket.Send(sendPckt.Data, sendPckt.Data.Length, sendPckt.Endpoint);
                    }
                }
            }
        }

        /// <summary>
        /// Add a received packet to the queue.
        /// </summary>
        /// <param name="packet"> The packet to enqueue. </param>
        void EnqueueReceivedPacket(Packet packet) => inQueue.Enqueue(packet);

        /// <summary>
        /// Retrieve a packet from the received packets queue.
        /// </summary>
        /// <param name="packet"> The retrieved packet if any. </param>
        /// <returns> True on success. </returns>
        public bool DequeueReceivedPacketQueue(out Packet packet)
        {
            return inQueue.TryDequeue(out packet);
        }

        /// <summary>
        /// Queue a packet for sending.
        /// </summary>
        /// <param name="packet"> Packet to queue. </param>
        public void EnqueuePacketToSend(Packet packet) => outQueue.Enqueue(packet);

        /// <summary>
        /// Retrieve a packet from the packets to transmit queue.
        /// </summary>
        /// <param name="packet"> The retrieved packet if any. </param>
        /// <returns> True on success. </returns>
        bool DequeueTransmitPacketQueue(out Packet packet)
        {
            return outQueue.TryDequeue(out packet);
        }
    }
}
