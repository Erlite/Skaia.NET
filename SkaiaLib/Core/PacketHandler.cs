using Skaia.Events;
using System;
using System.Collections.Generic;

namespace Skaia.Core
{
    public sealed class PacketHandler<T> where T : INetworkEvent
    {
        private Dictionary<Type, byte> _packetTypes = new Dictionary<Type, byte>();

        /// <summary>
        /// Tries to register a packet type and generates a compiled lambda for instantiation.
        /// </summary>
        /// <param name="type"> The type of the packet to register. </param>
        /// <returns> True on success. </returns>
        // TODO: Add logging with exceptions.
        public bool TryRegisterType(Type type)
        {
            if (_packetTypes.ContainsKey(type))
                return false;

            byte id = (byte)_packetTypes.Count;

            if (id == 255)
                return false;

            _packetTypes.Add(type, id);
            return true;
        }

        internal void InstantiateNetworkEvent<E>(byte[] data) where E : INetworkEvent
        {
            INetworkEvent netEvent = Activator.CreateInstance<E>();
            netEvent.Deserialize(data);
            netEvent.HandleEvent();
        }
    }
}