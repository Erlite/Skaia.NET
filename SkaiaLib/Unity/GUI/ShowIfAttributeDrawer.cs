
// -------------------------------------------------------------------------------------------------------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Allows developers to mark fields/properties with ShowIfAttribute to only show the variable in the Inspector if a condition is met.
// -------------------------------------------------------------------------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Skaia.Unity.GUI
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //get the attribute data
            ShowIfAttribute condHAtt = (ShowIfAttribute)attribute;
            //check if the propery we want to draw should be enabled
            bool enabled = GetShowIfAttributeResult(condHAtt, property);

            //Enable/disable the property
            bool wasEnabled = UnityEngine.GUI.enabled;
            UnityEngine.GUI.enabled = enabled;

            //Check if we should draw the property
            if (!condHAtt.ShouldShow || enabled)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            //Ensure that the next property that is being drawn uses the correct settings
            UnityEngine.GUI.enabled = wasEnabled;
        }

        private bool GetShowIfAttributeResult(ShowIfAttribute showIfAttr, SerializedProperty property)
        {
            bool enabled = true;
            //Look for the sourcefield within the object that the property belongs to
            string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
            string conditionPath = propertyPath.Replace(property.name, showIfAttr.FieldCondition); //changes the path to the conditionalsource property path
            SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

            if (sourcePropertyValue != null)
            {
                enabled = sourcePropertyValue.boolValue;
            }
            else
            {
                Debug.LogWarning("Attempting to use a ShowIfAttribute but no matching SourcePropertyValue found in object: " + showIfAttr.FieldCondition);
            }

            return enabled;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIfAttr = (ShowIfAttribute)attribute;
            bool enabled = GetShowIfAttributeResult(showIfAttr, property);

            if (!showIfAttr.ShouldShow || enabled)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            else
            {
                //The property is not being drawn
                //We want to undo the spacing added before and after the property
                return -EditorGUIUtility.standardVerticalSpacing;
            }
        }
    }
}