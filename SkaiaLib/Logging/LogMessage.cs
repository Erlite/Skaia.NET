﻿
// -----------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Logging message struct.
// -----------------------------------------

using System;

namespace Skaia.Logging
{
    public struct LogMessage
    {
        public MessageType Type;
        public string Message;
        public Exception Exception;
    }
}