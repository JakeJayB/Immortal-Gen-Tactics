using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Vector3Int CellLocation { get; set; }
    
    public static Unit Initialize(Vector3Int initLocation)
    {
        GameObject gameObj = new GameObject("Unit");
        Unit unit = gameObj.AddComponent<Unit>();
        unit.CellLocation = initLocation;
        
        UnitRenderer unitRenderer = gameObj.AddComponent<UnitRenderer>();
        unitRenderer.Render(unit.CellLocation);
        
        return unit;
    }
}
