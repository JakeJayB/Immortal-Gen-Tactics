#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class ValidItemAccessories
{
    public static readonly Dictionary<int, int> StorageCapacity = new()
    {
        { 202, 3 }, // Pouch
        { 102, 8 }, // Backpack
    };

    public static int GetCapacity(int accessoryId)
    {
        return StorageCapacity.GetValueOrDefault(accessoryId, 0);
    }
}

[CustomEditor(typeof(UnitDefinitionData))]
public class UnitInitializerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty actions = serializedObject.FindProperty("Actions");
        SerializedProperty items = serializedObject.FindProperty("Items");
        SerializedProperty equipment = serializedObject.FindProperty("Equipment");
        SerializedProperty behaviors = serializedObject.FindProperty("Behaviors");

        EditorGUILayout.PropertyField(actions);
        EditorGUILayout.PropertyField(equipment);

        SerializedProperty accessoryA = equipment.FindPropertyRelative("AccessoryA");
        SerializedProperty accessoryB = equipment.FindPropertyRelative("AccessoryB");

        int accA = accessoryA?.intValue ?? -1;
        int accB = accessoryB?.intValue ?? -1;

        int capA = ValidItemAccessories.GetCapacity(accA);
        int capB = ValidItemAccessories.GetCapacity(accB);

        SerializedProperty itemsA = items.FindPropertyRelative("StorageAItems");
        SerializedProperty itemsB = items.FindPropertyRelative("StorageBItems");

        // Show and fix size for Accessory A items
        if (capA > 0) {
            itemsA.arraySize = capA;
            EditorGUILayout.LabelField($"{EquipmentLibrary.Accessories[accA].equipName}[A] --- (Capacity: {capA})");
            for (int i = 0; i < capA; i++) {
                EditorGUILayout.PropertyField(itemsA.GetArrayElementAtIndex(i), new GUIContent($"Item {i + 1}"));
            }
        } else {
            itemsA.arraySize = 0;
        }

        // Show and fix size for Accessory B items
        if (capB > 0) {
            itemsB.arraySize = capB;
            EditorGUILayout.LabelField($"{EquipmentLibrary.Accessories[accB].equipName}[B] --- (Capacity: {capB})");
            for (int i = 0; i < capB; i++) {
                EditorGUILayout.PropertyField(itemsB.GetArrayElementAtIndex(i), new GUIContent($"Item {i + 1}"));
            }
        } else {
            itemsB.arraySize = 0;
        }

        EditorGUILayout.PropertyField(behaviors);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif