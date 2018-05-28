
// ----------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Enum to set the PacketBuilder's mode.
// ----------------------------------------------------

namespace Skaia.Serialization
{
    /// <summary>
    /// Sets the PacketBuilder's internal mode to Read or Write.
    /// </summary>
    public enum BuilderMode
    {
        /// <summary>
        /// Read data from a byte array.
        /// </summary>
        Read = 1,

        /// <summary>
        /// Write data into a byte array.
        /// </summary>
        Write = 2
    }
}
