
// --------------------------------------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Handle receiving/sending packets and keeping virtual connections alive.
// --------------------------------------------------------------------------------

using Skaia.Sockets;
using Skaia.Surrogates;
using Skaia.Utils;
using System;
using System.Threading.Tasks;

namespace Skaia.Core
{
    /// <summary>
    /// Core of Skaia.NET, handles package routing, virtual connections and anything that involves handling server/clients.
    /// </summary>
    public static class NetworkManager
    {
        private static NetSocket coreSocket;
        private static Client localMachine;
        private static NetworkMode netMode;

        internal static NetworkSettings Settings;

        #region Public API
        /// <summary>
        /// List of clients on the current network.
        /// </summary>
        public static ClientList Clients { get; private set; }

        /// <summary>
        /// The dispatcher that handles routing network events into their respective callbacks.
        /// </summary>
        // TODO: Remove this.
        //public static Dispatcher<NetworkEvent> Dispatcher { get; private set; } = new Dispatcher<NetworkEvent>();

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
        /// <param name="settings"> The settings used by the network manager. </param>
        /// <param name="mode"> The network mode to use. </param>
        public static void StartNetwork(NetSocket socket, NetworkSettings settings, NetworkMode mode)
        {
            if (IsStarted)
            {
                throw new InvalidOperationException("Cannot start NetworkManager: already started.");
            }

            coreSocket = socket;
            var address = NetUtils.GetLocalEndpoint();

            var local = SEndPoint.Create(address.GetAddressBytes(), settings.Port);
            coreSocket.BindSocket(local);
            NetUtils.SetConnReset(coreSocket.Socket);

            // Run the receiving loop on a new Task.
            Task.Run(() => coreSocket.ReceiveLoop());
            // TODO: Implement differed sending loop through Tick.
            
            netMode = mode;
            Settings = settings;
            IsStarted = true;
        }
        #endregion Public Methods
    }
}