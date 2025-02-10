using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfo : MonoBehaviour
{
    // Unit Current Stat Values
    private int currentHP;
    private int currentMP;
    private int currentAP;
    
    // Unit Final Stat Values
    # region Unit Final Stat Values
    [SerializeField] private int finalHP;
    [SerializeField] private int finalMP;
    [SerializeField] private int finalAP;
    [SerializeField] private int finalAttack;
    [SerializeField] private int finalDefense;
    [SerializeField] private int finalMagicAttack;
    [SerializeField] private int finalMagicDefense;
    [SerializeField] private int finalMove;
    [SerializeField] private int finalSpeed;
    # endregion
    
    // Unit Base Stat Values
    [SerializeField] private int baseHP;
    [SerializeField] private int baseMP;
    [SerializeField] private int baseAP;
    [SerializeField] private int baseAttack;
    [SerializeField] private int baseDefense;
    [SerializeField] private int baseMagicAttack;
    [SerializeField] private int baseMagicDefense;
    [SerializeField] private int baseMove;
    [SerializeField] private int baseSpeed;
    
    // Unit Equipment
    private UnitEquipment equipment;
    
    // Start is called before the first frame update
    void Start()
    {
        equipment = new UnitEquipment(this);
        equipment.EquipLeftHand(EquipmentLibrary.Weapons[0]);
        equipment.EquipArmor(EquipmentLibrary.Armor[100]);
        equipment.EquipArmor(EquipmentLibrary.Armor[101]);
        equipment.EquipArmor(EquipmentLibrary.Armor[102]);
        equipment.EquipArmor(EquipmentLibrary.Armor[103]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyEquipmentBonuses()
    {
        finalHP = baseHP + equipment.bonusHP;
        finalMP = baseMP + equipment.bonusMP;
        finalAP = baseAP + equipment.bonusAP;
        finalAttack = baseAttack + equipment.bonusAttack;
        finalMagicAttack = baseMagicAttack + equipment.bonusMagicAttack;
        finalDefense = baseDefense + equipment.bonusDefense;
        finalMagicDefense = baseMagicDefense + equipment.bonusMagicDefense;
        finalMove = baseMove + equipment.bonusMove;
        finalSpeed = baseSpeed + equipment.bonusSpeed;
    }
    
}