public class EquipmentBonus {
    public int HP;
    public int MP;
    public int AP;
    public int Attack;
    public int MagicAttack;
    public int Defense;
    public int MagicDefense;
    public int Move;
    public int Evade;
    public int Speed;

    private UnitInfo UnitInfo;
    
    public EquipmentBonus(UnitInfo unitInfo) {
        UnitInfo = unitInfo;
        Initialize();
    }
    
    private void Initialize() {
        HP = 0;
        MP = 0;
        AP = 0;
        Attack = 0;
        MagicAttack = 0;
        Defense = 0;
        MagicDefense = 0;
        Move = 0;
        Evade = 0;
        Speed = 0;
    }
    
    public static EquipmentBonus operator +(EquipmentBonus a, Equipment b) {
        return new EquipmentBonus(a.UnitInfo) {
            HP = a.HP + b.equipHP,
            MP = a.MP + b.equipMP,
            AP = a.AP + b.equipAP,
            Attack = a.Attack + b.equipAttack,
            MagicAttack = a.MagicAttack + b.equipMagicAttack,
            Defense = a.Defense + b.equipDefense,
            MagicDefense = a.MagicDefense + b.equipMagicDefense,
            Move = a.Move + b.equipMove,
            Evade = a.Evade + b.equipEvade,
            Speed = a.Speed + b.equipSpeed,
        };
    }
    
    public static EquipmentBonus operator -(EquipmentBonus a, Equipment b) {
        return new EquipmentBonus(a.UnitInfo) {
            HP = a.HP - b.equipHP,
            MP = a.MP - b.equipMP,
            AP = a.AP - b.equipAP,
            Attack = a.Attack - b.equipAttack,
            MagicAttack = a.MagicAttack - b.equipMagicAttack,
            Defense = a.Defense - b.equipDefense,
            MagicDefense = a.MagicDefense - b.equipMagicDefense,
            Move = a.Move - b.equipMove,
            Evade = a.Evade - b.equipEvade,
            Speed = a.Speed - b.equipSpeed,
        };
    }

    public void ApplyToUnit() {
        UnitInfo.ApplyEquipmentBonuses(this);
    }
}
