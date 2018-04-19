
// ---------------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Regroup information about a node on the network.
// ---------------------------------------------------------

using Skaia.Surrogates;
using System;
using System.Runtime.InteropServices;

namespace Skaia.Core
{
    /// <summary>
    /// Data container for all the information about a node on the network (server/client).
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct Node
    {
        /// <summary>
        /// The network ID of this node.
        /// The server node always has an ID of 0.
        /// </summary>
        [FieldOffset(0)]
        public byte NetId;
        /// <summary>
        /// The endpoint linked to this node. Null on clients for obvious reasons.
        /// </summary>
        [FieldOffset(1)]
        public SEndPoint EndPoint;
    }
}
