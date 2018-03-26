
// ---------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Dispatch callbacks based on data received.
// ---------------------------------------------------

using System;
using System.Collections.Generic;

namespace SkaiaLib.Utils
{
    public class Dispatcher<T>
    {
        Dictionary<Type, Action<T>> callbacks = new Dictionary<Type, Action<T>>();

        public void AddCallback<C>(Action<C> callback) where C : T
        {
            callbacks.Add(typeof(C), func => callback((C)func));
        }

        public void Call(T evnt)
        {
            if (callbacks.TryGetValue(evnt.GetType(), out Action<T> cback))
            {
                cback(evnt);
            }
            else
            {
                // TODO: Implement logger.
                Console.WriteLine("SkaiaLib [ERROR] => Couldn't find any callback of type " + evnt.GetType());
            }
        }
    }
}
