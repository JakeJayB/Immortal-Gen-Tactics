using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class EquipmentData
{
    // Equipment Info
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
}

[Serializable]
public class WeaponData : EquipmentData
{
    // Weapon Info
    public int maxDurability;
    public int range;
}

[Serializable]
public class ArmorData : EquipmentData
{
    // Armor Info
    public ArmorType armorType;
}

[Serializable]
public class AccessoryData : EquipmentData
{
    
}

[Serializable]
public class EquipmentList
{
    public List<WeaponData> Weapons = new List<WeaponData>();
    public List<ArmorData> Armor = new List<ArmorData>();
    public List<AccessoryData> Accessories = new List<AccessoryData>();
}

public class EquipmentLibrary
{
    public const string DEFAULT_DIRECTORY = "Assets/Resources/JSON/";
    public const string FILE_NAME = "EquipmentLibrary.json";
    public static Dictionary<int, Weapon> Weapons { get; private set; }
    public static Dictionary<int, Armor> Armor { get; private set; }
    public static Dictionary<int, Accessory> Accessories { get; private set; }

    public static void InitializeLibrary()
    {
        Weapons = new Dictionary<int, Weapon>();
        Armor = new Dictionary<int, Armor>();
        Accessories = new Dictionary<int, Accessory>();
        LoadFromJSON();
    }
    
    private static void LoadFromJSON()
    {
        string filePath = DEFAULT_DIRECTORY + FILE_NAME;

        if (File.Exists(filePath))
        {
            EquipmentList equipmentList = JsonUtility.FromJson<EquipmentList>(File.ReadAllText(filePath));
            UpdateLibrary(equipmentList);
        }
        else
        {
            Debug.LogError("File '" + filePath + "' does not exist...");
        }
    }

    private static void UpdateLibrary(EquipmentList equipmentList)
    {
        foreach (var weapon in equipmentList.Weapons)
        {
            Weapons.TryAdd(weapon.ID, new Weapon(weapon));
            Debug.Log(weapon.name + " has been added to WeaponLibrary. (ID: " + weapon.ID + ")");
        }
        
        foreach (var armor in equipmentList.Armor)
        {
            Armor.TryAdd(armor.ID, new Armor(armor));
            Debug.Log(armor.name + " has been added to WeaponLibrary. (ID: " + armor.ID + ")");
        }

        foreach (var accessory in equipmentList.Accessories)
        {
            Accessories.TryAdd(accessory.ID, new Accessory(accessory));
            Debug.Log(accessory.name + " has been added to WeaponLibrary. (ID: " + accessory.ID + ")");
        }
    }
}
