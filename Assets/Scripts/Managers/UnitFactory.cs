using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitFactory
{
    public static Unit Create(GameObject prefab, Vector3Int initLocation, UnitDirection unitDirection) {
        // Instantiate the Prefab + Add Necessary Components
        GameObject instance = Object.Instantiate(prefab);
        instance.AddComponent<BillboardEffect>();

        // Assign a SpriteRenderer to UnitRenderer
        SpriteRenderer spriteRenderer = instance.GetComponent<SpriteRenderer>() 
            ? instance.GetComponent<SpriteRenderer>()
            : instance.AddComponent<SpriteRenderer>();
        UnitRenderer unitRenderer = new UnitRenderer(spriteRenderer);
        
        // Access the Unit Definition Data 
        UnitDefinitionData unitData = instance.GetComponent<UDDLoader>().LoadedUDD;

        // If the Unit is to be AI-controlled, create an AI Unit...
        // Otherwise, create a normal Unit
        if (unitData.IsUnitAIControlled) {
            AIUnit aiUnit = new AIUnit(instance, unitData, unitRenderer);
            aiUnit.InitializeAI(initLocation, unitDirection);
            return aiUnit;
        }
        
        Unit unit = new Unit(instance, unitData, unitRenderer);
        unit.Initialize(initLocation, unitDirection);
        return unit;
    }
}
