
// ------------------------------------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Provide a compact way to serialize multiple bools into a single byte.
// ------------------------------------------------------------------------------

using System;
using UnityEngine;

namespace Skaia.Serialization
{
    /// <summary>
    /// Wrapper class that can hold eight bools in a single byte.
    /// </summary>
    public class CompressibleBool : ICompressible<byte>
    {
        /// <summary>
        /// The underlying byte which holds the eight booleans.
        /// </summary>
        public byte Byte = 0;

        /// <summary>
        /// Get or set a bool.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool this[int index]
        {
            get
            {
                if (index < 0 || index > 7)
                {
                    throw new IndexOutOfRangeException();
                }

                return (Byte | 8) >> index == 1;
            }

            set
            {
                if (index < 0 || index > 7)
                {
                    throw new IndexOutOfRangeException();
                }

                int i = value ? 1 : 0;
                Byte |= (byte)(i << index);
            }
        }

        [Obsolete("CompressibleBool is always compressed.")]
        public bool CompressionEnabled { get; set; }

        byte ICompressible<byte>.Value { get { return Byte; } set { Byte = value; } }

        public byte[] Compress()
        {
            return new byte[] { Byte };
        }

        public void Decompress(byte[] data)
        {
            Byte = data[0];
        }
    }
}
