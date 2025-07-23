#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitDefinitionData))]
public class UnitInitializerEditor : Editor {
    public override void OnInspectorGUI() {
        serializedObject.Update();

        SerializedProperty actions = serializedObject.FindProperty("Actions");
        SerializedProperty items = serializedObject.FindProperty("Items");
        SerializedProperty equipment = serializedObject.FindProperty("Equipment");
        SerializedProperty behaviors = serializedObject.FindProperty("Behaviors");

        EditorGUILayout.PropertyField(actions);
        EditorGUILayout.PropertyField(equipment);

        SerializedProperty accessoryA = equipment.FindPropertyRelative("AccessoryA");
        SerializedProperty accessoryB = equipment.FindPropertyRelative("AccessoryB");

        SerializeStorageItems(accessoryA, items, "A");
        SerializeStorageItems(accessoryB, items, "B");

        EditorGUILayout.PropertyField(behaviors);
        serializedObject.ApplyModifiedProperties();
    }

    private void SerializeStorageItems(SerializedProperty accessory, SerializedProperty items, string accessorySlot) {
        int accID = accessory?.intValue ?? -1;
        int capacity = UnitActionLibrary.FindAction(accID) is Storage storage
            ? storage.Capacity
            : 0;
        
        SerializedProperty storageItems = items.FindPropertyRelative($"Storage{accessorySlot}Items");
        
        // Show and fix size for storage items
        if (capacity > 0) {
            storageItems.arraySize = capacity;
            EditorGUILayout.LabelField($"{EquipmentLibrary.Accessories[accID].equipName}[{accessorySlot}] --- (Capacity: {capacity})");
            for (int i = 0; i < capacity; i++) {
                EditorGUILayout.PropertyField(storageItems.GetArrayElementAtIndex(i), new GUIContent($"Item {i + 1}"));
            }
        } else {
            storageItems.arraySize = 0;
        }
    }
}
#endif