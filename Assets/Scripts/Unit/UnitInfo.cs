using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private bool Dead = false;
    
    // Unit Final Stat Values
    # region Unit Final Stat Values
    public int FinalHP { get; private set; }
    public int FinalMP { get; private set; }
    public int FinalAP { get; private set; }
    public int FinalAttack { get; private set; }
    public int FinalDefense { get; private set; }
    public int FinalMagicAttack { get; private set; }
    public int FinalMagicDefense { get; private set; }
    public int FinalMove { get; private set; }
    public int FinalEvade { get; private set; }
    public int FinalSpeed { get; private set; }
    public int FinalSense { get; private set; }
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
    [SerializeField] private int baseEvade;
    [SerializeField] private int baseSpeed;
    [SerializeField] private int baseSense;
    
    // Unit Equipment
    public UnitEquipment equipment;
    
    // Unit Action Set
    public UnitActionSet ActionSet { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        var unitInitializer = gameObject.GetComponent<UnitInitializer>();
        if (unitInitializer)
        {
            ActionSet = new UnitActionSet(unitInitializer, IsAIUnit());
            equipment = new UnitEquipment(this, unitInitializer);
            Destroy(unitInitializer);
        }
        else
        {
            SetBaseStats(); // TODO: This function needs to set base stats based on UnitInitializer.
            equipment = new UnitEquipment(this);
            equipment.EquipLeftHand(EquipmentLibrary.Weapons[0]);

            ActionSet = new UnitActionSet();
            ActionSet.AddAction(new SplashSpell());
            ActionSet.AddAction(new Heal());
            ActionSet.AddAction(new Revive());
            ActionSet.AddAction(new Pouch());
            ActionSet.AddAction(new Potion());
        }
        
        ApplyEquipmentBonuses();
        ResetCurrentStatPoints();
    }

    private void ResetCurrentStatPoints()
    {
        currentHP = FinalHP;
        currentMP = FinalMP;
        currentAP = FinalAP;
        currentCT = 0;
    }
    
    private void SetBaseStats()
    {
        float statMultipler = 1 + (currentLevel - 1) * 0.2f;

        baseHP = Mathf.RoundToInt(30 * statMultipler);
        baseMP = Mathf.RoundToInt(15 * statMultipler);
        baseAP = 2;
        baseAttack = Mathf.RoundToInt(3 * statMultipler);
        baseDefense = Mathf.RoundToInt(1 * statMultipler); 
        baseMagicAttack = Mathf.RoundToInt(4 * statMultipler);
        baseMagicDefense = Mathf.RoundToInt(2 * statMultipler);
        baseMove = 4;
        baseEvade = 1;
        baseSpeed = UnityEngine.Random.Range(8, 13);
        //baseSpeed = Mathf.RoundToInt(5 * statMultipler);
        baseSense = 2;
    }
    
    public void ApplyEquipmentBonuses()
    {
        FinalHP = baseHP + equipment.bonusHP;
        FinalMP = baseMP + equipment.bonusMP;
        FinalAP = baseAP + equipment.bonusAP;
        FinalAttack = baseAttack + equipment.bonusAttack;
        FinalMagicAttack = baseMagicAttack + equipment.bonusMagicAttack;
        FinalDefense = baseDefense + equipment.bonusDefense;
        FinalMagicDefense = baseMagicDefense + equipment.bonusMagicDefense;
        FinalMove = baseMove + equipment.bonusMove;
        FinalEvade = baseEvade + equipment.bonusEvade;
        FinalSpeed = baseSpeed + equipment.bonusSpeed;
        FinalSense = baseSense;
    }
    
    public Vector2Int Vector2CellLocation() { return new Vector2Int(CellLocation.x, CellLocation.z); }
    
    public void RefreshAP() { currentAP = FinalAP; }

    public bool IsAlive() { return currentHP > 0; }

    public void Die()
    {
        if(Dead) return; 
        Dead = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 0.1f, 1);
        TurnSystem.RemoveUnit(gameObject.GetComponent<Unit>());
    }
    
    public void Revive()
    {
        if(!Dead) return; 
        Dead = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        TurnSystem.AddUnit(gameObject.GetComponent<Unit>());
    }

    public bool IsDead() { return Dead; }

    public bool IsAIUnit() { return gameObject.GetComponent<EnemyUnit>(); }
}