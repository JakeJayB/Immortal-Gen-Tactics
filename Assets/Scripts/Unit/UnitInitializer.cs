using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class ActionInitializer {
    [UnitActionIDDropdown(UnitActionType.Action)] public int[] Actions;
}

[Serializable]
public class ItemInitializer {
    [UnitActionIDDropdown(UnitActionType.Item)] public int[] StorageAItems;
    [UnitActionIDDropdown(UnitActionType.Item)] public int[] StorageBItems;

    public int[] All() { return StorageAItems.Concat(StorageBItems).ToArray(); }
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

    public int[] All() {
        return new [] { WeaponL, WeaponR, ArmorHead, ArmorBody, ArmorHands, ArmorFeet, AccessoryA, AccessoryB };
    }
}

[Serializable]
public class AIBehaviorInitializer
{
    public float Aggression;
    public float Survival;
    public float TacticalPositioning;
    public float AllySynergy;
    public float ResourceManagement;
    public float ReactionAwareness;
    public float ReactionAllocation;

    public float[] All() {
        return new [] { Aggression, Survival, TacticalPositioning, AllySynergy, ResourceManagement, ReactionAwareness,
            ReactionAllocation };
    }
}

[Serializable]
public class UnitInitializer : MonoBehaviour
{
    public ActionInitializer Actions;
    public ItemInitializer Items;
    public EquipmentInitializer Equipment;
    public AIBehaviorInitializer Behaviors;
    
    public int[] GetActions() { return Actions.Actions; }
    public int[] GetItems() { return Items.All(); }
    public int[] GetEquipment() { return Equipment.All(); }
    public float[] GetBehaviors() { return Behaviors.All(); }
}
