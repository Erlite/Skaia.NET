
// ---------------------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
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