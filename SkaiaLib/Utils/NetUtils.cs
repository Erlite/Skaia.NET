
// ------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Regroup networking utilities.
// ------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace SkaiaLib.Utils
{
    public static class NetUtils
    {
        public static IPEndPoint GetLocalEndpoint()
        {
            List<NetworkInterface> netInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                                                    .Where(n => n.Supports(NetworkInterfaceComponent.IPv4) && n.OperationalStatus == OperationalStatus.Up)
                                                    .ToList();

            if (netInterfaces.Count == 0)
            {
                // Well. Umm how can I say it. You ain't playing multiplayer at this point.
                // TODO: Default to 127.0.0.1
                // TODO: Logger.
                throw new System.NotImplementedException("No net interfaces.");
            }

            return null;        }
    }
}