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
    private int baseHP;
    private int baseMP;
    private int baseAP;
    private int baseAttack;
    private int baseDefense;
    private int baseMagicAttack;
    private int baseMagicDefense;
    private int baseMove;
    private int baseSpeed;
    
    // Unit Equipment
    private UnitEquipment equipment;
    
    // Start is called before the first frame update
    void Start()
    {
        equipment = gameObject.AddComponent<UnitEquipment>();
        equipment.EquipLeftHand(EquipmentLibrary.Weapons[0]);
        equipment.EquipArmor(EquipmentLibrary.Armor[100]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}