
// ---------------------------------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Handle receiving/sending packets and keeping virtual connections alive.
// ---------------------------------------------------------------------------

using SkaiaLib.Logging;
using SkaiaLib.Sockets;
using SkaiaLib.Utils;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace SkaiaLib.Core
{
    public static class NetworkManager
    {
        public static Dictionary<byte, IPEndPoint> Connections { get; private set; } = new Dictionary<byte, IPEndPoint>();
        public static BaseSocket CoreSocket { get; private set; }
        public static Thread SocketThread { get; private set; }
        public static bool Started { get; private set; }

        public static void Start(int port) => Start(new UDPSocket(), port);
        public static void Start(BaseSocket socket, int port)
        {
            if (Started)
            {
                SkaiaLogger.LogMessage(MessageType.Error, "Cannot attempt to start ConnectionManager: already started.");
                return;
            }

            CoreSocket = socket;
            IPAddress localAddress = NetUtils.GetLocalEndpoint();
            IPEndPoint localEndPoint = new IPEndPoint(localAddress, port);

            CoreSocket.BindSocket(localEndPoint);
            NetUtils.SetConnReset(CoreSocket.Socket);

            SocketThread = new Thread(CoreSocket.Loop)
            {
                Name = "SkaiaLib Socket Thread",
                IsBackground = true
            };
            SocketThread.Start();
            Started = true;
        }

        public static void Stop()
        {
            SkaiaLogger.LogMessage(MessageType.Info, "Stopping server...");
            // TODO: Tell all clients the server is closing.
            CoreSocket.Socket.Close();
            Connections = new Dictionary<byte, IPEndPoint>();
            Started = false;
            SocketThread.Abort();
            SkaiaLogger.LogMessage(MessageType.Info, "Server succesfully stopped.");
        }
    }
}