using System;

[Serializable]
public class EquipmentData {
    public int ID;
    public string name;
    public int HP;
    public int MP;
    public int AP;
    public int attack;
    public int magicAttack;
    public int defense;
    public int magicDefense;
    public int move;
    public int speed;
    public int action;
}

[Serializable]
public class WeaponData : EquipmentData {
    public int maxDurability;
    public int range;
}

[Serializable]
public class ArmorData : EquipmentData {
    public ArmorType armorType;
}

[Serializable]
public class AccessoryData : EquipmentData { }
