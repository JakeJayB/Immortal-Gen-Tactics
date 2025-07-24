using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class EquipmentList {
    public List<WeaponData> Weapons = new List<WeaponData>();
    public List<ArmorData> Armor = new List<ArmorData>();
    public List<AccessoryData> Accessories = new List<AccessoryData>();
}

public static class EquipmentLibrary
{
    public const string DEFAULT_DIRECTORY = "Assets/Resources/JSON/";
    public const string FILE_NAME = "EquipmentLibrary.json";
    private static Dictionary<int, Weapon> Weapons;
    private static Dictionary<int, Armor> Armor;
    private static Dictionary<int, Accessory> Accessories;
    
    public static void Clear()
    {
        Weapons = null;
        Armor = null;
        Accessories = null;
    }

    public static void InitializeLibrary()
    {
        Weapons = new Dictionary<int, Weapon> { {-1, null} };
        Armor = new Dictionary<int, Armor> { {-1, null} };
        Accessories = new Dictionary<int, Accessory> { {-1, null} };
        LoadFromJSON();
    }
    
    public static T FindEquipment<T>(int id) where T : Equipment {
        if (typeof(T) == typeof(Weapon) && Weapons.TryGetValue(id, out var weapon))
            return weapon as T;
    
        if (typeof(T) == typeof(Armor) && Armor.TryGetValue(id, out var armor))
            return armor as T;
    
        if (typeof(T) == typeof(Accessory) && Accessories.TryGetValue(id, out var accessory))
            return accessory as T;

        return null;
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

        var options = Weapons
            .Select(pair => (pair.Key, pair.Value?.equipName ?? "None"))
            .OrderBy(t => t.Key)
            .ToList();
        
        options.Insert(0, (-1, "None"));
        return options;
    }

    public static List<(int id, string name)> GetArmorDropdownOptions(ArmorType type)
    {
        EnsureInitialized();

        var options = Armor
            .Where(pair => pair.Key == -1 || pair.Value?.armorType == type)
            .Select(pair => (pair.Key, pair.Value?.equipName ?? "None"))
            .OrderBy(t => t.Key)
            .ToList();
        
        options.Insert(0, (-1, "None"));
        return options;
    }

    public static List<(int id, string name)> GetAccessoryDropdownOptions()
    {
        EnsureInitialized();

        var options = Accessories
            .Select(pair => (pair.Key, pair.Value?.equipName ?? "None"))
            .OrderBy(t => t.Key)
            .ToList();
        
        options.Insert(0, (-1, "None"));
        return options;
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
