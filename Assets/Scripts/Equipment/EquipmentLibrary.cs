using System;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


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

[Serializable]
public class EquipmentList {
    public List<WeaponData> Weapons = new List<WeaponData>();
    public List<ArmorData> Armor = new List<ArmorData>();
    public List<AccessoryData> Accessories = new List<AccessoryData>();
}

public class EquipmentLibrary : MonoBehaviour
{
    public const string DEFAULT_DIRECTORY = "Assets/Resources/JSON/";
    public const string FILE_NAME = "EquipmentLibrary.json";
    public static Dictionary<int, Weapon> Weapons { get; private set; }
    public static Dictionary<int, Armor> Armor { get; private set; }
    public static Dictionary<int, Accessory> Accessories { get; private set; }


    public static void Clear()
    {
        Weapons = null;
        Armor = null;
        Accessories = null;
    }


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
    
    private static void EnsureInitialized()
    {
        if (Weapons == null || Armor == null || Accessories == null ||
            Weapons.Count == 0 && Armor.Count == 0 && Accessories.Count == 0)
        {
            InitializeLibraryEditorSafe();
        }
    }

    
#if UNITY_EDITOR
    public static List<(int id, string name)> GetWeaponDropdownOptions()
    {
        EnsureInitialized();

        return Weapons
            .Select(pair => (pair.Key, pair.Value.equipName))
            .OrderBy(t => t.Key)
            .ToList();
    }

    public static List<(int id, string name)> GetArmorDropdownOptions(ArmorType type)
    {
        EnsureInitialized();

        return Armor
            .Where(pair => pair.Value.armorType == type)
            .Select(pair => (pair.Key, pair.Value.equipName))
            .OrderBy(t => t.Key)
            .ToList();
    }

    public static List<(int id, string name)> GetAccessoryDropdownOptions()
    {
        EnsureInitialized();

        return Accessories
            .Select(pair => (pair.Key, pair.Value.equipName))
            .OrderBy(t => t.Key)
            .ToList();
    }
    
    private static void InitializeLibraryEditorSafe()
    {
        Weapons = new Dictionary<int, Weapon>();
        Armor = new Dictionary<int, Armor>();
        Accessories = new Dictionary<int, Accessory>();

        string filePath = DEFAULT_DIRECTORY + FILE_NAME;

        if (File.Exists(filePath))
        {
            EquipmentList equipmentList = JsonUtility.FromJson<EquipmentList>(File.ReadAllText(filePath));

            foreach (var weapon in equipmentList.Weapons)
            {
                if (!Weapons.ContainsKey(weapon.ID))
                    Weapons.Add(weapon.ID, new Weapon(weapon));
            }

            foreach (var armor in equipmentList.Armor)
            {
                if (!Armor.ContainsKey(armor.ID))
                    Armor.Add(armor.ID, new Armor(armor));
            }

            foreach (var accessory in equipmentList.Accessories)
            {
                if (!Accessories.ContainsKey(accessory.ID))
                    Accessories.Add(accessory.ID, new Accessory(accessory));
            }
        }
        else
        {
            Debug.LogError($"File '{filePath}' does not exist...");
        }
    }
#endif
}
