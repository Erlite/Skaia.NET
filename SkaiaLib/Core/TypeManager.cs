
// ----------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Register types for serialization
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace Skaia.Core
{
    /// <summary>
    /// Manages types to be used with serialization by <see cref="Serialization.PacketReader"/>
    /// </summary>
    [Obsolete("Obsolete, this will be ported to PacketHandler")]
    public static class TypeManager
    {
        private static ushort _id = 0;
        private static Dictionary<Type, ushort> _typeIDPair = new Dictionary<Type, ushort>();

        /// <summary>
        /// Try to register a type for serialization.
        /// </summary>
        /// <param name="type"> The type to register. </param>
        /// <param name="id"> The returned ID registered to the type on success. </param>
        /// <param name="statusMessage"> The returned message linked to the action. </param>
        /// <returns> True if the type was successfully registered. </returns>
        public static bool TryRegisterType(Type type, out ushort id, out string statusMessage)
        {
            // TODO: Make the amount of bytes used for types be a setting in NetworkSettings.
            if (_typeIDPair.ContainsKey(type))
            {
                id = 0;
                statusMessage = $"Type {type.Name} is already registered with ID {_typeIDPair[type]}.";
                return false;
            }

            if (_id > ushort.MaxValue)
            {
                id = 0;
                statusMessage = $"Cannot register type: limit of {ushort.MaxValue} types was hit.";
                return false;
            }

            _typeIDPair.Add(type, _id);
            id = _id;
            statusMessage = $"Successfully added type {type.Name} with ID {_id}.";
            _id++;

            return true;
        }

        /// <summary>
        /// Try to get the ID linked to a type.
        /// </summary>
        /// <param name="type"> The type to search for. </param>
        /// <param name="id"> The returned ID if the type exists. </param>
        /// <returns> True on success. </returns>
        public static bool TryGetIDFromType(Type type, out ushort id)
        {
            if (_typeIDPair.ContainsKey(type))
            {
                id = _typeIDPair[type];
                return true;
            }

            id = 0;
            return false;
        }

        /// <summary>
        /// Try to get the type linked to an ID.
        /// </summary>
        /// <param name="id"> The ID to search for. </param>
        /// <param name="type"> The returned type if the ID exists. </param>
        /// <returns> True on success. </returns>
        public static bool TryGetTypeFromID(ushort id, out Type type)
        {
            if (_typeIDPair.ContainsValue(id))
            {
                type = _typeIDPair.Keys.FirstOrDefault(v => _typeIDPair[v] == id);
                return true;
            }

            type = null;
            return false;
        }

        /// <summary>
        /// Get the byte array representation of an ID.
        /// </summary>
        /// <param name="id"> The ID to translate. </param>
        /// <returns> The translated ID as a byte array. </returns>
        public static byte[] GetBytesFromID(ushort id)
        {
            return BitConverter.GetBytes(id);
        }
    }
}