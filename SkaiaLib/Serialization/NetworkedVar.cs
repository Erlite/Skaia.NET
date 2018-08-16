
// -----------------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Provide an abstract class for networked variables.
// -----------------------------------------------------------

using UnityEngine;

namespace Skaia.Serialization
{
    public abstract class NetworkedVar<T>
    {
        [SerializeField]
        private bool _enableCompression;
    }
}
