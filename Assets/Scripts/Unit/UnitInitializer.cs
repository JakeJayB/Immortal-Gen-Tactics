using System;
using UnityEngine;

[Serializable]
public class ActionInitializer {
    [UnitActionIDDropdown(UnitActionType.Action)] public int[] Actions;
}

[Serializable]
public class ItemInitializer {
    [UnitActionIDDropdown(UnitActionType.Item)] public int[] Items;
}

[Serializable]
public class EquipmentInitializer {
    [EquipmentIDDropdown(EquipmentType.Weapon)] public int WeaponL;
    [EquipmentIDDropdown(EquipmentType.Weapon)] public int WeaponR;
    [EquipmentIDDropdown(EquipmentType.ArmorHead)] public int ArmorHead;
    [EquipmentIDDropdown(EquipmentType.ArmorBody)] public int ArmorBody;
    [EquipmentIDDropdown(EquipmentType.ArmorHands)] public int ArmorHands;
    [EquipmentIDDropdown(EquipmentType.ArmorFeet)] public int ArmorFeet;
    [EquipmentIDDropdown(EquipmentType.Accessory)] public int AccessoryA;
    [EquipmentIDDropdown(EquipmentType.Accessory)] public int AccessoryB;
}

[Serializable]
public class UnitInitializer : MonoBehaviour
{
    public ActionInitializer Actions;
    public ItemInitializer Items;
    public EquipmentInitializer Equipment;
}
