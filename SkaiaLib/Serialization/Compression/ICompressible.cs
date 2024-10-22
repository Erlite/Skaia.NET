﻿
// -----------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Provide an interface for compressible types.
// -----------------------------------------------------

namespace Skaia.Serialization
{
    public interface ICompressible<T>
    {
        // Should we use compression?
        bool CompressionEnabled { get; set; }
        // The actual value of this instance.
        T Value { get; set; }
        // Compress the current value.
        byte[] Compress();
        // Decompress a byte array into T
        T Decompress(byte[] data);
    }
}