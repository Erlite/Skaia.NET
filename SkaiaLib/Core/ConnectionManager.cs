
// ----------------------------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Handle keeping endpoints alive as pseudo-connections for UDP.
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Net;

namespace SkaiaLib.Core
{
    public static class ConnectionManager
    {
        public static Dictionary<byte, IPEndPoint> Connections { get; } = new Dictionary<byte, IPEndPoint>();


    }
}