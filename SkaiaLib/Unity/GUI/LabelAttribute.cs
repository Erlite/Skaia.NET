
// -------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Add a label to a property.
// -------------------------------------------------

using System;
using UnityEngine;

namespace Skaia.Unity.GUI
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class LabelAttribute : PropertyAttribute
    {
        public string Label = string.Empty;

        /// <summary>
        /// Add a label on top of this property.
        /// </summary>
        /// <param name="label"> The text to display. </param>
        public LabelAttribute(string label)
        {
            Label = label;
        }
    }
}