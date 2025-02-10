using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static UnitInfo unitInfo { get; private set; }
    public Vector3Int CellLocation { get; set; }
    
    public static Unit Initialize(Vector3Int initLocation)
    {
        GameObject gameObj = new GameObject("Unit");
        Unit unit = gameObj.AddComponent<Unit>();
        unitInfo = gameObj.AddComponent<UnitInfo>();
        unit.CellLocation = initLocation;
        
        UnitRenderer unitRenderer = gameObj.AddComponent<UnitRenderer>();
        unitRenderer.Render(unit.CellLocation);
        
        return unit;
    }
}
