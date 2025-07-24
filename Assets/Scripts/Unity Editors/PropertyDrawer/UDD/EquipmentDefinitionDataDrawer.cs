using UnityEditor;

#if UNITY_EDITOR
namespace IGT.UnityEditor {
    [CustomPropertyDrawer(typeof(EquipmentDefinitionData))]
    public class EquipmentDefinitionDataDrawer : PropertyDrawer {
        [EquipmentIDDropdown(EquipmentType.Weapon)] public int WeaponL;
        [EquipmentIDDropdown(EquipmentType.Weapon)] public int WeaponR;
        [EquipmentIDDropdown(EquipmentType.ArmorHead)] public int ArmorHead;
        [EquipmentIDDropdown(EquipmentType.ArmorBody)] public int ArmorBody;
        [EquipmentIDDropdown(EquipmentType.ArmorHands)] public int ArmorHands;
        [EquipmentIDDropdown(EquipmentType.ArmorFeet)] public int ArmorFeet;
        [EquipmentIDDropdown(EquipmentType.Accessory)] public int AccessoryA;
        [EquipmentIDDropdown(EquipmentType.Accessory)] public int AccessoryB;
    }
}
#endif