using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Equipment;
using UnityEngine;

[Serializable]
public class WeaponData
{
    // Weapon ID
    public int ID;
    
    // Weapon Properties
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
    public int maxDurability;
    public int range;
}

[Serializable]
public class WeaponList
{
    public List<WeaponData> Weapons = new List<WeaponData>();
}

public class WeaponLibrary : MonoBehaviour
{
    private const string DEFAULT_DIRECTORY = "Assets/Resources/JSON";
    public const string FILE_NAME = "WeaponLibrary.json";
    public static Dictionary<int, Weapon> Library { get; private set; }
    
    // Start is called before the first frame update
    void Awake()
    {
        Library = new Dictionary<int, Weapon>();
        LoadFromJSON();
    }

    private void LoadFromJSON()
    {
        string filePath = DEFAULT_DIRECTORY + "/" + FILE_NAME;

        if (File.Exists(filePath))
        {
            WeaponList weaponList = JsonUtility.FromJson<WeaponList>(File.ReadAllText(filePath));
            UpdateLibrary(weaponList.Weapons);
        }
        else
        {
            Debug.LogError("File '" + filePath + "' does not exist...");
        }
    }

    private void UpdateLibrary(List<WeaponData> weaponData)
    {
        foreach (var data in weaponData)
        {
            Library.Add(data.ID, new Weapon(data));
            Debug.Log(data.name + " has been added to WeaponLibrary. (ID: " + data.ID + ")");
        }
    }
    
    
}
