
// -----------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Log level enumerator for logging.
// -----------------------------------------

namespace SkaiaLib.Logging
{
    [System.Flags]
    public enum LogLevel
    {
        Verbose = 1 << 1,
        Debug = 1 << 2,
        Info = 1 << 3,
    }
}