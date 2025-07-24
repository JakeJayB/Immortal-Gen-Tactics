using UnityEngine;

public class UnitInfo {
    public Unit unit;
    public Vector3Int CellLocation;
    public UnitDirection UnitDirection;
    public UnitAffiliation UnitAffiliation;
    
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
    public int baseHP { get; private set; }
    public int baseMP { get; private set; }
    public int baseAP { get; private set; }
    public int baseAttack { get; private set; }
    public int baseDefense { get; private set; }
    public int baseMagicAttack { get; private set; }
    public int baseMagicDefense { get; private set; }
    public int baseMove { get; private set; }
    public int baseEvade { get; private set; }
    public int baseSpeed { get; private set; }
    public int baseSense { get; private set; }

    public UnitInfo(Unit unit) {
        this.unit = unit;
    }
    
    public void Initialize(UnitDefinitionData udd) {
        FinalHP = baseHP = udd.BaseStats.BaseHP;
        FinalMP = baseMP = udd.BaseStats.BaseMP;
        FinalAP = baseAP = udd.BaseStats.BaseAP;
        FinalAttack = baseAttack = udd.BaseStats.BaseAttack;
        FinalDefense = baseDefense = udd.BaseStats.BaseDefense;
        FinalMagicAttack = baseMagicAttack = udd.BaseStats.BaseMagicAttack;
        FinalMagicDefense = baseMagicDefense = udd.BaseStats.BaseMagicDefense;
        FinalMove = baseMove = udd.BaseStats.BaseMove;
        FinalEvade = baseEvade = udd.BaseStats.BaseEvade;
        FinalSpeed = baseSpeed = udd.BaseStats.BaseSpeed;
        FinalSense = baseSense = udd.BaseStats.BaseSense;
        UnitAffiliation = udd.UnitAffiliation;
        ResetCurrentStatPoints();
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