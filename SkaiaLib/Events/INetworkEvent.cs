
// -----------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Base interface for networked events.
// -----------------------------------------

using Skaia.Serialization;
using System.Net;

namespace Skaia.Events
{
    /// <summary>
    /// Interface for network events that must be sent accross the network.
    /// </summary>
    public interface INetworkEvent : INetworkSerializable
    {
        /// <summary>
        /// The endpoint from which this event was sent.
        /// </summary>
        // TODO: Implement simple verification to avoid tempering. Make it optional ?
        EndPoint Sender { get; set; }

        /// <summary>
        /// The ID of this NetEvent.
        /// </summary>
        // HACK: Might be possible to take a smaller data type than int.
        int Id { get; set; }

        /// <summary>
        /// The entity to which this is destined. Optional.
        /// </summary>
        int Target { get; set; }

        /// <summary>
        /// Handle this event after deserializing the packet.
        /// </summary>
        void HandleEvent();        
    }
}
