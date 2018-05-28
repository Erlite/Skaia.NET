
// ----------------------------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Handle reading/writing classes from/into compact byte arrays.
// ----------------------------------------------------------------------

using Skaia.Core;
using System;
using System.IO;

namespace Skaia.Serialization
{
    /// <summary>
    /// Read and write byte arrays into/to classes.
    /// </summary>
    public class PacketBuilder : IDisposable
    {
        private BinaryReader _reader = null;
        private BinaryWriter _writer = null;
        private BuilderMode _mode;
        private byte[] _buffer = null;

        #region Ctor and IDisposable
        /// <summary>
        /// Initialize a new PacketBuilder instance.
        /// </summary>
        public PacketBuilder(BuilderMode mode)
        {
            _mode = mode;

            if (_mode == BuilderMode.Read)
            {
                _reader = new BinaryReader(new MemoryStream());
            }
            else
            {
                _writer = new BinaryWriter(new MemoryStream());
            }
        }

        /// <summary>
        /// Releases all the resources used by the <see cref="PacketBuilder"/>
        /// </summary>
        public void Dispose()
        {
            if (_mode == BuilderMode.Read)
            {
                _reader.Dispose();
            }
            else
            {
                _writer.Dispose();
            }
        }
        #endregion Ctor and IDisposable

        #region Other Methods
        private void AssertMode(BuilderMode mode)
        {
            if (_mode != mode)
            {
                throw new InvalidOperationException($"PacketBuilder is currently set to {_mode.ToString()} mode and therefore cannot use {mode.ToString()} methods.");
            }
        }

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
        /// Sets the buffer from which the <see cref="PacketBuilder"/> will read data.
        /// </summary>
        /// <param name="buffer"> The buffer from which to read data. </param>
        public void FromByteArray(byte[] buffer)
        {
            AssertMode(BuilderMode.Read);
            if (_buffer != null)
            {
                throw new InvalidOperationException("The PacketBuilder's buffer is already set.");
            }

            _buffer = buffer;
            _reader.Read(_buffer, 0, _buffer.Length);
        }

        /// <summary>
        /// Read a type from the buffer. Type must be registered in <see cref="TypeManager"/>
        /// </summary>
        /// <returns> The corresponding type. </returns>
        public Type ReadType()
        {
            AssertMode(BuilderMode.Read);
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
            AssertMode(BuilderMode.Read);
            return _reader.ReadByte();
        }

        /// <summary>
        /// Read a signed byte off the buffer.
        /// </summary>
        /// <returns> The signed byte read. </returns>
        public sbyte ReadSByte()
        {
            AssertMode(BuilderMode.Read);
            return _reader.ReadSByte();
        }

        /// <summary>
        /// Read a short off the buffer.
        /// </summary>
        /// <returns> The short read. </returns>
        public short ReadShort(short maxValue = short.MaxValue)
        {
            AssertMode(BuilderMode.Read);
            return _reader.ReadInt16();
        }

        /// <summary>
        /// Read an unsigned short off the buffer.
        /// </summary>
        /// <returns> The unsigned short read. </returns>
        public ushort ReadUShort()
        {
            AssertMode(BuilderMode.Read);
            return _reader.ReadUInt16();
        }

        /// <summary>
        /// Read an int off the buffer.
        /// </summary>
        /// <returns> The int read. </returns>
        public int ReadInt()
        {
            AssertMode(BuilderMode.Read);
            return _reader.ReadInt32();
        }

        /// <summary>
        /// Read an unsigned int off the buffer.
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt()
        {
            AssertMode(BuilderMode.Read);
            return _reader.ReadUInt32();
        }
        #endregion Read Methods

    }
}
