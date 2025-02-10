using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEquipment : MonoBehaviour
{
    // Equipment Slots
    private Weapon leftHand;
    private Weapon rightHand;

    private Dictionary<ArmorType, Armor> armorSlots = new Dictionary<ArmorType, Armor>()
    {
        { ArmorType.Head, null },
        { ArmorType.Body, null },
        { ArmorType.Hands, null },
        { ArmorType.Feet, null }
    };
    
    private Accessory accessoryA;
    private Accessory accessoryB;
    
    // Bonus Stat Values
    private int bonusHP;
    private int bonusMP;
    private int bonusAP;
    private int bonusAttack;
    private int bonusMagicAttack;
    private int bonusDefense;
    private int bonusMagicDefense;
    private int bonusMove;
    private int bonusSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Equip Functions
    public void EquipLeftHand(Weapon weapon)
    {
        leftHand = weapon; 
        AddEquipmentBonus(weapon);
        Debug.Log("Left hand successfully equipped.");
    }

    public void EquipRightHand(Weapon weapon)
    {
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

        armorSlots[armor.armorType] = armor;
        AddEquipmentBonus(armor);
        Debug.Log($"{armor.armorType} successfully equipped.");
    }
    public void EquipAccessoryA(Accessory accessory) { accessoryA = accessory; }
    public void EquipAccessoryB(Accessory accessory) { accessoryB = accessory; }
    
    // Bonus Calculation Functions
    private void AddEquipmentBonus(Equipment equipment)
    {
        
    }
}