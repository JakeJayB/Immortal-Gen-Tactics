using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class UnitActionLibrary : MonoBehaviour
{
    private static Dictionary<int, Func<UnitAction>> UnitActions { get; set; } = new()
    {
        {0, () => new SplashSpell()},
        {1, () => new Heal()},
        {2, () => new Revive()}
    };

    private static Dictionary<int, Func<UnitAction>> Items { get; set; } = new()
    {
        {100, () => new Potion()},
        {101, () => new Ether()}
    };
    
#if UNITY_EDITOR
    public static List<(int id, string name)> GetUnitActionDropdownOptions()
    {
        return UnitActions
            .Select(pair => (pair.Key, pair.Value.Invoke().Name))
            .OrderBy(t => t.Key)
            .ToList();
    }

    public static List<(int id, string name)> GetItemDropdownOptions()
    {
        return Items
            .Select(pair => (pair.Key, pair.Value.Invoke().Name))
            .OrderBy(t => t.Key)
            .ToList();
    }
#endif
}
