
// --------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Provide a lightweight IP/Port pair.
// --------------------------------------------

using System.Net;
using System.Runtime.InteropServices;

namespace Skaia.Surrogates
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct SEndPoint
    {
        [FieldOffset(0)]
        private byte[] IPv4;
        [FieldOffset(4)]
        private int Port;

        public static implicit operator IPEndPoint(SEndPoint value)
        {
            return new IPEndPoint(new IPAddress(value.IPv4), value.Port);
        }

        public static implicit operator SEndPoint(IPEndPoint value)
        {
            return new SEndPoint
            {
                IPv4 = value.Address.GetAddressBytes(),
                Port = value.Port
            };
        }
    }
}