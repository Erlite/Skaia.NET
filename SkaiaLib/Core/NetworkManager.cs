
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
        private static Client localMachine;
        private static NetworkMode netMode;
        private static Thread socketThread;

        #region Public API
        /// <summary>
        /// List of clients on the current network.
        /// </summary>
        public static ClientList Clients { get; private set; }

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
        public static Client LocalClient
        {
            get
            {
                if (!IsStarted)
                {
                    throw new InvalidOperationException("Cannot retrieve the local client when the NetworkManager isn't started.");
                }

                return localMachine;
            }
            private set { localMachine = value; }
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
        public static void StartNetwork(BaseSocket socket, NetworkSettings settings, NetworkMode mode)
        {
            if (IsStarted)
            {
                throw new InvalidOperationException("Cannot start NetworkManager: already started.");
            }

            coreSocket = socket;
            IPAddress address = NetUtils.GetLocalEndpoint();

            SEndPoint local = SEndPoint.Create(address.GetAddressBytes(), settings.Port);
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