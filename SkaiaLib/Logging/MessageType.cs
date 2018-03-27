
// -----------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Message type for logging.
// -----------------------------------------

namespace SkaiaLib.Logging
{
    [System.Flags]
    public enum MessageType
    {
        Info = 1 << 1,
        Debug = 1 << 2,
        Error = 1 << 3,
        Critical = 1 << 4,
        Fatal = 1 << 5
    }
}