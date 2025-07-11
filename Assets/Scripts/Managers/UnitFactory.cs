using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitFactory
{
    public static Unit Create(GameObject prefab, Vector3Int initLocation, UnitDirection unitDirection) {
        GameObject instance = Object.Instantiate(prefab);
        instance.AddComponent<BillboardEffect>();
        
        UnitDefinitionData unitData = instance.GetComponent<UDDLoader>().LoadedUDD;

        if (unitData.IsUnitAIControlled) {
            AIUnit aiUnit = new AIUnit(instance, unitData);
            aiUnit.InitializeAI(initLocation, unitDirection);
            return aiUnit;
        }
        
        Unit unit = new Unit(instance, unitData);
        unit.Initialize(initLocation, unitDirection);
        return unit;
    }
}
