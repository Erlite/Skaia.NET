
// ---------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Dispatch objects using callbacks.
// ---------------------------------------------------

using System;
using System.Collections.Generic;

namespace Skaia.Core
{
    /// <summary>
    /// Dispatch a specific type to a callback.
    /// </summary>
    /// <typeparam name="T"> The type of object to dispatch. </typeparam>
    [Obsolete("Refactoring this into the INetworkEvent itself.")]
    public sealed class Dispatcher<T> 
    {
        Dictionary<Type, Action<T>> callbacks = new Dictionary<Type, Action<T>>();

        /// <summary>
        /// Add a callback for a specific type.
        /// </summary>
        /// <typeparam name="C"> Type to which the callback is linked. </typeparam>
        /// <param name="callback"> Callback to invoke when the type is dispatched. </param>
        public void AddCallback<C>(Action<C> callback) where C : T
        { 
            callbacks.Add(typeof(C), (obj) => callback((C)obj));
        }


        /// <summary>
        /// Invoke the callback linked to the specified Type.
        /// </summary>
        /// <param name="type"> The type of the callback. </param>
        /// <param name="data"> The data to send through the callback. </param>
        /// <returns> True if the specified Type has a callback. </returns>
        public bool TryInvokeCallback(Type type, T data)
        {
            Action<T> cback;
            if (callbacks.TryGetValue(type, out cback))
            {
                cback(data);
                return true;
            }
            return false;
        }
    }
}