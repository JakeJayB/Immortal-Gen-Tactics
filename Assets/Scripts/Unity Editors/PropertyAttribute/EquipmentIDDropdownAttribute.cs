using UnityEngine;

public enum EquipmentType { Weapon, ArmorHead, ArmorBody, ArmorHands, ArmorFeet, Accessory }

public class EquipmentIDDropdownAttribute : PropertyAttribute {
    public EquipmentType Type { get; private set; }
    public EquipmentIDDropdownAttribute(EquipmentType type) { Type = type; }
}
