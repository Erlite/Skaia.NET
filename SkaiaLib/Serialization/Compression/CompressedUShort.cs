
// ----------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Provide a compressed ushort surrogate.
// ----------------------------------------------------

using Skaia.Utils;
using System;

namespace Skaia.Serialization
{
    /// <summary>
    /// A compressible ushort for serialization.
    /// </summary>
    public class CompressedUShort : ICompressible<ushort>
    {
        public CompressedUShort(ushort minValue, ushort maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        private ushort _value;

        /// <summary>
        /// The minimum value of this ushort.
        /// </summary>
        public ushort MinValue { get; }
        /// <summary>
        /// The maximum value of this ushort.
        /// </summary>
        public ushort MaxValue { get; }
        /// <summary>
        /// Should the value be clamped between Min and Max?
        /// </summary>
        public bool ShouldClamp { get; set; }
        /// <summary>
        /// The actual ushort.
        /// </summary>
        public ushort Value
        {
            get { return _value; }
            set
            {
                if (ShouldClamp)
                    Clamp();
                _value = value;
            }
        }

        #region Public Methods

        /// <summary>
        /// Clamp this value within bounds of Min and Max value.
        /// </summary>
        public void Clamp()
        {
            Value = Math.Min(Value, MaxValue);
            Value = Math.Max(Value, MinValue);
        }

        /// <summary>
        /// Compress this ushort into a compact byte array.
        /// </summary>
        public byte[] Compress()
        {
            // Get the max range of this compressed ushort...
            uint range = (uint)(MaxValue - MinValue);
            // ... and the required bytes to hold it.
            uint required = ((ICompressible<ushort>)this).GetRequiredBytes();

            // Now we grab the actual value to compress.
            // For that we just substract the MinValue from the current value.
            ushort compressed = (ushort)(Value - MinValue);

            // Convert into a byte array...
            byte[] source = BitConverter.GetBytes(compressed);
            byte[] destination = new byte[required];

            // Copy the required bytes into the destination array to get rid of the useless bytes.
            Array.Copy(source, destination, required);
            return destination;
        }

        /// <summary>
        /// Decompress a compressed ushort from a compact byte array.
        /// </summary>
        public void Decompress(byte[] data)
        {
            // Make a byte array as big as the size of a ushort.
            byte[] decompressed = new byte[sizeof(ushort)];
            Array.Copy(data, decompressed, data.Length);
            ushort decompressedushort = BitConverter.ToUInt16(decompressed, 0);

            // Set the value by adding the MinValue to the decompressed ushort.
            Value = (ushort)(MinValue + decompressedushort);
        }
        #endregion Public Methods

        uint ICompressible<ushort>.GetRequiredBytes()
        {
            uint range = (uint)(MaxValue - MinValue);
            return Maths.GetRequiredBytes(range, sizeof(ushort));
        }

        // Get the underlying ushort by using CompressedUShort as a right-hand value.
        public static implicit operator ushort(CompressedUShort cUShort)
        {
            return cUShort.Value;
        }
    }
}