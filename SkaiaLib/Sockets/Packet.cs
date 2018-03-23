using System.Net;

namespace SkaiaLib.Sockets
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