
// ---------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Dispatch objects using callbacks.
// ---------------------------------------------------

using Skaia.Logging;
using System;
using System.Net;
using System.Collections.Generic;

namespace Skaia.Core
{
    /// <summary>
    /// Dispatch a specific type to a callback.
    /// </summary>
    /// <typeparam name="T"> The type of object to dispatch. </typeparam>
    public class Dispatcher<T> 
    {
        Dictionary<Type, Action<EndPoint, T>> callbacks = new Dictionary<Type, Action<EndPoint, T>>();

        /// <summary>
        /// Add a callback for a specific type.
        /// </summary>
        /// <typeparam name="C"> Type to which the callback is linked. </typeparam>
        /// <param name="callback"> Callback to invoke when the type is dispatched. </param>
        public void AddCallback<C>(Action<EndPoint, C> callback) where C : T
        { 
            callbacks.Add(typeof(C), (ep, obj) => callback(ep, (C)obj));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="caller"></param>
        /// <param name="data"></param>
        public void CallEvent(Type evnt, EndPoint caller, T data)
        {
            if (callbacks.TryGetValue(evnt, out Action<EndPoint, T> cback))
            {
                // TODO: Find out the the fook I can cast that to the original type. Maybe on the action's method arguments itself.
                cback(caller, data);
            }
            else
            {
                // TODO: Should throw somehow? Maybe make that an option.
                SkaiaLogger.LogMessage(MessageType.Error, $"Couldn't find any callback of type {evnt} to Dispatch");
            }
        }
    }
}