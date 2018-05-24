
// ---------------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Regroup information about a node on the network.
// ---------------------------------------------------------

using Skaia.Surrogates;
using System;

namespace Skaia.Core
{
    /// <summary>
    /// Data container for all the information about a node on the network (server/client).
    /// </summary>
    [Serializable]
    public class Client
    {
        /// <summary>
        /// The network ID of this node.
        /// The server node always has an ID of 0.
        /// </summary>
        public byte NetId { get; internal set; }

        /// <summary>
        /// The state of the connection.
        /// </summary>
        public ConnectionState State { get; internal set; }

        /// <summary>
        /// The endpoint linked to this node. Null on clients for obvious reasons.
        /// </summary>
        public SEndPoint EndPoint { get; internal set; }
    }
}
