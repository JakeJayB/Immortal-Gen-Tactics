using UnityEngine;

public class UnitInfo {
    // Unit
    public Unit unit;
    
    // Unit Cell Location
    public Vector3Int CellLocation;
    public UnitDirection UnitDirection;
    
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
    private int baseHP;
    private int baseMP;
    private int baseAP;
    private int baseAttack;
    private int baseDefense;
    private int baseMagicAttack;
    private int baseMagicDefense;
    private int baseMove;
    private int baseEvade;
    private int baseSpeed;
    private int baseSense;

    public UnitInfo(Unit unit) {
        this.unit = unit;
    }
    
    public void Initialize(UnitDefinitionData udd) {
        
        // Unit Affiliation
        UnitAffiliation = udd.UnitAffiliation;
        
        // Set the Base Stats
        baseHP = udd.BaseStats.BaseHP;
        baseMP = udd.BaseStats.BaseMP;
        baseAP = udd.BaseStats.BaseAP;
        baseAttack = udd.BaseStats.BaseAttack;
        baseDefense = udd.BaseStats.BaseDefense;
        baseMagicAttack = udd.BaseStats.BaseMagicAttack;
        baseMagicDefense = udd.BaseStats.BaseMagicDefense;
        baseMove = udd.BaseStats.BaseMove;
        baseEvade = udd.BaseStats.BaseEvade;
        baseSpeed = udd.BaseStats.BaseSpeed;
        baseSense = udd.BaseStats.BaseSense;
    }

    public void ResetCurrentStatPoints() {
        currentHP = FinalHP;
        currentMP = FinalMP;
        currentAP = FinalAP;
        currentCT = 0;
    }
    
    public void ApplyEquipmentBonuses(EquipmentBonus bonus) {
        FinalHP = baseHP + bonus.HP;
        FinalMP = baseMP + bonus.MP;
        FinalAP = baseAP + bonus.AP;
        FinalAttack = baseAttack + bonus.Attack;
        FinalMagicAttack = baseMagicAttack + bonus.MagicAttack;
        FinalDefense = baseDefense + bonus.Defense;
        FinalMagicDefense = baseMagicDefense + bonus.MagicDefense;
        FinalMove = baseMove + bonus.Move;
        FinalEvade = baseEvade + bonus.Evade;
        FinalSpeed = baseSpeed + bonus.Speed;
        FinalSense = baseSense;
    }
    
    public Vector2Int Vector2CellLocation() { return new Vector2Int(CellLocation.x, CellLocation.z); }
    
    public void RefreshAP() { currentAP = FinalAP; }

    public void Die() {
        if(Dead) return; 
        Dead = true;
        unit.GameObj.GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 0.1f, 1);
        TurnSystem.RemoveUnit(unit);
    }
    
    public void Revive() {
        if(!Dead) return; 
        Dead = false;
        unit.GameObj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        TurnSystem.AddUnit(unit);
    }

    public bool IsDead() { return Dead; }
}