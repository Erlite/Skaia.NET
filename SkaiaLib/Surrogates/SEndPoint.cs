
// --------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Provide a lightweight IP/Port pair.
// --------------------------------------------

using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Skaia.Surrogates
{
    /// <summary>
    /// Lighter surrogate for <seealso cref="IPEndPoint"/>
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct SEndPoint : IEquatable<SEndPoint>
    {
        [FieldOffset(0)]
        private byte[] IPv4;
        [FieldOffset(4)]
        private int Port;

        /// <summary>
        /// Convert a SEndPoint into an IPEndPoint.
        /// </summary>
        /// <param name="value"> The SEndPoint to convert. </param>
        public static implicit operator IPEndPoint(SEndPoint value)
        {
            return new IPEndPoint(new IPAddress(value.IPv4), value.Port);
        }

        /// <summary>
        /// Convert an IPEndPoint to a SEndPoint.
        /// </summary>
        /// <param name="value"> The IPEndPoint to convert. </param>
        public static implicit operator SEndPoint(IPEndPoint value)
        {
            return new SEndPoint
            {
                IPv4 = value.Address.GetAddressBytes(),
                Port = value.Port
            };
        }

        /// <summary>
        /// Create a new instance of SEndPoint using a byte array for the address.
        /// </summary>
        /// <param name="address"> The endpoint's IPv4 address. </param>
        /// <param name="port"> The endpoint's port. </param>
        /// <returns> The corresponding SEndPoint. </returns>
        public static SEndPoint Create(byte[] address, int port)
        {
            if (address.Length != 4)
            {
                throw new FormatException("An address must consist of 4 bytes.");
            }

            return new SEndPoint
            {
                IPv4 = address,
                Port = port
            };
        }

        /// <summary>
        /// Create a new instance of SEndPoint using an IPAddress for the address.
        /// </summary>
        /// <param name="address"> The endpoint's IPv4 address. </param>
        /// <param name="port"> The endpoint's port. </param>
        /// <returns> The corresponding SEndPoint. </returns>
        public static SEndPoint Create(IPAddress address, int port)
        {
            if (address.AddressFamily != AddressFamily.InterNetwork)
            {
                throw new FormatException("The AddressFamily of IPAddress must be AddressFamily.InterNetwork (IPv4)");
            }

            return new SEndPoint
            {
                IPv4 = address.GetAddressBytes(),
                Port = port
            };
        }

        /// <summary>
        /// Create a new instance of SEndPoint using a string for the address.
        /// </summary>
        /// <param name="address"> The endpoint's IPv4 address. </param>
        /// <param name="port"> The endpoint's port. </param>
        /// <returns> The corresponding SEndPoint. </returns>
        public static SEndPoint Create(string address, int port)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new FormatException("Address cannot be null or empty.");
            }

            if (IPAddress.TryParse(address, out IPAddress adr))
            {
                if (adr.AddressFamily != AddressFamily.InterNetwork)
                {
                    throw new FormatException("The AddressFamily of IPAddress must be AddressFamily.InterNetwork (IPv4)");
                }

                return new SEndPoint
                {
                    IPv4 = IPAddress.Parse(address).GetAddressBytes(),
                    Port = port
                };
            }
            else
            {
                throw new FormatException("Parameter address is an invalid IP.");
            }
        }

        /// <summary>
        /// Check if this is equal to another SEndPoint.
        /// </summary>
        /// <param name="other"> The other SEndPoint to check. </param>
        /// <returns> True if equal. </returns>
        public bool Equals(SEndPoint other)
        {
            IPAddress address = new IPAddress(IPv4);
            IPAddress otherAddress = new IPAddress(other.IPv4);

            return address.ToString() == otherAddress.ToString() && Port == other.Port;
        }
    }
}