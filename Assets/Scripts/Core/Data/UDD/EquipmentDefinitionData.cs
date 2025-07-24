using System;
    
[Serializable]
public class EquipmentDefinitionData {
    public int WeaponL;
    public int WeaponR;
    public int ArmorHead;
    public int ArmorBody;
    public int ArmorHands;
    public int ArmorFeet;
    public int AccessoryA;
    public int AccessoryB;

    public int[] All() {
        return new [] { WeaponL, WeaponR, ArmorHead, ArmorBody, ArmorHands, ArmorFeet, AccessoryA, AccessoryB };
    }
}
