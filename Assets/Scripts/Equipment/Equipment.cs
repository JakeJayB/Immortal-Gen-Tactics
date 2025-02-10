public class Weapon : Equipment
{
    // Weapon Info
    private int maxDurability;
    private int durability;
    private int range;

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

public class Accessory : Equipment { }

public class Equipment
{
    // Equipment Info
    protected string equipName;

    // Equipment Stat Values
    protected int equipHP;
    protected int equipMP;
    protected int equipAP;
    protected int equipAttack;
    protected int equipMagicAttack;
    protected int equipDefense;
    protected int equipMagicDefense;
    protected int equipMove;
    protected int equipSpeed;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}