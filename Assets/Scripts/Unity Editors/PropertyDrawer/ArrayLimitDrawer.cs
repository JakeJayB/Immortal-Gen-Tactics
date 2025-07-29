#if UNITY_EDITOR
using IGT.Core;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ArrayLimitAttribute))]
public class LimitArrayLengthDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var attr = (ArrayLimitAttribute)attribute;

        if (property.isArray && property.arraySize > attr.MaxLength)
            property.arraySize = attr.MaxLength;

        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
#endif