
// ---------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Dispatch received packets using callbacks.
// ---------------------------------------------------

using SkaiaLib.Logging;
using SkaiaLib.Sockets;
using SkaiaLib.Utils;
using System;
using System.Net;
using System.Collections.Generic;
using System.Threading;

namespace SkaiaLib.Core
{
    // TODO: Make this a normal class and have an instance of this inside NetworkManager.
    public static class PacketDispatcher<T>
    {
        public static Thread DispatcherThread;
        public static bool Started { get; private set; }

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

        /// <summary>
        /// Start the packet dispatcher.
        /// </summary>
        public static void Start()
        {
            if (Started)
            {
                SkaiaLogger.LogMessage(MessageType.Error, "Cannot start PacketDispatcher: already started.");
                return;
            }

            if (!NetworkManager.Started)
            {
                SkaiaLogger.LogMessage(MessageType.Error, "Cannot start PacketDispatcher: ConnectionManager wasn't started.");
            }

            DispatcherThread = new Thread(DispatcherLoop)
            {
                Name = "SkaiaLib Packet Dispatcher Thread",
                IsBackground = true
            };
            DispatcherThread.Start();
            Started = true;
        }

        /// <summary>
        /// Start the dispatcher loop.
        /// </summary>
        private static void DispatcherLoop()
        {
            while (true)
            {
                while (NetworkManager.CoreSocket.DequeueReceivedPacketQueue(out Packet packet))
                {
                    object data = NetUtils.ByteArrayToObject(packet.Data);
                    Type evntType = data.GetType();
                    CallEvent(evntType, packet.Endpoint, data);
                }
                Thread.Sleep(1);
            }
        }
    }
}