
// ----------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Provide a compressed short surrogate.
// ----------------------------------------------------

using Skaia.Utils;
using System;

namespace Skaia.Serialization
{
    /// <summary>
    /// A compressible short for packet serialization.
    /// </summary>
    [Serializable]
    public class CompressedShort : ICompressible<short>
    {
        public CompressedShort(short minValue, short maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

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
        /// The actual short.
        /// </summary>
        public short Value { get { return _value; } set { SetAndClamp(value); } }

        #region Public Methods
        /// <summary>
        /// Compress this short into a compact byte array.
        /// </summary>
        public byte[] Compress()
        {
            // Get the max range of this compressed short...
            uint range = (uint)(MaxValue - MinValue);
            // ... and the required bytes to hold it.
            uint required = Maths.GetRequiredBytes(range, sizeof(short));

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

        // Clamps the value if necessary and sets the underlying value.
        void SetAndClamp(short value)
        {
            // Clamp value if out of bounds.
            if (value < MinValue || value > MaxValue)
            {
                this.Value = value < MinValue ? MinValue : MaxValue;
                throw new ArgumentOutOfRangeException("Value was out of compression range and has been clamped.");
            }

            this.Value = value;
        }

        // Get the underlying short by using CompressedShort as a right-hand value.
        public static implicit operator short(CompressedShort cShort)
        {
            return cShort.Value;
        }
    }
}