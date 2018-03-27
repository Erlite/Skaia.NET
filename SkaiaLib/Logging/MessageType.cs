
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
        Info,
        Debug,
        Critical,
        Error,
        Fatal
    }
}