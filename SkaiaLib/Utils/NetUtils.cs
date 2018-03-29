
// ------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Regroup networking utilities.
// ------------------------------------------

using SkaiaLib.Logging;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SkaiaLib.Utils
{
    public static class NetUtils
    {
        /// <summary>
        /// Tries to find a network interface with a valid IPv4 address and returns it.
        /// </summary>
        /// <returns> The local machine's IPv4 address if any, 127.0.0.1 on failure. </returns>
        public static IPAddress GetLocalEndpoint()
        {
            List<NetworkInterface> netInterfaces = new List<NetworkInterface>();
            try
            {
                netInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                                .Where(n => n.Supports(NetworkInterfaceComponent.IPv4) && n.OperationalStatus == OperationalStatus.Up)
                                .ToList();
            }
            catch (NetworkInformationException netEx)
            {
                SkaiaLogger.LogMessage(MessageType.Critical, "Cannot get information about the network interfaces on this machine.", netEx);
                return null;
            }

            if (netInterfaces.Count == 0)
            {
                // Well. Umm how can I say it. You ain't playing multiplayer at this point.
                SkaiaLogger.LogMessage(MessageType.Critical, "Cannot find any operating network interface with IPv4 support, defaulting to 127.0.0.1");

                // TODO: Check if we should just fail at this point, dunno if 127.0.0.1 will even work.
                return IPAddress.Parse("127.0.0.1");
            }

            IPAddress address =  netInterfaces.Select(n => n.GetIPProperties())
                                .SelectMany(i => i.UnicastAddresses)
                                .Where(u => u.IsDnsEligible) // Just a safety check I suppose.
                                .Select(u => u.Address)
                                .FirstOrDefault();

            // Shouldn't ever arrive but you're never sure enough.
            if (address == null)
                return IPAddress.Parse("127.0.0.1");

            return address;
        }

        /// <summary>
        /// Sets the connection reset on a socket.
        /// </summary>
        /// <param name="s"></param>
        public static void SetConnReset(Socket s)
        {
            try
            {
                const uint IOC_IN = 0x80000000;
                const uint IOC_VENDOR = 0x18000000;
                uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                s.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
            }
            catch { }
        }
    }
}