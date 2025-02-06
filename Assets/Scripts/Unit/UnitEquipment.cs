using System.Collections;
using System.Collections.Generic;
using Equipment;
using UnityEngine;

public class UnitEquipment : MonoBehaviour
{
    // Equipment Slots
    private Weapon leftHand;
    private Weapon rightHand;
    private ArmorHead head;
    private ArmorBody body;
    private ArmorHands hands;
    private ArmorFeet feet;
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
    public void EquipLeftHand(Weapon weapon) { leftHand = weapon; Debug.Log("Left hand successfully equipped."); }
    public void EquipRightHand(Weapon weapon) { rightHand = weapon; }
    public void EquipHead(ArmorHead armor) { head = armor; }
    public void EquipBody(ArmorBody armor) { body = armor; }
    public void EquipHands(ArmorHands armor) { hands = armor; }
    public void EquipFeet(ArmorFeet armor) { feet = armor; }
    public void EquipAccessoryA(Accessory accessory) { accessoryA = accessory; }
    public void EquipAccessoryB(Accessory accessory) { accessoryB = accessory; }
}