
// ---------------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Packet struct to link data with receiver/sender.
// ---------------------------------------------------------

using System.Net;

namespace Skaia.Sockets
{
    /// <summary>
    /// Regroups the data received from an endpoint/the data to send to an endpoint.
    /// This is not meant to be sent directly to the socket, thus why it isn't serializable.
    /// The socket should rather take the Data to send and set the receiver to EndPoint.
    /// </summary>
    public struct Packet
    {
        public byte[] Data;
        public EndPoint Endpoint;
    }
}