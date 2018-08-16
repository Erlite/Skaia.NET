
// ----------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: A custom collection for clients.
// ----------------------------------------------

using System.Collections;

namespace Skaia.Core
{
    public class ClientList : IEnumerable
    {
        private Client[] _clients;

        public readonly int MaxClients;
        public int Count { get; private set; }

        public IEnumerator GetEnumerator()
        {
            return new ClientEnumerator(_clients);
        }

        public Client this[int index]
        {
            get
            {
                return _clients[index];
            }
        }

        /// <summary>
        /// Is this server full?
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            for (int i = 0; i < _clients.Length; i++)
            {
                if (_clients[i] == null)
                    return false;
            }
            return true;
        }

        public ClientList(int maxClients)
        {
            _clients = new Client[maxClients];
            MaxClients = maxClients;

            for (int i = 0; i < _clients.Length; i++)
            {
                _clients[i] = null;
            }
        }
    }
}