using Skaia.Events;
using System;
using System.Collections.Generic;

namespace Skaia.Core
{
    public sealed class PacketHandler<T> where T : INetworkEvent
    {
        private Dictionary<byte, Type> _packetTypes = new Dictionary<byte, Type>();

        /// <summary>
        /// Tries to register a packet type and generates a compiled lambda for instantiation.
        /// </summary>
        /// <param name="type"> The type of the packet to register. </param>
        /// <returns> True on success. </returns>
        // TODO: Add logging with exceptions.
        public bool TryRegisterType(Type type)
        {
            if (_packetTypes.ContainsValue(type))
                return false;

            byte id = (byte)_packetTypes.Count;

            if (id == 255)
                return false;

            _packetTypes.Add(id, type);
            return true;
        }

        /// <summary>
        /// Try to retrieve a type for the specified id.
        /// </summary>
        /// <param name="id"> The id of the type to retrieve. </param>
        /// <param name="type"> The retrieved type if any. </param>
        /// <returns> True if the specified id exists. </returns>
        public bool TryGetType(byte id, out Type type)
        {
            type = null;
            if (_packetTypes.ContainsKey(id))
            {
                type = _packetTypes[id];
                return true;
            }
            return false;
        }

        internal void InstantiateNetworkEvent(Type type, byte[] data)
        {
            if (type.IsAssignableFrom(typeof(INetworkEvent)))
            {
                INetworkEvent netEvent = (INetworkEvent)Activator.CreateInstance(type);
                netEvent.Deserialize(data);
                netEvent.HandleEvent();
            }
            else
            {
                // TODO: Log error.
                throw new NotImplementedException("PacketHandler#InstantiateNetworkEvent() when type isn't INetworkEvent.");
            }
        }
    }
}