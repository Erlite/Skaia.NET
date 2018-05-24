
// -------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Provide an enumerator for ClientList.
// -------------------------------------------------

using System.Collections;

namespace Skaia.Core
{
    public class ClientEnumerator : IEnumerator
    {
        public Client[] _clients;
        int pos = -1;

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public Client Current
        {
            get
            {
                return _clients[pos];
            }
        }

        public bool MoveNext()
        {
            pos++;
            return pos < _clients.Length;
        }

        public void Reset()
        {
            pos = -1;
        }

        public ClientEnumerator(Client[] connections)
        {
            _clients = connections;
        }
    }
}
