
// -----------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Message type for logging.
// -----------------------------------------

namespace Skaia.Logging
{
    [System.Flags]
    public enum MessageType
    {
        Info,
        Debug,
        Critical,
        Error,
        Fatal
    }
}