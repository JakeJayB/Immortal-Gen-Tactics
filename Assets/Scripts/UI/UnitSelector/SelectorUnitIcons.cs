using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SelectorUnitIcons : MonoBehaviour
{
    private static GameObject unitIcon;

    private static Dictionary<int, Vector2Int> activeUnits = new Dictionary<int, Vector2Int>();
    private static int currentIdx = 0;

    void Awake()
    {
        InstantiateUnitPanel();
    }

    private void InstantiateUnitPanel()
    {

        // Create plane white background panel for unit icons


        // Get list of units from PartyManager.cs
        // Get sprite of the 0th unit in the list
        // Create and store a new GameObject for the unit icon
    }

    public static void NextUnit()
    {
        // increment currentIdx
        // if currentIdx is greater than the number of units, set currentIdx to 0

        // get unit from PartyManager.cs at currentIdx
        // set unitIcon to new unit's icon

        // if currentIdx is in activeUnits, shade unitIcon
    }

    public static void PreviousUnit()
    {
        // decrement currentIdx
        // if currentIdx is less than 0, set currentIdx to the number of units - 1

        // get unit from PartyManager.cs at currentIdx
        // set unitIcon to new unit's icon
    }



    public static void isUnitActive()
    {
        // if currentIdx is in activeUnits, return true
        // else return false
    }


    public static Unit GetUnit()
    {
        return null;
    }



    public static void GetUnit(Vector2Int cellLocation)
    {
        // if currentIdx is in activeUnits, 

        // get the unit from PartyManager.cs at currentIdx
        // Add currentIdx to activeUnits, set to value cellLocation
        // shade unitIcon
    }

    public static void ShadeUnitIcon()
    {
        // shade unitIcon at currentIdx
    }
}
