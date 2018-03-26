
// -----------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Base socket implementation.
// -----------------------------------------

using System;
using System.Net;
using System.Net.Sockets;

namespace SkaiaLib.Base
{
    public abstract class BaseSocket
    {
        protected abstract Socket Socket { get; set; }
        protected abstract EndPoint LocalEndpoint { get; set; }

        public abstract void BindSocket(EndPoint localEndpoint);
        public abstract bool Poll(int timeout);
        public abstract int Receive(byte[] buffer, int length, out EndPoint sender);
        public abstract int Send(byte[] buffer, int length, EndPoint sender);

        /// <summary>
        /// Thanks to the network freelancer, according to him everything fails without this.
        /// </summary>
        /// <param name="s"></param>
        protected void SetConnReset(Socket s)
        {
            try
            {
                const uint IOC_IN = 0x80000000;
                const uint IOC_VENDOR = 0x18000000;
                uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                s.IOControl((int)SIO_UDP_CONNRESET, new Byte[] { Convert.ToByte(false) }, null);
            }
            catch { }
        }
    }
}
