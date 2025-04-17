using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfo : MonoBehaviour
{
    // Unit Cell Location
    public Vector3Int CellLocation;
    public UnitDirection UnitDirection;
    public Sprite sprite;
    
    // Unit Affiliation
    public UnitAffiliation UnitAffiliation;

    // Unit Current Stat Values
    public int currentHP { get; protected internal set; }
    public int currentMP { get; protected internal set; }
    public int currentAP { get; protected internal set; }
    public int currentCT { get; protected internal set; }
    public int currentLevel { get; protected internal set; } = 1;
    
    protected int currentEXP;
    
    // Unit Final Stat Values
    # region Unit Final Stat Values
    [SerializeField] public int finalHP;
    [SerializeField] public int finalMP;
    [SerializeField] public int finalAP;
    [SerializeField] public int finalAttack;
    [SerializeField] public int finalDefense;
    [SerializeField] public int finalMagicAttack;
    [SerializeField] public int finalMagicDefense;
    [SerializeField] public int finalMove;
    [SerializeField] public int finalSpeed;
    [SerializeField] public int finalSense;
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
    [SerializeField] private int baseSense;
    
    // Unit Equipment
    public UnitEquipment equipment;
    
    // Unit Action Set
    public UnitActionSet ActionSet { get; protected internal set; }

    // Start is called before the first frame update
    void Start()
    {
        SetBaseStats();
        equipment = new UnitEquipment(this);
/*        equipment.EquipLeftHand(EquipmentLibrary.Weapons[0]);
        equipment.EquipRightHand(EquipmentLibrary.Weapons[1]);
        equipment.EquipArmor(EquipmentLibrary.Armor[100]);
        equipment.EquipArmor(EquipmentLibrary.Armor[101]);
        equipment.EquipArmor(EquipmentLibrary.Armor[102]);
        equipment.EquipArmor(EquipmentLibrary.Armor[103]);
        equipment.EquipAccessoryA(EquipmentLibrary.Accessories[200]);
        equipment.EquipAccessoryB(EquipmentLibrary.Accessories[201]);*/


        ActionSet = new UnitActionSet();
        ActionSet.AddAction(new SplashSpell());
        ActionSet.AddAction(new Pouch());
        ActionSet.AddAction(new Potion());
        
        ApplyEquipmentBonuses();
        ResetCurrentStatPoints();
    }

    private void ResetCurrentStatPoints()
    {
        currentHP = finalHP;
        currentMP = finalMP;
        currentAP = finalAP;
        currentCT = 0;
    }


    private void SetBaseStats()
    {
        float statMultipler = 1 + (currentLevel - 1) * 0.2f;

        baseHP = Mathf.RoundToInt(30 * statMultipler);
        baseMP = Mathf.RoundToInt(15 * statMultipler);
        baseAP = 2;
        baseAttack = Mathf.RoundToInt(2 * statMultipler);
        baseDefense = Mathf.RoundToInt(1 * statMultipler); 
        baseMagicAttack = Mathf.RoundToInt(3 * statMultipler);
        baseMagicDefense = Mathf.RoundToInt(2 * statMultipler);
        baseMove = 3;
        baseSpeed = Mathf.RoundToInt(5 * statMultipler);
        baseSense = 2;
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
        finalSense = baseSense;
    }
    
    public Vector2Int Vector2CellLocation() { return new Vector2Int(CellLocation.x, CellLocation.z); }
    
    public void RefreshAP() { currentAP = finalAP; }
}