
// -----------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Base class for networked events.
// -----------------------------------------

using System.Net;

namespace Skaia.Events
{
    /// <summary>
    /// Base class for all network events that must be sent accross the network.
    /// </summary>
    public abstract class NetEvent
    {
        /// <summary>
        /// The endpoint from which this event was sent.
        /// </summary>
        // TODO: Implement simple verification to avoid tempering. Make it optional ?
        public abstract EndPoint Sender { get; internal set; }

        /// <summary>
        /// The ID of this NetEvent.
        /// </summary>
        // HACK: Might be possible to take a smaller data type than int.
        public abstract int ID { get; internal set; }

        /// <summary>
        /// The entity to which this is destined.
        /// -1 if none.
        /// </summary>
        public abstract int Target { get; internal set; }
    }
}
