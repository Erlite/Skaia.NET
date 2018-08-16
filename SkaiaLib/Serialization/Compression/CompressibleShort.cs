
// ----------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Provide a compressed short surrogate.
// ----------------------------------------------------

using Skaia.Unity.GUI;
using Skaia.Utils;
using System;
using UnityEngine;

namespace Skaia.Serialization
{
    /// <summary>
    /// A compressible short for packet serialization.
    /// </summary>
    [Serializable]
    public class CompressibleShort : INumericCompressible<short>
    {
        public CompressibleShort(short minValue, short maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        [SerializeField]
        private short _value;
        [SerializeField]
        private bool _compressionEnabled = true;
        [SerializeField]
        [ShowIf("_compressionEnabled", true)]
        private short _minValue, _maxValue;

        /// <summary>
        /// The minimum value of this short.
        /// </summary>
        public short MinValue { get { return _minValue; } set { _minValue = value; } }

        /// <summary>
        /// The maximum value of this short.
        /// </summary>
        public short MaxValue { get { return _maxValue; } set { _maxValue = value; } }

        /// <summary>
        /// The actual short.
        /// </summary>
        public short Value { get { return _value; } set { SetAndClamp(value); } }

        /// <summary>
        /// Whether we should compress this before serializing.
        /// </summary>
        public bool CompressionEnabled { get { return _compressionEnabled; } set { _compressionEnabled = value; } }

        /// <summary>
        /// Compress this short into a compact byte array.
        /// </summary>
        public byte[] Compress()
        {
            if (!CompressionEnabled)
            {
                return BitConverter.GetBytes(Value);
            }

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
            if (!CompressionEnabled)
            {
                Value = BitConverter.ToInt16(data, 0);
                return;
            }

            // Make a byte array as big as the size of a short.
            byte[] decompressed = new byte[sizeof(short)];
            Array.Copy(data, decompressed, data.Length);
            short decompressedShort = BitConverter.ToInt16(decompressed, 0);

            // Set the value by adding the MinValue to the decompressed short.
            Value = (short)(MinValue + decompressedShort);
        }

        // Clamps the value if necessary and sets the underlying value.
        void SetAndClamp(short value)
        {
            // Clamp value if out of bounds.
            if (value < MinValue || value > MaxValue)
            {
                _value = value < MinValue ? MinValue : MaxValue;
                throw new ArgumentOutOfRangeException("Value was out of compression range and has been clamped.");
            }

            _value = value;
        }

        // Get the underlying short by using CompressibleShort as a right-hand value.
        public static implicit operator short(CompressibleShort cShort)
        {
            return cShort.Value;
        }
    }
}