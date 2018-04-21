
// ---------------------------------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Handle receiving/sending packets and keeping virtual connections alive.
// ---------------------------------------------------------------------------

using Skaia.Events;
using Skaia.Sockets;
using Skaia.Surrogates;
using Skaia.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace Skaia.Core
{
    /// <summary>
    /// Core of Skaia.NET, handles package routing, virtual connections and anything that involves handling server/clients.
    /// </summary>
    public static class NetworkManager
    {
        private static BaseSocket coreSocket;
        private static Node localNode;
        private static NetworkMode netMode;
        private static Thread socketThread;

        #region Public API
        /// <summary>
        /// List of nodes on the current network.
        /// </summary>
        // TODO: Make a custom container for the Node list to allow checking for stupid things like two nodes with the same ID.
        public static List<Node> Nodes { get; private set; } = new List<Node>();

        /// <summary>
        /// The dispatcher that handles routing network events into their respective callbacks.
        /// </summary>
        public static Dispatcher<NetEvent> Dispatcher { get; private set; } = new Dispatcher<NetEvent>();

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

        /// <summary>
        /// Returns true if the local machine is in Server mode.
        /// </summary>
        public static bool IsServer
        {
            get
            {
                if (!IsStarted)
                    throw new InvalidOperationException("Cannot get IsServer when the NetworkManager isn't started.");
                return netMode == NetworkMode.Server;
            }
        }

        /// <summary>
        /// Returns true if the local machine is in Client mode.
        /// </summary>
        public static bool IsClient
        {
            get
            {
                if (!IsStarted)
                    throw new InvalidOperationException("Cannot get IsClient when the NetworkManager isn't started.");
                return netMode == NetworkMode.Client;
            }
        }
        #endregion Public API

        #region Public Methods
        /// <summary>
        /// Starts the NetworkManager.
        /// </summary>
        /// <param name="socket"> The socket to use. </param>
        /// <param name="port"> The port to bind to. Will default to a random open port for clients. </param>
        /// <param name="mode"> The network mode to use. </param>
        public static void StartNetwork(BaseSocket socket, int port, NetworkMode mode)
        {
            if (IsStarted)
            {
                throw new InvalidOperationException("Cannot start NetworkManager: already started.");
            }

            coreSocket = socket;
            IPAddress address = NetUtils.GetLocalEndpoint();
            port = mode == NetworkMode.Server ? port : 0;

            SEndPoint local = SEndPoint.Create(address.GetAddressBytes(), port);
            coreSocket.BindSocket(local);
            NetUtils.SetConnReset(coreSocket.Socket);

            socketThread = new Thread(coreSocket.Loop)
            {
                Name = "Skaia.NET NetworkManager",
                IsBackground = true
            };

            socketThread.Start();
            netMode = mode;
            IsStarted = true;
        }
        #endregion Public Methods
    }
}