
// ---------------------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Provide an interface for compression of numeric types.
// ---------------------------------------------------------------

namespace Skaia.Serialization
{
    public interface INumericCompressible<T> : ICompressible<T>
    {
        // The minimum possible value of T.
        T MinValue { get; }
        // The maximum possible value of T.
        T MaxValue { get; }
    }
}