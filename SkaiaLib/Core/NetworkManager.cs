
// ---------------------------------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Handle receiving/sending packets and keeping virtual connections alive.
// ---------------------------------------------------------------------------

using Skaia.Logging;
using Skaia.Sockets;
using Skaia.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace Skaia.Core
{
    /// <summary>
    /// Core of Skaia.NET, handles package routing, virtual connections and anything that involves handling server/clients.
    /// </summary>
    public static class NetworkManager
    {
        private static BaseSocket CoreSocket { get; set; }
        private static Node localNode;

        /// <summary>
        /// List of nodes on the current network.
        /// </summary>
        // TODO: Make a custom container for the Node list to allow checking for stupid things like two nodes with the same ID.
        public static List<Node> Nodes { get; private set; } = new List<Node>();
        /// <summary>
        /// The dispatcher that handles routing network events into their respective callbacks.
        /// </summary>
        // TODO: Change this to a Dispatcher<NetEvent>
        public static Dispatcher<Packet> Dispatcher { get; private set; } = new Dispatcher<Packet>();

        /// <summary>
        /// The thread on which the socket runs.
        /// </summary>
        // TODO: Might be a good idea to make this internal or private?
        public static Thread SocketThread { get; private set; }

        /// <summary>
        /// Returns the current state of the NetworkManager.
        /// </summary>
        public static bool IsStarted { get; private set; }

        /// <summary>
        /// The local machine's node.
        /// </summary>
        public static Node LocalNode
        {
            get
            {
                if (!IsStarted)
                {
                    throw new InvalidOperationException("Cannot retrieve the local node when the NetworkManager isn't started.");
                }

                return localNode;
            }
            private set { localNode = value; }
        }


        // TODO: Refactor this to have server/client methods.
        // TODO: And local node.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void Start(int port) => Start(new UDPSocket(), port);
        public static void Start(BaseSocket socket, int port)
        {
            if (IsStarted)
            {
                SkaiaLogger.LogMessage(MessageType.Error, "Cannot attempt to start ConnectionManager: already started.");
                return;
            }

            CoreSocket = socket;
            IPAddress localAddress = NetUtils.GetLocalEndpoint();
            IPEndPoint localEndPoint = new IPEndPoint(localAddress, port);

            CoreSocket.BindSocket(localEndPoint);
            NetUtils.SetConnReset(CoreSocket.Socket);

            SocketThread = new Thread(CoreSocket.Loop)
            {
                Name = "Skaia Socket Thread",
                IsBackground = true
            };
            SocketThread.Start();
            IsStarted = true;
        }

        public static void Stop()
        {
            if (!IsStarted)
            {
                SkaiaLogger.LogMessage(MessageType.Error, "Cannot stop NetworkManager: not started.", new InvalidOperationException("Cannot stop server when it isn't started."));
                return;
            }

            SkaiaLogger.LogMessage(MessageType.Info, "Stopping NetworkManager...");
            // TODO: Tell all clients the server is closing.
            CoreSocket.Socket.Close();
            Nodes = new List<Node>();
            SocketThread.Abort();

            SkaiaLogger.LogMessage(MessageType.Info, "NetworkManager succesfully stopped.");
            IsStarted = false;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}