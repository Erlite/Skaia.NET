
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
    [Serializable, StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct Node
    {
        [FieldOffset(0)]
        public byte NetId;
        [FieldOffset(1)]
        public SEndPoint EndPoint;
    }
}
