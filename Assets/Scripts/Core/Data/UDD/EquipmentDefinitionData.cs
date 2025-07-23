using System;

[Serializable]
public class EquipmentDefinitionData {
    [EquipmentIDDropdown(EquipmentType.Weapon)] public int WeaponL;
    [EquipmentIDDropdown(EquipmentType.Weapon)] public int WeaponR;
    [EquipmentIDDropdown(EquipmentType.ArmorHead)] public int ArmorHead;
    [EquipmentIDDropdown(EquipmentType.ArmorBody)] public int ArmorBody;
    [EquipmentIDDropdown(EquipmentType.ArmorHands)] public int ArmorHands;
    [EquipmentIDDropdown(EquipmentType.ArmorFeet)] public int ArmorFeet;
    [EquipmentIDDropdown(EquipmentType.Accessory)] public int AccessoryA;
    [EquipmentIDDropdown(EquipmentType.Accessory)] public int AccessoryB;

    public int[] All() {
        return new [] { WeaponL, WeaponR, ArmorHead, ArmorBody, ArmorHands, ArmorFeet, AccessoryA, AccessoryB };
    }
}
