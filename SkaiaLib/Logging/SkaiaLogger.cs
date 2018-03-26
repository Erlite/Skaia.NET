
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
        /// The level of messages to log.
        /// </summary>
        public static LogLevel LogLevel { get; set; } = LogLevel.Info;

        /// <summary>
        /// Raised when a message to log has been received.
        /// </summary>
        public static Action<LogMessage> OnMessageLogged;

        /// <summary>
        /// Set the logging level to use.
        /// </summary>
        /// <param name="level"></param>
        public static void SetLevel(LogLevel level)
        {
            LogLevel = level;
            LogMessage msg = new LogMessage
            {
                Severity = MsgSeverity.Log,
                Message = $"Set logging level to {level.ToString()}"
            };
        }

        /// <summary>
        /// Log a message, fires the OnMessageLogged event.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public static void Log(LogLevel level, LogMessage message)
        {
            if ((LogLevel & level) == level)
                OnMessageLogged(message);
        }
    }
}