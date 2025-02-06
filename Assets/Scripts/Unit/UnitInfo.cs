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
    private int finalHP;
    private int finalMP;
    private int finalAP;
    private int finalAttack;
    private int finalDefense;
    private int finalMagicAttack;
    private int finalMagicDefense;
    private int finalMove;
    private int finalSpeed;
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
    [SerializeField] private UnitEquipment equipment;
    
    // Start is called before the first frame update
    void Start()
    {
        equipment = gameObject.AddComponent<UnitEquipment>();
        equipment.EquipLeftHand(WeaponLibrary.Library[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}