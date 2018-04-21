namespace Skaia.Core
{
    /// <summary>
    /// Contains the possible network modes that SkaiaLib currently supports.
    /// </summary>
    public enum NetworkMode
    {
        /// <summary>
        /// A client node which can connect to a server.
        /// </summary>
        Client = 1,
        /// <summary>
        /// A server node which can accept incoming connections.
        /// </summary>
        Server = 2
    }
}