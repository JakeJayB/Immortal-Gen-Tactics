using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(EquipmentIDDropdownAttribute))]
public class EquipmentIDDropdownDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EquipmentIDDropdownAttribute dropdownAttr = (EquipmentIDDropdownAttribute)attribute;

        // Get the correct list of options based on dropdown type
        var options = dropdownAttr.Type switch {
            EquipmentType.Weapon => EquipmentLibrary.GetWeaponDropdownOptions(),
            EquipmentType.ArmorHead => EquipmentLibrary.GetArmorDropdownOptions(ArmorType.Head),
            EquipmentType.ArmorBody => EquipmentLibrary.GetArmorDropdownOptions(ArmorType.Body),
            EquipmentType.ArmorHands => EquipmentLibrary.GetArmorDropdownOptions(ArmorType.Hands),
            EquipmentType.ArmorFeet => EquipmentLibrary.GetArmorDropdownOptions(ArmorType.Feet),
            EquipmentType.Accessory => EquipmentLibrary.GetAccessoryDropdownOptions(),
            _ => null
        };
        
        if (options == null || options.Count == 0) {
            EditorGUI.LabelField(position, label.text, "No weapons loaded.");
            return;
        }

        string[] displayNames = options.Select(opt => $"{opt.id}: {opt.name}").ToArray();
        int[] ids = options.Select(opt => opt.id).ToArray();

        int currentId = property.intValue;
        int index = Array.IndexOf(ids, currentId);
        if (index < 0) index = 0;

        int selectedIndex = EditorGUI.Popup(position, label.text, index, displayNames);
        property.intValue = ids[selectedIndex];
    }
}
#endif
