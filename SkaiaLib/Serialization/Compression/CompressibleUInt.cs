
// ----------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Provide a compressible int surrogate.
// ----------------------------------------------------

using Skaia.Unity.GUI;
using Skaia.Utils;
using System;
using UnityEngine;

namespace Skaia.Serialization
{
    /// <summary>
    /// A compressible uint for packet serialization.
    /// </summary>
    [Serializable]
    public class CompressibleUInt : INumericCompressible<uint>
    {
        public CompressibleUInt(uint minValue, uint maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        [SerializeField]
        private uint _value;
        [SerializeField]
        private bool _compressionEnabled = true;
        [SerializeField]
        [ShowIf("_compressionEnabled", true)]
        private uint _minValue, _maxValue;

        /// <summary>
        /// The minimum value of this uint.
        /// </summary>
        public uint MinValue { get { return _minValue; } set { _minValue = value; } }

        /// <summary>
        /// The maximum value of this uint.
        /// </summary>
        public uint MaxValue { get { return _maxValue; } set { _maxValue = value; } }

        /// <summary>
        /// The actual uint.
        /// </summary>
        public uint Value { get { return _value; } set { SetAndClamp(value); } }

        /// <summary>
        /// Whether we should compress this before serializing into a byte array.
        /// </summary>
        public bool CompressionEnabled { get { return _compressionEnabled; } set { _compressionEnabled = value; } }

        /// <summary>
        /// Compress this uint into a compact byte array.
        /// </summary>
        public byte[] Compress()
        {
            if (!CompressionEnabled)
            {
                return BitConverter.GetBytes(Value);
            }

            // Get the max range of this compressed int...
            uint range = (uint)(MaxValue - MinValue);
            // ... and the required bytes to hold it.
            uint required = Maths.GetRequiredBytes(range, sizeof(int));

            // Now we grab the actual value to compress.
            // For that we just substract the MinValue from the current value.
            uint compressed = Value - MinValue;

            // Convert into a byte array...
            byte[] source = BitConverter.GetBytes(compressed);
            byte[] destination = new byte[required];

            // Copy the required bytes into the destination array to get rid of the useless bytes.
            Array.Copy(source, destination, required);
            return destination;
        }

        /// <summary>
        /// Decompress a compressed uint from a compact byte array.
        /// </summary>
        public void Decompress(byte[] data)
        {
            if (!CompressionEnabled)
            {
                Value = BitConverter.ToUInt32(data, 0);
                return;
            }

            // Make a byte array as big as the size of a int.
            byte[] decompressed = new byte[sizeof(int)];
            Array.Copy(data, decompressed, data.Length);
            uint decompressedint = BitConverter.ToUInt32(decompressed, 0);

            // Set the value by adding the MinValue to the decompressed int.
            Value = MinValue + decompressedint;
        }

        // Clamps the value if necessary and sets the underlying value.
        void SetAndClamp(uint value)
        {
            // Clamp value if out of bounds.
            if (value < MinValue || value > MaxValue)
            {
                _value = value < MinValue ? MinValue : MaxValue;
                throw new ArgumentOutOfRangeException("Value was out of compression range and has been clamped.");
            }

            _value = value;
        }

        // Get the underlying int by using CompressibleUInt as a right-hand value.
        public static implicit operator uint(CompressibleUInt cUInt)
        {
            return cUInt.Value;
        }
    }
}