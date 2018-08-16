
// -------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Show a property if the condition is met.
// -------------------------------------------------

using System;
using UnityEngine;

namespace Skaia.Unity.GUI
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public string FieldCondition = string.Empty;
        public bool ShouldShow = false;

        /// <summary>
        /// Mark a property to only show it if a condition is met.
        /// </summary>
        /// <param name="fieldName"> The field to check. Must be of type boolean. </param>
        /// <param name="show"> True to show this property if the field returns true. False to hide. </param>
        public ShowIfAttribute(string fieldName, bool show)
        {
            FieldCondition = fieldName;
            ShouldShow = show;
        }
    }
}