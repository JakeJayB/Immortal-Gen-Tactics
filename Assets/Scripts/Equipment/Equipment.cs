public class Weapon : Equipment
{
    // Weapon Info
    public int maxDurability { get; private set; }
    public int durability { get; private set; }
    public int range { get; private set; }

    public Weapon(WeaponData data)
    {
        equipName = data.name;
        equipHP = data.HP;
        equipMP = data.MP;
        equipAP = data.AP;
        equipAttack = data.attack;
        equipMagicAttack = data.magicAttack;
        equipDefense = data.defense;
        equipMagicDefense = data.magicDefense;
        equipMove = data.move;
        equipSpeed = data.speed;
        equipAction = data.action;
        maxDurability = data.maxDurability;
        range = data.range;
    }
}

public class Armor : Equipment
{
    public ArmorType armorType;

    public Armor(ArmorData data)
    {
        equipName = data.name;
        equipHP = data.HP;
        equipMP = data.MP;
        equipAP = data.AP;
        equipAttack = data.attack;
        equipMagicAttack = data.magicAttack;
        equipDefense = data.defense;
        equipMagicDefense = data.magicDefense;
        equipMove = data.move;
        equipSpeed = data.speed;
        equipAction = data.action;
        armorType = data.armorType;
    }
}

public class Accessory : Equipment
{
    public Accessory(AccessoryData data)
    {
        equipName = data.name;
        equipHP = data.HP;
        equipMP = data.MP;
        equipAP = data.AP;
        equipAttack = data.attack;
        equipMagicAttack = data.magicAttack;
        equipDefense = data.defense;
        equipMagicDefense = data.magicDefense;
        equipMove = data.move;
        equipSpeed = data.speed;
        equipAction = data.action;
    }
}

public class Equipment
{
    // Equipment Info
    public string equipName { get; protected set; }

    // Equipment Stat Values
    public int equipHP { get; protected set; }
    public int equipMP { get; protected set; }
    public int equipAP { get; protected set; }
    public int equipAttack { get; protected set; }
    public int equipMagicAttack { get; protected set; }
    public int equipDefense { get; protected set; }
    public int equipMagicDefense { get; protected set; }
    public int equipMove { get; protected set; }
    public int equipEvade { get; protected set; }
    public int equipSpeed { get; protected set; }
    public int equipAction { get; protected set; }
}