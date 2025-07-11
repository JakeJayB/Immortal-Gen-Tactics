using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitFactory
{
    public static Unit Create(GameObject prefab, Vector3Int initLocation, UnitDirection unitDirection) {
        GameObject instance = Object.Instantiate(prefab);
        
        switch (instance.gameObject.GetComponent<UnitInfo>().UnitAffiliation) {
            case UnitAffiliation.Player:
                Unit unit = new Unit(instance);
                unit.Initialize(initLocation, unitDirection);
                return unit;
            case UnitAffiliation.Enemy:
                AIUnit aiUnit = new AIUnit(instance);
                aiUnit.InitializeAI(initLocation, unitDirection);
                return aiUnit;
        }
        
        return null;
    }
}
