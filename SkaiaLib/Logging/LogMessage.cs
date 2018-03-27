﻿
// -----------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Logging message struct.
// -----------------------------------------

using System;

namespace SkaiaLib.Logging
{
    public struct LogMessage
    {
        public MessageType Type;
        public string Message;
        public Exception Exception;
    }
}