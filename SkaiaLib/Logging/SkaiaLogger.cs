
// -----------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Logger for SkaiaLib.
// -----------------------------------------

using System;

namespace SkaiaLib.Logging
{
    /// <summary>
    /// Hook onto the <seealso cref="OnMessageLogged"/> event to receive logs from SkaiaLib.
    /// </summary>
    public static class SkaiaLogger
    {
        /// <summary>
        /// Raised when a message to log has been received.
        /// </summary>
        public static Action<LogMessage> OnMessageLogged;

        /// <summary>
        /// Log a message, fires the OnMessageLogged event.
        /// </summary>
        /// <param name="message"> The LogMessage to log. </param>
        public static void Log(LogMessage message)
        {
            OnMessageLogged(message);
        }
    }
}