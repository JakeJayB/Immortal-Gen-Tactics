using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEquipment
{
    // Unit Info Reference
    private UnitInfo unitInfo;
    
    // Equipment Slots
    public Weapon leftHand { get; private set; }
    public Weapon rightHand { get; private set; }

    public Dictionary<ArmorType, Armor> armorSlots { get; private set; } = new Dictionary<ArmorType, Armor>()
    {
        { ArmorType.Head, null },
        { ArmorType.Body, null },
        { ArmorType.Hands, null },
        { ArmorType.Feet, null }
    };
    
    public Accessory accessoryA { get; private set; }
    public Accessory accessoryB { get; private set; }
    
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

    public UnitEquipment(UnitInfo unitInfo)
    {
        this.unitInfo = unitInfo;
        InitializeBonusStats();
    }
    

    // Equip Functions
    public void EquipLeftHand(Weapon weapon)
    {
        RemoveLeftWeapon();
        leftHand = weapon; 
        AddEquipmentBonus(weapon);
        Debug.Log("Left hand successfully equipped.");
    }

    public void EquipRightHand(Weapon weapon)
    {
        RemoveRightWeapon();
        rightHand = weapon; 
        AddEquipmentBonus(weapon);
        Debug.Log("Right hand successfully equipped.");
    }

    public void EquipArmor(Armor armor)
    {
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

    public void EquipAccessoryA(Accessory accessory)
    {
        accessoryA = accessory;
        AddEquipmentBonus(accessoryA);
        Debug.Log("AccessoryA successfully equipped.");
    }

    public void EquipAccessoryB(Accessory accessory)
    {
        accessoryB = accessory;
        AddEquipmentBonus(accessoryB);
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
        
        unitInfo.ApplyEquipmentBonuses();
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
        
        unitInfo.ApplyEquipmentBonuses();
    }
}