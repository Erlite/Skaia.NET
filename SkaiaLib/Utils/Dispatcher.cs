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
            Action<T> cback;
            if (callbacks.TryGetValue(evnt.GetType(), out cback))
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
