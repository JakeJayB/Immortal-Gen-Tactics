using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class UnitEquipment
{
    // Unit Info Reference
    private Unit unit;
    
    // Equipment Slots
    public Weapon leftHand;
    public Weapon rightHand;

    public Dictionary<ArmorType, Armor> armorSlots { get; private set; } = new Dictionary<ArmorType, Armor>()
    {
        { ArmorType.Head, null },
        { ArmorType.Body, null },
        { ArmorType.Hands, null },
        { ArmorType.Feet, null }
    };

    public Accessory accessoryA;
    public Accessory accessoryB;
    
    // Bonus Stat Values
    public int bonusHP { get; private set; }
    public int bonusMP { get; private set; }
    public int bonusAP { get; private set; }
    public int bonusAttack { get; private set; }
    public int bonusMagicAttack { get; private set; }
    public int bonusDefense { get; private set; }
    public int bonusMagicDefense { get; private set; }
    public int bonusMove { get; private set; }
    public int bonusEvade { get; private set; }
    public int bonusSpeed { get; private set; }

    public UnitEquipment(Unit unit)
    {
        this.unit = unit;
        InitializeBonusStats();
    }

    public void Initialize(UnitDefinitionData unitData)
    {
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
    }
    
    // Equip Functions
    public void EquipLeftHand(Weapon weapon)
    {
        if (weapon == null) return;
        RemoveLeftWeapon();
        leftHand = weapon; 
        AddEquipmentBonus(weapon);
        Debug.Log("Left hand successfully equipped.");
    }

    public void EquipRightHand(Weapon weapon)
    {
        if (weapon == null) return;
        RemoveRightWeapon();
        rightHand = weapon; 
        AddEquipmentBonus(weapon);
        Debug.Log("Right hand successfully equipped.");
    }

    public void EquipArmor(Armor armor)
    {
        if (armor == null) return;
        if (!armorSlots.ContainsKey(armor.armorType))
        {
            Debug.LogError("Invalid Armor Type");
            return;
        }

        RemoveArmor(armor);
        armorSlots[armor.armorType] = armor;
        AddEquipmentBonus(armor);
        Debug.Log($"{armor.armorType} successfully equipped.");
    }

    public void EquipAccessoryA(Accessory accessory, int[] accAItems = null)
    {
        if (accessory == null) return;
        accessoryA = accessory;
        AddEquipmentBonus(accessoryA);
        
        var accAction = UnitActionLibrary.FindAction(accessory.equipAction);
        if (accAction != null)
        {
            switch (accAction.ActionType)
            {
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

    public void EquipAccessoryB(Accessory accessory, int[] accBItems = null)
    {
        if (accessory == null) return;
        accessoryB = accessory;
        AddEquipmentBonus(accessoryB);
        
        var accAction = UnitActionLibrary.FindAction(accessory.equipAction);
        if (accAction != null)
        {
            switch (accAction.ActionType)
            {
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
    private void RemoveLeftWeapon()
    {
        if (leftHand == null) return;
        RemoveEquipmentBonus(leftHand);
        leftHand = null;
    }
    
    private void RemoveRightWeapon()
    {
        if (rightHand == null) return;
        RemoveEquipmentBonus(rightHand);
        rightHand = null;
    }
    
    private void RemoveArmor(Armor armor)
    {
        if (armorSlots[armor.armorType] == null) return;
        RemoveEquipmentBonus(armorSlots[armor.armorType]);
        armorSlots[armor.armorType] = null;
    }
    
    private void RemoveAccessoryA()
    {
        if (accessoryA == null) return;
        RemoveEquipmentBonus(accessoryA);
        accessoryA = null;
    }
    
    private void RemoveAccessoryB()
    {
        if (accessoryB == null) return;
        RemoveEquipmentBonus(accessoryB);
        accessoryB = null;
    }
    
    // Bonus Calculation Functions
    private void InitializeBonusStats()
    {
        bonusHP = 0;
        bonusMP = 0;
        bonusAP = 0;
        bonusAttack = 0;
        bonusMagicAttack = 0;
        bonusDefense = 0;
        bonusMagicDefense = 0;
        bonusMove = 0;
        bonusEvade = 0;
        bonusSpeed = 0;
    }
    
    private void AddEquipmentBonus(Equipment equipment)
    {
        bonusHP += equipment.equipHP;
        bonusMP += equipment.equipMP;
        bonusAP += equipment.equipAP;
        bonusAttack += equipment.equipAttack;
        bonusMagicAttack += equipment.equipMagicAttack;
        bonusDefense += equipment.equipDefense;
        bonusMagicDefense += equipment.equipMagicDefense;
        bonusMove += equipment.equipMove;
        bonusEvade += equipment.equipEvade;
        bonusSpeed += equipment.equipSpeed;
        
        unit.UnitInfo.ApplyEquipmentBonuses();
    }
    
    private void RemoveEquipmentBonus(Equipment equipment)
    {
        bonusHP -= equipment.equipHP;
        bonusMP -= equipment.equipMP;
        bonusAP -= equipment.equipAP;
        bonusAttack -= equipment.equipAttack;
        bonusMagicAttack -= equipment.equipMagicAttack;
        bonusDefense -= equipment.equipDefense;
        bonusMagicDefense -= equipment.equipMagicDefense;
        bonusMove -= equipment.equipMove;
        bonusSpeed -= equipment.equipSpeed;
        
        unit.UnitInfo.ApplyEquipmentBonuses();
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