using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class StatsDefinitionData {
    public int BaseHP;
    public int BaseMP;
    public int BaseAP;
    public int BaseAttack;
    public int BaseDefense;
    public int BaseMagicAttack;
    public int BaseMagicDefense;
    public int BaseMove;
    public int BaseEvade;
    public int BaseSpeed;
    public int BaseSense;
}

[Serializable]
public class ActionDefinitionData {
    [UnitActionIDDropdown(UnitActionType.Action)] public int[] Actions;
}

[Serializable]
public class ItemDefinitionData {
    [UnitActionIDDropdown(UnitActionType.Item)] public int[] StorageAItems;
    [UnitActionIDDropdown(UnitActionType.Item)] public int[] StorageBItems;

    public int[] All() { return StorageAItems.Concat(StorageBItems).ToArray(); }
}

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

[Serializable]
public class AIDefinitionData
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
public class UnitDefinitionData
{
    public StatsDefinitionData BaseStats = new();
    public ActionDefinitionData Actions = new();
    public ItemDefinitionData Items = new();
    public EquipmentDefinitionData Equipment = new();
    public AIDefinitionData Behaviors = new();
    public UnitAffiliation UnitAffiliation;
    
    public int[] GetActions() { return Actions.Actions; }
    public int[] GetItems() { return Items.All(); }
    public int[] GetEquipment() { return Equipment.All(); }
    public float[] GetBehaviors() { return Behaviors.All(); }
    public int[] GetAIActions() {
        var actions = Actions?.Actions ?? Array.Empty<int>();
        var items = Items?.All() ?? Array.Empty<int>();
        return actions.Concat(items).ToArray();
    }
}
