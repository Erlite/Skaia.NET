
// ---------------------------------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Handle dispatching packets and keeping virtual connections alive.
// ---------------------------------------------------------------------------

using SkaiaLib.Sockets;
using SkaiaLib.Utils;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace SkaiaLib.Core
{
    public static class ConnectionManager
    {
        public static Dictionary<byte, IPEndPoint> Connections { get; } = new Dictionary<byte, IPEndPoint>();
        public static BaseSocket CoreSocket { get; private set; }
        public static Thread SocketThread { get; private set; }

        public static void Start(int port) => Start(new UDPSocket(), port);
        public static void Start(BaseSocket socket, int port)
        {
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
        }

    }
}