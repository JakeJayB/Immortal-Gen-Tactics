using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public GameObject gameObj { get; private set; }

    public UnitInfo unitInfo { get; set; }

    public UnitEquipment unitEquipment { get; set; }

    public UnitRenderer unitRenderer { get; set; }

    public static Unit Initialize(Vector3Int initLocation)
    {
        GameObject gameObj = new GameObject("Unit " + initLocation);
        Unit unit = gameObj.AddComponent<Unit>();
        unit.gameObj = gameObj;

        unit.unitInfo = gameObj.AddComponent<UnitInfo>();
        unit.unitInfo.CellLocation = initLocation;

        //unitEquipment = gameObj.AddComponent<UnitEquipment>();
        unit.unitEquipment = new UnitEquipment(unit.unitInfo);

        unit.unitRenderer = gameObj.AddComponent<UnitRenderer>();
        unit.unitRenderer.Render(initLocation);

        unit.gameObj.AddComponent<BillboardEffect>();
        return unit;
    }

/*    public void Initialize(Vector3Int initLocation)
    {
        GameObject gameObj = new GameObject("Unit " + initLocation);
        unitInfo = gameObj.AddComponent<UnitInfo>();
        unitInfo.CellLocation = initLocation;

        //unitEquipment = gameObj.AddComponent<UnitEquipment>();
        unitEquipment = new UnitEquipment(unitInfo);

        UnitRenderer unitRenderer = gameObj.AddComponent<UnitRenderer>();
        unitRenderer.Render(initLocation);
        
    }*/
}
