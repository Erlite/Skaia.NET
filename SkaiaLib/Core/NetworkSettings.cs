
// ---------------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Provide a groupment of network settings to devs.
// ---------------------------------------------------------

namespace Skaia.Core
{
    public class NetworkSettings
    {
        /// <summary>
        /// The max amount of players the server will accept.
        /// Default: 1
        /// </summary>
        public int MaxPlayers = 1;

        /// <summary>
        /// The port the server will use.
        /// For clients, it's recommended to set the port to 0, as the socket will automatically determine an available port.
        /// Default: 27015
        /// </summary>
        public int Port = 27015;
    }
}