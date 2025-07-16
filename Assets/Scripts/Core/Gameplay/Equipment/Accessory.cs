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