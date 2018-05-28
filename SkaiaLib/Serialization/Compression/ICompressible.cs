
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
    /// Base class to compress numeric types.
    /// Works best for numeric types, else well you're on your own.
    /// </summary>
    /// <typeparam name="T"> The type to compress. </typeparam>
    public interface ICompressible<T> where T : struct
    {
        // The minimum possible value of T.
        T MinValue { get; }
        // The maximum possible value of T.
        T MaxValue { get; }
        // The uncompressed value itself.
        T Value { get; set; }

        // Should the Value be clamped within the minimum and maximum value?
        bool ShouldClamp { get; set; }
         
        // Used to clamp Value between MinValue and MaxValue.
        void Clamp();

        // Gets the required bytes to serialize this value.
        uint GetRequiredBytes();

        // Compress the current value.
        byte[] Compress();

        // Decompress the current value.
        void Decompress(byte[] data);
    }
}