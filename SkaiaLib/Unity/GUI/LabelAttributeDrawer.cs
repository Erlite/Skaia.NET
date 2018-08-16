
// ---------------------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Erlite @ VM
// Purpose: Allows developers to add a label on a serialized item.
// ---------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Skaia.Unity.GUI
{
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //get the attribute data
            LabelAttribute lAttr = (LabelAttribute)attribute;
            EditorGUI.LabelField(position, lAttr.Label);

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
    }
}