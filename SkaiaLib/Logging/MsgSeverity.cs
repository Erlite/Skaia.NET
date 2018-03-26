
// -----------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Message severity for logging.
// -----------------------------------------

namespace SkaiaLib.Logging
{
    [System.Flags]
    public enum MsgSeverity
    {
        Log = 1 << 1,
        Error = 1 << 2,
        Critical = 1 << 3,
        Fatal = 1 << 4
    }
}