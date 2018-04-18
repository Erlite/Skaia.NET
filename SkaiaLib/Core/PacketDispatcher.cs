
// ---------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Dispatch received packets using callbacks.
// ---------------------------------------------------

using SkaiaLib.Logging;
using System;
using System.Net;
using System.Collections.Generic;

namespace SkaiaLib.Core
{
    // TODO: Make this a normal class and have an instance of this inside NetworkManager.
    public class Dispatcher<T>
    {
        static Dictionary<Type, Action<EndPoint, object>> callbacks = new Dictionary<Type, Action<EndPoint, object>>();

        /// <summary>
        /// Add a callback for a specific type.
        /// </summary>
        /// <typeparam name="C"> Type to which the callback is linked. </typeparam>
        /// <param name="callback"> Callback to invoke when the type is dispatched. </param>
        public static void AddCallback<C>(Action<EndPoint, object> callback) where C : T
        {
            callbacks.Add(typeof(C), (ep, obj) => callback(ep, obj));
        }

        /// <summary>
        /// Invoke a callback for a specific type.
        /// </summary>
        /// <param name="evnt"> The specific type to invoke. </param>
        public static void CallEvent(Type evnt, EndPoint caller, object data)
        {
            if (callbacks.TryGetValue(evnt, out Action<EndPoint, object> cback))
            {
                cback(caller, data);
            }
            else
            {
                SkaiaLogger.LogMessage(MessageType.Error, $"Couldn't find any callback of type {evnt} to Dispatch");
            }
        }
    }
}