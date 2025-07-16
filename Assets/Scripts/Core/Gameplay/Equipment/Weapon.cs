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