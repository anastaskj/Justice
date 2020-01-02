using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(IsometricCoordinates))]
public class IsometricCoordinateDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        IsometricCoordinates coords = new IsometricCoordinates(
            property.FindPropertyRelative("x").intValue,
            property.FindPropertyRelative("y").intValue
        );

        position = EditorGUI.PrefixLabel(position, label);
        GUI.Label(position, coords.ToStringGUI());
    }
}
