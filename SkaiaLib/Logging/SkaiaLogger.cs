
// -----------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Logger for Skaia.
// -----------------------------------------

using System;

namespace Skaia.Logging
{
    /// <summary>
    /// Hook onto the <seealso cref="OnMessageLogged"/> event to receive logs from Skaia.
    /// </summary>
    public static class SkaiaLogger
    {
        /// <summary>
        /// Raised when a message to log has been received.
        /// </summary>
        public static Action<LogMessage> OnMessageLogged;

        /// <summary>
        /// Log a message, fires the <seealso cref="OnMessageLogged"/> event.
        /// </summary>
        /// <param name="message"> The LogMessage to log. </param>
        public static void LogMessage(LogMessage message)
        {
            OnMessageLogged(message);
        }

        /// <summary>
        /// Log a message, fires the <seealso cref="OnMessageLogged"/> event
        /// </summary>
        /// <param name="type"> Type of the message to log. </param>
        /// <param name="message"> The message to log. </param>
        /// <param name="ex"> The exception linked to the message if any. </param>
        public static void LogMessage(MessageType type, string message, Exception ex = null)
        {
            OnMessageLogged(new LogMessage { Type = type, Message = message, Exception = ex });
        }
    }
}