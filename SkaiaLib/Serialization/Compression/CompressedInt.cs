
// ----------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Provide a compressed int surrogate.
// ----------------------------------------------------

using Skaia.Logging;
using Skaia.Utils;
using System;

namespace Skaia.Serialization
{
    public class CompressedInt : ICompressible<int>
    {
        public CompressedInt(int minValue, int maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        private int _value;

        /// <summary>
        /// The minimum value of this int.
        /// </summary>
        public int MinValue { get; }
        /// <summary>
        /// The maximum value of this int.
        /// </summary>
        public int MaxValue { get; }

        /// <summary>
        /// The actual int.
        /// </summary>
        public int Value { get { return _value; } set { SetAndClamp(value); } }

        #region Public Methods
        /// <summary>
        /// Compress this int into a compact byte array.
        /// </summary>
        public byte[] Compress()
        {
            // Get the max range of this compressed int...
            uint range = (uint)(MaxValue - MinValue);
            // ... and the required bytes to hold it.
            uint required = Maths.GetRequiredBytes(range, sizeof(int));

            // Now we grab the actual value to compress.
            // For that we just substract the MinValue from the current value.
            int compressed = Value - MinValue;
            
            // Convert into a byte array...
            byte[] source = BitConverter.GetBytes(compressed);
            byte[] destination = new byte[required];

            // Copy the required bytes into the destination array to get rid of the useless bytes.
            Array.Copy(source, destination, required);
            return destination;
        }

        /// <summary>
        /// Decompress a compressed int from a compact byte array.
        /// </summary>
        public void Decompress(byte[] data)
        {
            // Make a byte array as big as the size of a int.
            byte[] decompressed = new byte[sizeof(int)];
            Array.Copy(data, decompressed, data.Length);
            int decompressedint = BitConverter.ToUInt16(decompressed, 0);

            // Set the value by adding the MinValue to the decompressed int.
            Value = MinValue + decompressedint;
        }
        #endregion Public Methods

        // Clamps the value if necessary and sets the underlying value.
        void SetAndClamp(int value)
        {
            // Clamp value if out of bounds.
            if (value < MinValue || value > MaxValue)
            {
                this.Value = value < MinValue ? MinValue : MaxValue;
                throw new ArgumentOutOfRangeException("Value was out of compression range and has been clamped.");
            }

            this.Value = value;
        }

        // Get the underlying int by using Compressedint as a right-hand value.
        public static implicit operator int(CompressedInt cInt)
        {
            return cInt.Value;
        }
    }
}