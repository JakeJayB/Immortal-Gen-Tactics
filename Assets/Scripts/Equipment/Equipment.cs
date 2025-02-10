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
    public int equipSpeed { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}