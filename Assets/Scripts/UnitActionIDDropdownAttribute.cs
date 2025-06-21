using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitActionType { Action, Item }

public class UnitActionIDDropdownAttribute : PropertyAttribute
{
    public UnitActionType Type { get; private set; }

    public UnitActionIDDropdownAttribute(UnitActionType type)
    {
        Type = type;
    }
    
}
