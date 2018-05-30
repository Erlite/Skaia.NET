
// ----------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Provide an abstract compression class.
// ----------------------------------------------------


// -----------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Provide an interface for compressible types.
// -----------------------------------------------------

namespace Skaia.Serialization
{
    public interface ICompressible<T>
    {
        // The minimum possible value of T.
        T MinValue { get; }
        // The maximum possible value of T.
        T MaxValue { get; }
        // The uncompressed value itself.
        T Value { get; set; }
        // Compress the current value.
        byte[] Compress();
        // Decompress a byte array and set this value.
        void Decompress(byte[] data);
    }
}