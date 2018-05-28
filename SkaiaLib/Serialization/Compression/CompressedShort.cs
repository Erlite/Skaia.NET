
// ----------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Provide an abstract compression class.
// ----------------------------------------------------

using Skaia.Utils;
using System;

namespace Skaia.Serialization
{
    /// <summary>
    /// A compressable short for packet serialization.
    /// </summary>
    public class CompressedShort : ICompressible<short>
    {
        private short _value;

        /// <summary>
        /// The minimum value of this short.
        /// </summary>
        public short MinValue { get; }
        /// <summary>
        /// The maximum value of this short.
        /// </summary>
        public short MaxValue { get; }
        /// <summary>
        /// Should the value be clamped between Min and Max?
        /// </summary>
        public bool ShouldClamp { get; set; }
        /// <summary>
        /// The actual short.
        /// </summary>
        public short Value
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
        /// Compress this short into a compact byte array.
        /// </summary>
        public byte[] Compress()
        {
            // Get the max range of this compressed short...
            uint range = (uint)(MaxValue - MinValue);
            // ... and the required bytes to hold it.
            uint required = ((ICompressible<short>)this).GetRequiredBytes();

            // Now we grab the actual value to compress.
            // For that we just substract the MinValue from the current value.
            short compressed = (short)(Value - MinValue);

            // Convert into a byte array...
            byte[] source = BitConverter.GetBytes(compressed);
            byte[] destination = new byte[required];

            // Copy the required bytes into the destination array to get rid of the useless bytes.
            Array.Copy(source, destination, required);
            return destination;
        }

        /// <summary>
        /// Decompress a compressed short from a compact byte array.
        /// </summary>
        public void Decompress(byte[] data)
        {
            // Make a byte array as big as the size of a short.
            byte[] decompressed = new byte[sizeof(short)];
            Array.Copy(data, decompressed, data.Length);
            short decompressedShort = BitConverter.ToInt16(decompressed, 0);

            // Set the value by adding the MinValue to the decompressed short.
            Value = (short)(MinValue + decompressedShort);
        }
        #endregion Public Methods

        uint ICompressible<short>.GetRequiredBytes()
        {
            uint range = (uint)(MaxValue - MinValue);
            return Maths.GetRequiredBytes(range, sizeof(short));
        }

        // Get the underlying short by using CompressedShort as a right-hand value.
        public static implicit operator short(CompressedShort cShort)
        {
            return cShort.Value;
        }
    }
}