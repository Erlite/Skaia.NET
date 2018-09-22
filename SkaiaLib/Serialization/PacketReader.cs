
// --------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Handle reading packets from byte arrays.
// --------------------------------------------------

using Skaia.Core;
using System;
using System.IO;

namespace Skaia.Serialization
{
    /// <summary>
    /// Reads data of a packet.
    /// </summary>
    public class PacketReader : IDisposable
    {
        private BinaryReader _reader = null;

        #region Ctor and IDisposable
        /// <summary>
        /// Initialize a new PacketBuilder instance.
        /// </summary>
        public PacketReader(byte[] data)
        {
            _reader = new BinaryReader(new MemoryStream());
            _reader.Read(data, 0, data.Length);
        }

        /// <summary>
        /// Releases all the resources used by the <see cref="PacketReader"/>
        /// </summary>
        public void Dispose()
        {
            _reader.Dispose();
        }
        #endregion Ctor and IDisposable

        #region Other Methods

        private uint GetBytesRequired(uint n)
        {
            for (int i = 31; i >= 0; --i)
            {
                uint b = 1U << i;

                if ((n & b) == b)
                {
                    uint result = (uint)i + 1;

                    return (result + 7) >> 3;
                }
            }

            return 0;
        }
        #endregion Other Methods

        #region Read Methods

        /// <summary>
        /// Read a type from the buffer. Type must be registered in <see cref="TypeManager"/>
        /// </summary>
        /// <returns> The corresponding type. </returns>
        public Type ReadType()
        {
            byte[] identifier = _reader.ReadBytes(2);
            ushort id = BitConverter.ToUInt16(identifier, 0);
            Type type;
            if (TypeManager.TryGetTypeFromID(id, out type))
            {
                return type;
            }
            else
            {
                throw new InvalidOperationException($"No type with ID {id} has been registered in the TypeManager.");
            }
        }

        /// <summary>
        /// Read a byte off the buffer.
        /// </summary>
        /// <returns> The byte read. </returns>
        public byte ReadByte()
        {
            return _reader.ReadByte();
        }

        /// <summary>
        /// Read a signed byte off the buffer.
        /// </summary>
        /// <returns> The signed byte read. </returns>
        public sbyte ReadSByte()
        {
            return _reader.ReadSByte();
        }

        /// <summary>
        /// Read a short off the buffer.
        /// </summary>
        /// <returns> The short read. </returns>
        public short ReadShort(short maxValue = short.MaxValue)
        {
            return _reader.ReadInt16();
        }

        /// <summary>
        /// Read an unsigned short off the buffer.
        /// </summary>
        /// <returns> The unsigned short read. </returns>
        public ushort ReadUShort()
        {
            return _reader.ReadUInt16();
        }

        /// <summary>
        /// Read an int off the buffer.
        /// </summary>
        /// <returns> The int read. </returns>
        public int ReadInt()
        {
            return _reader.ReadInt32();
        }

        /// <summary>
        /// Read an unsigned int off the buffer.
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt()
        {
            return _reader.ReadUInt32();
        }
        #endregion Read Methods

    }
}
