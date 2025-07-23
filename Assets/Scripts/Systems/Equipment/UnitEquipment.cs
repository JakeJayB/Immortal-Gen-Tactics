using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitEquipment {
    private Unit unit;
    public Weapon leftHand;
    public Weapon rightHand;

    public Dictionary<ArmorType, Armor> armorSlots { get; private set; } = new() {
        { ArmorType.Head, null },
        { ArmorType.Body, null },
        { ArmorType.Hands, null },
        { ArmorType.Feet, null }
    };

    public Accessory accessoryA;
    public Accessory accessoryB;
    
    // Bonus Stat Values
    public EquipmentBonus EquipmentBonus;

    public UnitEquipment(Unit unit) {
        this.unit = unit;
        EquipmentBonus = new EquipmentBonus(unit.UnitInfo);
    }

    public void Initialize(UnitDefinitionData unitData) {
        var equipmentSet = unitData.GetEquipment();
        if (equipmentSet.Length != 8) { Debug.LogError("Equipment Set Does Not Reference All 8 Slots");}
        
        EquipmentLibrary.InitializeLibrary();
        EquipLeftHand(EquipmentLibrary.Weapons[equipmentSet[0]]);
        EquipRightHand(EquipmentLibrary.Weapons[equipmentSet[1]]);
        EquipArmor(EquipmentLibrary.Armor[equipmentSet[2]]);
        EquipArmor(EquipmentLibrary.Armor[equipmentSet[3]]);
        EquipArmor(EquipmentLibrary.Armor[equipmentSet[4]]);
        EquipArmor(EquipmentLibrary.Armor[equipmentSet[5]]);
        EquipAccessoryA(EquipmentLibrary.Accessories[equipmentSet[6]], unitData.Items.StorageAItems);
        EquipAccessoryB(EquipmentLibrary.Accessories[equipmentSet[7]], unitData.Items.StorageBItems);
        
        EquipmentBonus.ApplyToUnit();
    }
    
    // Equip Functions
    public void EquipLeftHand(Weapon weapon) {
        if (weapon == null) return;
        RemoveLeftWeapon();
        leftHand = weapon; 
        AddEquipmentBonus(weapon);
        Debug.Log("Left hand successfully equipped.");
    }

    public void EquipRightHand(Weapon weapon) {
        if (weapon == null) return;
        RemoveRightWeapon();
        rightHand = weapon; 
        AddEquipmentBonus(weapon);
        Debug.Log("Right hand successfully equipped.");
    }

    public void EquipArmor(Armor armor) {
        if (armor == null) return;
        if (!armorSlots.ContainsKey(armor.armorType)) {
            Debug.LogError("Invalid Armor Type");
            return;
        }

        RemoveArmor(armor.armorType);
        armorSlots[armor.armorType] = armor;
        AddEquipmentBonus(armor);
        Debug.Log($"{armor.armorType} successfully equipped.");
    }

    public void EquipAccessoryA(Accessory accessory, int[] accAItems = null) {
        if (accessory == null) return;
        accessoryA = accessory;
        AddEquipmentBonus(accessoryA);
        
        var accAction = UnitActionLibrary.FindAction(accessory.equipAction);
        if (accAction != null) {
            switch (accAction.ActionType) {
                case ActionType.Accessory:
                    break;
                case ActionType.Storage:
                    ApplyStorageAction(accAction, accAItems);
                    break;
                default:
                    Debug.Log("");
                    break;
            }
        }
        
        Debug.Log("AccessoryA successfully equipped.");
    }

    public void EquipAccessoryB(Accessory accessory, int[] accBItems = null) {
        if (accessory == null) return;
        accessoryB = accessory;
        AddEquipmentBonus(accessoryB);
        
        var accAction = UnitActionLibrary.FindAction(accessory.equipAction);
        if (accAction != null) {
            switch (accAction.ActionType) {
                case ActionType.Accessory:
                    break;
                case ActionType.Storage:
                    ApplyStorageAction(accAction, accBItems);
                    break;
                default:
                    Debug.Log("");
                    break;
            }
        }
        
        Debug.Log("AccessoryB successfully equipped.");
    }
    
    // Remove Equipment Functions
    private void RemoveLeftWeapon() {
        if (leftHand == null) return;
        RemoveEquipmentBonus(leftHand);
        leftHand = null;
    }
    
    private void RemoveRightWeapon() {
        if (rightHand == null) return;
        RemoveEquipmentBonus(rightHand);
        rightHand = null;
    }
    
    private void RemoveArmor(ArmorType armorType) {
        if (armorSlots[armorType] == null) return;
        RemoveEquipmentBonus(armorSlots[armorType]);
        armorSlots[armorType] = null;
    }
    
    private void RemoveAccessoryA() {
        if (accessoryA == null) return;
        RemoveEquipmentBonus(accessoryA);
        accessoryA = null;
    }
    
    private void RemoveAccessoryB() {
        if (accessoryB == null) return;
        RemoveEquipmentBonus(accessoryB);
        accessoryB = null;
    }
    
    private void AddEquipmentBonus(Equipment equipment, bool applyImmediately = false) {
        EquipmentBonus += equipment;
        if (applyImmediately) EquipmentBonus.ApplyToUnit();
    }
    
    private void RemoveEquipmentBonus(Equipment equipment, bool applyImmediately = false) {
        EquipmentBonus -= equipment;
        if (applyImmediately) EquipmentBonus.ApplyToUnit();
    }

    private void ApplyEquipmentAction(UnitAction action) {
        unit.ActionSet.AddAction(action);
    }

    private void ApplyStorageAction(UnitAction action, int[] accItems) {
        if (unit is AIUnit) { return; }
        if (action is Storage storage) { storage.Initialize(accItems); }
        unit.ActionSet.AddAction(action);
    }
}